using UnityEngine;
using System.Collections;
using NetherWars;


public class CardController : MonoBehaviour {


	#region Serialized fields
	 
	[SerializeField]
	private GameObject _cardContainer;

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

	#endregion


	#region Private
	
	private NWCard _card;

	#endregion


	#region Card Events

	public delegate void CardHoverDelegate(CardController card);
	public delegate void CardEndHoverDelegate(CardController card);
	public delegate void CardClickedDelegate(CardController card);
	public delegate void CardIsDraggedDelegate(CardController card, Vector3 mousePosition);
	public delegate void CardBeginDragingDelegate(CardController card, Vector3 mousePosition);
	public delegate void CardEndDragingDelegate(CardController card, Vector3 mousePosition);

	public event CardHoverDelegate OnCardHover;
	public event CardEndHoverDelegate OnCardEndHover;
	public event CardClickedDelegate OnCardClicked;
	public event CardIsDraggedDelegate OnCardIsDragged;
	public event CardBeginDragingDelegate OnCardStartDraging;
	public event CardEndDragingDelegate OnCardEndDraging;

	#endregion

	#region Initialization
	
	public void SetCard(NWCard card)
	{
		_card = card;
		UpdateCard();
	}

	#endregion


	#region Setters

	public bool IsFlipped {
		get {
			return _isFlipped;
		}
		set {
			_isFlipped = value;
			if (_isFlipped)
			{
				_cardContainer.transform.localRotation = Quaternion.Euler(new Vector3(90, -180, 0));
				_manaCostLabel.gameObject.SetActive(false);
				_manaGainLabel.gameObject.SetActive(false);
				_powerLabel.gameObject.SetActive(false);
				_toughnessLabel.gameObject.SetActive(false);
				_descriptionLabel.gameObject.SetActive(false);
				_nameLabel.gameObject.SetActive(false);
			}
			else
			{
				_cardContainer.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
				_manaCostLabel.gameObject.SetActive(true);
				_manaGainLabel.gameObject.SetActive(true);
				_powerLabel.gameObject.SetActive(true);
				_toughnessLabel.gameObject.SetActive(true);
				_descriptionLabel.gameObject.SetActive(true);
				_nameLabel.gameObject.SetActive(true);
			}
		}
	}

	#endregion


	#region Getters
	
	public int UniqueId
	{
		get
		{
			return _card.CardUniqueID;
		}
	}
	
	#endregion


	#region Private

	private void UpdateCard()
	{
		_manaCostLabel.text = _card.CastingCost.ToString();
		_manaGainLabel.text = "1";
		_powerLabel.text = _card.Power.ToString();
		_toughnessLabel.text = _card.Toughness.ToString();
		_descriptionLabel.text = "";
		_nameLabel.text = _card.CardName;
	}

	#endregion


	#region Public - User Interactions

	public void CardHover()
	{
		if (OnCardHover != null)
		{
			OnCardHover(this);
		}
	}

	public void CardEndHover()
	{
		if (OnCardEndHover != null)
		{
			OnCardEndHover(this);
		}
	}

	public void CardClicked()
	{
		if (OnCardClicked != null)
		{
			OnCardClicked(this);
		}
	}

	public void CardDragged(Vector3 mousePosition)
	{
		if (OnCardIsDragged != null)
		{
			OnCardIsDragged(this, mousePosition);
		}
	}

	public void CardStartDraging(Vector3 mousePosition)
	{
		if (OnCardStartDraging != null)
		{
			OnCardStartDraging(this, mousePosition);
		}
	}

	public void CardEndDraging(Vector3 mousePosition)
	{
		if (OnCardEndDraging != null)
		{
			OnCardEndDraging(this, mousePosition);
		}
	}

	#endregion

}
