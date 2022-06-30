using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardMenusDisplay : MonoBehaviour {

    private List<GameObject> cardsList = new List<GameObject>();
    [SerializeField] private RectTransform cardsDisplayContent;
    [SerializeField] private bool displayEquipped;
    [SerializeField] private TextMeshProUGUI displayToggleLabel;

    private int sorting;

    public void SortCards(int sortBy) {
        sorting = sortBy;
        //Remove the current display of cards
        GameObject cardDisplay;
        while (cardsList.Count > 0) {
            cardDisplay = cardsList[0];
            cardsList.RemoveAt(0);
            Destroy(cardDisplay.gameObject);
        }

        //Grab card selection
        List<Card> cards = new List<Card>();
        switch ((Enums.Affinity)sortBy) {
            case Enums.Affinity.Chalice:
                if (displayEquipped) cards.AddRange(PlayerData.ChaliceCards);
                else cards.AddRange(PlayerData.ChaliceCardsOwned);
                break;
            case Enums.Affinity.Pentacle:
                if (displayEquipped) cards.AddRange(PlayerData.PentacleCards);
                else cards.AddRange(PlayerData.PentacleCardsOwned);
                break;
            case Enums.Affinity.Staff:
                if (displayEquipped) cards.AddRange(PlayerData.StaffCards);
                else cards.AddRange(PlayerData.StaffCardsOwned);
                break;
            case Enums.Affinity.Sword:
                if (displayEquipped) cards.AddRange(PlayerData.SwordCards);
                else cards.AddRange(PlayerData.SwordCardsOwned);
                break;
            default:
                if (displayEquipped) {
                    cards.AddRange(PlayerData.ChaliceCards);
                    cards.AddRange(PlayerData.PentacleCards);
                    cards.AddRange(PlayerData.StaffCards);
                    cards.AddRange(PlayerData.SwordCards);
                } else {
                    cards.AddRange(PlayerData.ChaliceCardsOwned);
                    cards.AddRange(PlayerData.PentacleCardsOwned);
                    cards.AddRange(PlayerData.StaffCardsOwned);
                    cards.AddRange(PlayerData.SwordCardsOwned);
                }
                break;
        }

        //Sort cards by name
        Card[] cardSort = cards.ToArray();
        cards.Sort(delegate (Card x, Card y) {
            if (string.IsNullOrEmpty(x.Name) && string.IsNullOrEmpty(y.Name)) return 0;
            else if (string.IsNullOrEmpty(x.Name)) return -1;
            else if (string.IsNullOrEmpty(y.Name)) return 1;
            else return x.Name.CompareTo(y.Name);
        });

        //Display cards
        GameObject display;
        foreach (Card c in cards) {
            display = Instantiate(Resources.Load<GameObject>("UserInterface/CardDisplay"));
            display.GetComponent<CardDisplayController>().CardData = c;
            cardsList.Add(display);
            display.transform.SetParent(cardsDisplayContent);
        }
    }

    public void ToggleDisplay() {
        displayEquipped = !displayEquipped;
        if (displayEquipped) displayToggleLabel.text = "Deck";
        else displayToggleLabel.text = "All";
        SortCards(sorting);
    }
}
