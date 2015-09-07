using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace NetherWars
{
	public class CardsEditor : EditorWindow {
		
		private List<NWCard> _loadedCards = null;
		private NWCard _selectedCard = null;
		
		
		private NWTriggerType _tmpTriggerSelction = NWTriggerType.None;
		
		
		[MenuItem ("Nether Wars/Cards Editor")]
		static void ShowWindow () {
			// Get existing open window or if none, make a new one:
			EditorWindow.GetWindow (typeof (CardsEditor));
			
		}
		
		void OnEnable()
		{
			ReloadCardList();
		}
		
		void OnGUI()
		{
			EditorGUILayout.BeginHorizontal();
			
			// cards list
			
			GUI.Box(new Rect(0,0,position.width * 0.3f , position.height), "");
			GUILayout.BeginArea(new Rect(0,0,position.width * 0.3f , position.height));
			
			if (_loadedCards != null)
			{
				foreach (NWCard card in _loadedCards)
				{
					if (GUILayout.Button(card.CardName + " " + card.CardID))
					{
						SelectCard(card);
					}
				}
			}
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(position.width * 0.3f ,0,position.width - (position.width * 0.3f) , position.height));
			
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Create New Card"))	{	CreateNewCard();		}
			if (GUILayout.Button("Save Card"))			{	SaveSelectedCard();		}
			if (GUILayout.Button("Delete Card"))		{ 	DeleteSelectedCard();	}
			EditorGUILayout.EndHorizontal();
			
			
			if (_selectedCard != null)
			{
				GUILayout.BeginArea(new Rect(0 ,40 ,position.width - (position.width * 0.3f) , position.height - 40));
				
				EditorGUILayout.BeginHorizontal();
				
				/*
			 * TODO: Draw the card texture
			 * 
			*/
				
				EditorGUILayout.BeginVertical();
				
				GUILayout.BeginArea(new Rect(180 , 0, position.width - (position.width * 0.3f) - 190, 270));
				
				_selectedCard.CardID = int.Parse( EditorGUILayout.TextField("Card Id: ", _selectedCard.CardID.ToString()));
				_selectedCard.CardName = EditorGUILayout.TextField("Card Name: ", _selectedCard.CardName);
				_selectedCard.CastingCost = EditorGUILayout.IntField("Casting Cost: ", _selectedCard.CastingCost);
				_selectedCard.Thrashold = EditorGUILayout.TextField("Thrashold: ", _selectedCard.Thrashold);
				
				EditorGUILayout.Space();
				
				EditorGUILayout.BeginHorizontal();
				// colors
				EditorGUILayout.BeginVertical();
				if (_selectedCard.CardColors != null)
				{
					foreach (NWColor color in Enum.GetValues(typeof(NWColor)))
					{
						bool isSelected = EditorGUILayout.Toggle(color.ToString() , _selectedCard.CardColors.Contains(color));
						if (isSelected)
						{
							if (!_selectedCard.CardColors.Contains(color))
							{
								_selectedCard.CardColors.Add(color);
							}
						}
						else
						{
							if (_selectedCard.CardColors.Contains(color))
							{
								_selectedCard.CardColors.Remove(color);
							}
						}
					}
				}
				
				EditorGUILayout.EndVertical();
				
				
				EditorGUILayout.BeginVertical();
				// Types
				if (_selectedCard.CardTypes != null)
				{
					foreach (NWCardType cardType in Enum.GetValues(typeof(NWCardType)))
					{
						bool isSelected = EditorGUILayout.Toggle(cardType.ToString() , _selectedCard.CardTypes.Contains(cardType));
						if (isSelected)
						{
							if (!_selectedCard.CardTypes.Contains(cardType))
							{
								_selectedCard.CardTypes.Add(cardType);
							}
						}
						else
						{
							if (_selectedCard.CardTypes.Contains(cardType))
							{
								_selectedCard.CardTypes.Remove(cardType);
							}
						}
					}
					
				}
				
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.EndHorizontal();
				
				
				if (_selectedCard.CardTypes != null && _selectedCard.CardTypes.Contains(NWCardType.Creature))
				{
					EditorGUILayout.BeginHorizontal();
					_selectedCard.Power = EditorGUILayout.IntField("Power: ", _selectedCard.Power);
					_selectedCard.Toughness = EditorGUILayout.IntField("Toughness: ", _selectedCard.Toughness);
					EditorGUILayout.EndHorizontal();
				}
				
				
				
				GUILayout.EndArea();
				
				
				// effects
				GUILayout.BeginArea(new Rect(10 , 270, position.width - (position.width * 0.3f) - 20.0f , 1000));
				
				
				EditorGUILayout.BeginVertical("Box");
				
				
				EditorGUILayout.BeginHorizontal();
				
				if (GUILayout.Button("Add Abillity"))
				{
					if (_selectedCard.Abilities == null)
					{
						_selectedCard.Abilities = new List<NWAbility>();
					}
					_selectedCard.Abilities.Add(new NWAbility());
				}
				
				EditorGUILayout.EndHorizontal();
				
				if (_selectedCard.Abilities != null)
				{
					foreach (NWAbility ability in _selectedCard.Abilities)
					{
						AbilityEdit(ability);
					}
					
					if ( _selectedCard.Abilities.Contains(null))
					{
						_selectedCard.Abilities.Remove(null);
					}
					
				}
				
				
				
				GUILayout.EndArea();
				
				
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.EndHorizontal();
				
				GUILayout.EndArea();
				
			}
			
			GUILayout.EndArea();
			
			EditorGUILayout.EndHorizontal();
		}
		
		
		private void SelectCard(NWCard card)
		{
			_selectedCard = card;
			
		}
		
		private void ReloadCardList()
		{
			Debug.Log("reload cards list");
			_loadedCards = NWCardsLoader.LoadAllCards();
		}
		
		private void CreateNewCard()
		{
			NWCard newCard = new NWCard();
			newCard.CardTypes = new List<NWCardType>();
			newCard.CardColors = new List<NWColor>();
			_selectedCard = newCard;
		}
		
		private void SaveSelectedCard()
		{
			NWCardsLoader.SaveCardToFile(_selectedCard);
			ReloadCardList();
		}
		
		private void DeleteSelectedCard()
		{
			
		}
		
		private void AbilityEdit(NWAbility ability)
		{
			if (ability != null)
			{
				EditorGUILayout.Space();
				
				EditorGUILayout.BeginVertical("Box");
				
				EditorGUILayout.BeginHorizontal();
				
				ability.Type = (NWAbilityType)EditorGUILayout.EnumPopup("Ability Type:", ability.Type);
				
				if (GUILayout.Button("Add Effect"))
				{
					if (ability.Effects == null)
					{
						ability.Effects = new List<NWEffect>();
					}
					NWEffect effect = new NWEffect();
					effect.Target = new NWTarget();
					effect.Count = new NWCount();
					ability.Effects.Add(effect);
				}
				
				if (GUILayout.Button("Delete Ability"))
				{
					Debug.Log("before: " + ability);
					DeleteAbility(ability);
					Debug.Log("after: " + ability);
					return;
				}
				
				EditorGUILayout.EndHorizontal();
				
				
				// triggers
				
				EditorGUILayout.BeginVertical("Box");
				
				switch (ability.Type)
				{
				case NWAbilityType.Activated:
				{
					break;
				}
				case NWAbilityType.Triggered:
				{
					if (ability.Trigger == null)
					{
						EditorGUILayout.BeginHorizontal();
						
						_tmpTriggerSelction = (NWTriggerType)EditorGUILayout.EnumPopup("Trigger Type:", _tmpTriggerSelction);
						if (GUILayout.Button("Create Trigger"))
						{
							ability.Trigger = new NWTrigger();
							ability.Trigger.Target = new NWTarget();
							ability.Trigger.Type = _tmpTriggerSelction;
							_tmpTriggerSelction = NWTriggerType.None;
						}
						EditorGUILayout.EndHorizontal();
					}
					
					else
					{
						EditorGUILayout.BeginHorizontal();
						ability.Trigger.Type = (NWTriggerType)EditorGUILayout.EnumPopup("Trigger Type:", ability.Trigger.Type);
						if (GUILayout.Button("Delete Trigger"))
						{
							ability.Trigger = null;
							return;
						}
						EditorGUILayout.EndHorizontal();
						
						switch (ability.Trigger.Type)
						{
						case NWTriggerType.EnterZone:
						{
							EditorGUILayout.BeginHorizontal();
							
							ability.Trigger.ToZone = (eZoneType)EditorGUILayout.EnumPopup("Zone To Enter: ", ability.Trigger.ToZone);
							
							EditTarget(ability.Trigger.Target);
							
							EditorGUILayout.EndHorizontal();
							break;
						}
						default:
						{
							break;
						}
						}
					}
					break;
				}
				case NWAbilityType.Static:
				{
					break;
				}
				case NWAbilityType.None:
				default:
				{
					break;
				}
				}
				
				EditorGUILayout.EndVertical();
				
				// Effects
				
				
				if (ability.Effects != null)
				{
					foreach (NWEffect effect in ability.Effects)
					{
						if (effect != null)
						{
							EditorGUILayout.BeginVertical("Box");
							
							EditorGUILayout.BeginHorizontal();
							effect.Type = (NWEffectType)EditorGUILayout.EnumPopup("Effect Type: ", effect.Type);
							effect.InfoText = EditorGUILayout.TextField("Info Text: ", effect.InfoText);
							if (GUILayout.Button("Delete Effect"))
							{
								ability.Effects[ability.Effects.IndexOf(effect)] = null;
								return;
							}
							EditorGUILayout.EndHorizontal();
							
							switch (effect.Type)
							{
							case NWEffectType.DrawCards:
							{
								EditTarget(effect.Target);
								EditCount(effect.Count);
								break;
							}
							default:
							{
								break;
							}
							}
							
							EditorGUILayout.EndVertical();
						}
						else
						{
							ability.Effects.Remove(effect);
							return;
						}
						
					}
				}
				
				
				
				
				EditorGUILayout.EndVertical();
				
			}
		}
		
		
		private void EditTarget(NWTarget target)
		{
			EditorGUILayout.BeginHorizontal();
			target.Type = (NWTargetType)EditorGUILayout.EnumPopup("Target: ", target.Type);
			EditorGUILayout.EndHorizontal();
		}
		
		private void EditCount(NWCount count)
		{
			EditorGUILayout.BeginHorizontal();
			count.Type = (NWCountType)EditorGUILayout.EnumPopup("Count: ", count.Type);
			switch (count.Type)
			{
			case NWCountType.Fixedvalue:
			{
				count.Value = EditorGUILayout.IntField("Value: ", count.Value);
				break;
			}
			default:
			{
				break;
			}
			}
			EditorGUILayout.EndHorizontal();
		}
		
		private void DeleteAbility(NWAbility ability)
		{
			if (_selectedCard != null)
			{
				if (_selectedCard.Abilities != null && _selectedCard.Abilities.Contains(ability))
				{
					_selectedCard.Abilities[_selectedCard.Abilities.IndexOf(ability)] = null;
				}
			}
		}
		
	}
}

