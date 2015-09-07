using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace NetherWars
{
	public class NWCardsLoader  {
		
		public static string CARDS_RESOURCE_PATH = "/Resources/Cards/";
		public static string CARDS_RESOURCE_EXTENSION = ".xml";
		
		private static List<NWCard> _loadedCardsList = new List<NWCard>();

		private static int _UniqueIdCount = 100;
		
		public static string[] GetAllCardsFilesPaths()
		{
			string dir = Application.dataPath;
			string[] filePaths = Directory.GetFiles(dir + NWCardsLoader.CARDS_RESOURCE_PATH, "*" + NWCardsLoader.CARDS_RESOURCE_EXTENSION);
			return filePaths; 
		}
		
		public static List<NWCard> LoadAllCards()
		{
			string[] cardsFilesPaths = GetAllCardsFilesPaths();
			List <NWCard> cards = new List<NWCard>();
			foreach (string cardFilePath in cardsFilesPaths)
			{
				//Debug.Log("cardFilePath: " + cardFilePath);
				NWCard card = NWCardsLoader.LoadCardFile(cardFilePath);
				if (card != null)
				{
					cards.Add(card);
				}
			}
			return cards; 
		}
		
		
		public static void SaveCardToFile(NWCard card)
		{
			string dir = Application.dataPath;
			string filePath = Path.Combine(dir + NWCardsLoader.CARDS_RESOURCE_PATH, card.CardID + NWCardsLoader.CARDS_RESOURCE_EXTENSION);
			Debug.Log("save card: " + filePath);
			var serializer = new XmlSerializer(typeof(NWCard));
			using(var stream = new FileStream(filePath, FileMode.Create))
			{
				serializer.Serialize(stream, card);
				stream.Close();
			}
		}


		public static NWCard LoadCardFile(string filePath)
		{
			var serializer = new XmlSerializer(typeof(NWCard));
			var stream = new FileStream(filePath, FileMode.Open);
			NWCard card = serializer.Deserialize(stream) as NWCard;
			stream.Close();
			card.CardUniqueID = ++_UniqueIdCount;
			_loadedCardsList.Add(card);
			return card;
		}



#if UNITY_STANDALONE
		public static NWCard LoadCard(int cardId, int uniqueId = -1)
		{
			TextAsset cardXML = Resources.Load<TextAsset>("Cards/" + cardId.ToString());
			XmlSerializer serializer = new XmlSerializer(typeof(NWCard));
			StringReader reader = new StringReader(cardXML.text);
			NWCard card = serializer.Deserialize(reader) as NWCard;
			reader.Close();
			Debug.Log("loaded card: " + card.CardID);
			if (uniqueId >= 0)
			{
				card.CardUniqueID = uniqueId;
			}
			else
			{
				card.CardUniqueID = ++_UniqueIdCount;
			}
			_loadedCardsList.Add(card);
			return card;
		}
#else
		public static NWCard LoadCard(int cardId)
		{
			string dir = Application.dataPath;
			string filePath = Path.Combine(dir + NWCardsLoader.CARDS_RESOURCE_PATH, cardId.ToString() + NWCardsLoader.CARDS_RESOURCE_EXTENSION);
			return NWCardsLoader.LoadCardFile(filePath);
		}

#endif
		public static List<NWCard> LoadCardList(int[] cardsList)
		{
			List<NWCard> loadedCards = new List<NWCard>();
			foreach (int cardId in cardsList)
			{
				NWCard card = NWCardsLoader.LoadCard(cardId);
				if (card != null)
				{
					loadedCards.Add(card);
				}
				else
				{
					Debug.LogError("ERROR loading card " + cardId);
				}
			}
			return loadedCards;
		}
		
		
		public static NWCard GetCardForId(int CardUniqueID)
		{
			foreach (NWCard card in _loadedCardsList)
			{
				if (card.CardUniqueID == CardUniqueID)
				{
					return card;
				}
			}
			return null;
		}
	}
}

