using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityUILogger : MonoBehaviour
{
	struct LogMessage
	{
		public string message;
		public string stackTrace;
		public LogType type;
		public string timeStamp;
	}
	
	#region Private Properties
	
	private List<LogMessage> _logMessagesHistory = new List<LogMessage>();
	private Vector2 _scrollPosition;
	private bool _displayConsole;
	private bool _collapse;
	private GameObject _canvasBackgorund;
	private static UnityUILogger _instance;
	const int MARGIN = 20;
	const float BUTTON_WIDTH_SCALE = 0.1f;
	private Rect _windowRect = new Rect(MARGIN, MARGIN, Screen.width - (MARGIN * 2), Screen.height - (MARGIN * 2));
	private Rect _titleBarRect = new Rect(0, 0, 10000, 20);
	private GUIContent _clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
	private GUIContent _closeLabel = new GUIContent("Close", "Close the Console.");
	private GUIContent _collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");
	private Dictionary<LogType, Color> _logTypeColors;
	private GUIStyle _guiStyle;
	
	#endregion
	
	
	#region Public Properties
	
	public bool EnableConsole = true;
	public bool EnableToggleButton = true;
	public KeyCode ToggleKey = KeyCode.Space;
	public int MaxLines = 1000;
	public bool DisplayTimeStamp = true;
	public Color AssertColor = Color.white;
	public Color ErrorColor = Color.red;
	public Color ExeptionColor = Color.red;
	public Color LogColor = Color.white;
	public Color WarningColor = Color.yellow;
	
	#endregion
	
	
	#region Initialization
	
	void Awake()
	{
		if (UnityUILogger._instance == null)
		{
			Application.RegisterLogCallback(HandleLog);
			
			UnityUILogger._instance = this;
			DontDestroyOnLoad(this.gameObject);
			_canvasBackgorund = new GameObject();
			_canvasBackgorund.AddComponent<Canvas>();
			_canvasBackgorund.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			_canvasBackgorund.transform.SetParent(this.gameObject.transform);
			_canvasBackgorund.name = "Console Canvas";
			_canvasBackgorund.AddComponent<GraphicRaycaster>();
			GameObject overlayPanel = new GameObject();
			overlayPanel.AddComponent<Image>();
			overlayPanel.transform.SetParent(_canvasBackgorund.transform);
			overlayPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
			overlayPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
			overlayPanel.GetComponent<RectTransform>().position = Vector3.zero;
			overlayPanel.GetComponent<RectTransform>().offsetMax = Vector2.zero;
			overlayPanel.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			overlayPanel.GetComponent<Image>().color = new Color(0,0,0,0);
			overlayPanel.AddComponent<BoxCollider2D>().size = new Vector2(Screen.width, Screen.height);
			_logTypeColors = new Dictionary<LogType, Color>()
			{
				{ LogType.Assert, AssertColor },
				{ LogType.Error, ErrorColor },
				{ LogType.Exception, ExeptionColor },
				{ LogType.Log, LogColor },
				{ LogType.Warning, WarningColor },
			};
			this.setGuiStyle();
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	private void setGuiStyle()
	{
		_guiStyle = new GUIStyle();
		_guiStyle.normal.textColor = Color.white;
		_guiStyle.fontSize = this.getAdaptiveToScreenFontSize();
	}

	private int getAdaptiveToScreenFontSize()
	{
		int fontSize = 0;

		int proportionModifierBetweenScreenAndFont = 40;

		int screenHeight = Screen.height;

		fontSize = screenHeight / proportionModifierBetweenScreenAndFont;

		return fontSize;
	}
	
	public static UnityUILogger Instance()
	{
		if (!Application.isPlaying)
		{
			return null;
		}
		if (UnityUILogger._instance == null)
		{
			GameObject consoleContainer = new GameObject();
			consoleContainer.name = "Console";
			DontDestroyOnLoad(consoleContainer);
			UnityUILogger._instance = consoleContainer.AddComponent<UnityUILogger>();
		}
		return UnityUILogger._instance;
	}
	
	#endregion
	
	
	#region Runtime
	
	void Update ()
	{
		if (Input.GetKeyDown(ToggleKey) && EnableConsole) {
			_displayConsole = !_displayConsole;
		}
	}
	
	void OnGUI ()
	{
		if (!_displayConsole) {
			if (_canvasBackgorund.activeInHierarchy)
			{
				_canvasBackgorund.SetActive(false);
			}
			
			if (EnableToggleButton && EnableConsole)
			{
				float buttonSize = Screen.width * BUTTON_WIDTH_SCALE;
				if (GUI.Button(new Rect(0, Screen.height - buttonSize, buttonSize, buttonSize), "+"))
				{
					_displayConsole = true;
				}
			}
			
			return;
		}
		if (!_canvasBackgorund.activeInHierarchy)
		{
			_canvasBackgorund.SetActive(true);
		}
		_windowRect = GUILayout.Window(9999, _windowRect, ConsoleWindow, "Console");
	}
	
	
	private void ConsoleWindow (int windowID)
	{
		_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
		for (int i = 0; i < _logMessagesHistory.Count; i++) {
			var log = _logMessagesHistory[i];
			if (_collapse) {
				var messageSameAsPrevious = i > 0 && log.message == _logMessagesHistory[i - 1].message;
				if (messageSameAsPrevious) {
					continue;
				}
			}
			GUI.contentColor = _logTypeColors[log.type];
			if (DisplayTimeStamp)
			{
				GUILayout.Label("[" + log.timeStamp + "]: " +  log.message, this._guiStyle);
			}
			else
			{
				GUILayout.Label(log.message, this._guiStyle);
			}
			
			if (log.type == LogType.Exception || log.type == LogType.Assert)
			{
				GUILayout.Label(log.stackTrace, this._guiStyle);
			}
		}
		GUILayout.EndScrollView();
		GUI.contentColor = Color.white;
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(_clearLabel, GUILayout.Height(Screen.width * BUTTON_WIDTH_SCALE))) {
			_logMessagesHistory.Clear();
		}
		if (GUILayout.Button(_closeLabel, GUILayout.Height(Screen.width * BUTTON_WIDTH_SCALE)))
		{
			_displayConsole = false;
		}
		_collapse = GUILayout.Toggle(_collapse, _collapseLabel, GUILayout.ExpandWidth(false), GUILayout.Height(Screen.width * BUTTON_WIDTH_SCALE));
		GUILayout.EndHorizontal();
		GUI.DragWindow(_titleBarRect);
	}
	
	#endregion
	
	
	#region Handle Messages
	
	private void HandleLog (string message, string stackTrace, LogType type)
	{

		_logMessagesHistory.Add(new LogMessage() {
			message = message,
			stackTrace = stackTrace,
			type = type,
			timeStamp = System.DateTime.Now.ToLongTimeString(),
		});
		
		if (_logMessagesHistory.Count > MaxLines)
		{
			_logMessagesHistory.RemoveAt(0);
		}
	}
	
	#endregion	
}
