using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Draft Pool", menuName = "Card Draft Pool")]
[System.Serializable]
public class CardDraftPool : ScriptableObject {
    public List<Card> cardList = new List<Card>();
    public int cardsToChoose = 4;
}