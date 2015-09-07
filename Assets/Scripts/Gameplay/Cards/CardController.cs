using UnityEngine;
using System.Collections;
using NetherWars;

public class CardController : MonoBehaviour {


	[SerializeField]
	private GameObject _frame;

	[SerializeField]
	private GameObject _back;

	[SerializeField]
	private GameObject _cardImage;

	[SerializeField]
	private TextMesh _manaGainLabel;

	[SerializeField]
	private TextMesh _manaCostLabel;

	[SerializeField]
	private TextMesh _toughnessLabel;

	[SerializeField]
	private TextMesh _powerLabel;

	[SerializeField]
	private TextMesh _descriptionLabel;

	[SerializeField]
	private TextMesh _nameLabel;


	[SerializeField]
	private bool _isFlipped;
	
	private NWCard _card;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetCard(NWCard card)
	{
		_card = card;
		UpdateCard();
	}


	public bool IsFlipped {
		get {
			return _isFlipped;
		}
		set {
			_isFlipped = value;
			if (_isFlipped)
			{
				this.transform.localRotation = Quaternion.Euler(new Vector3(270, 180, 0));
				_manaCostLabel.gameObject.SetActive(false);
				_manaGainLabel.gameObject.SetActive(false);
				_powerLabel.gameObject.SetActive(false);
				_toughnessLabel.gameObject.SetActive(false);
				_descriptionLabel.gameObject.SetActive(false);
				_nameLabel.gameObject.SetActive(false);
			}
			else
			{
				this.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
				_manaCostLabel.gameObject.SetActive(true);
				_manaGainLabel.gameObject.SetActive(true);
				_powerLabel.gameObject.SetActive(true);
				_toughnessLabel.gameObject.SetActive(true);
				_descriptionLabel.gameObject.SetActive(true);
				_nameLabel.gameObject.SetActive(true);
			}
		}
	}

	private void UpdateCard()
	{
		_manaCostLabel.text = _card.CastingCost.ToString();
		_manaGainLabel.text = "1";
		_powerLabel.text = _card.Power.ToString();
		_toughnessLabel.text = _card.Toughness.ToString();
		_descriptionLabel.text = "";
		_nameLabel.text = _card.CardName;
	}


	#region Getters

	public int UniqueId
	{
		get
		{
			return _card.CardUniqueID;
		}
	}

	#endregion

}
