using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DeckBuilder : MonoBehaviour {
    
    private static DeckBuilder instance;
    public static DeckBuilder Instance { get { return instance; } }

    private int cardsToChoose;
    private List<CardDisplayController> selectedCards = new List<CardDisplayController>();

    private List<GameObject> draftCards = new List<GameObject>();
    private List<GameObject> currentCards = new List<GameObject>();

    [SerializeField] private RectTransform currentCardsDisplay;
    [SerializeField] private RectTransform draftCardsDisplay;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI cardsLeftText;

    public void Awake(){
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (this != instance) {
            Destroy(gameObject);
        }
    }

    public void StartDraft(CardDraftPool cardPool) {
        cardsToChoose = cardPool.cardsToChoose;
        GameObject display;
        foreach (Card c in cardPool.cardList) {
            display = Instantiate(Resources.Load<GameObject>("UserInterface/CardDisplaySelect"));
            display.GetComponentInChildren<CardDisplayController>().CardData = c;
            draftCards.Add(display);
            display.transform.SetParent(draftCardsDisplay);
        }
        SortCurrentCards(0);
    }

    public void AddSelection(CardDisplayController card) {
        if (selectedCards.Count == cardsToChoose)
            selectedCards[0].SelectCard();
        selectedCards.Add(card);

        cardsLeftText.text = (cardsToChoose - selectedCards.Count).ToString();
        if (selectedCards.Count == cardsToChoose)
            confirmButton.interactable = true;
    }
    public void RemoveSelection(CardDisplayController card) {
        selectedCards.Remove(card);
        confirmButton.interactable = false;
        cardsLeftText.text = (cardsToChoose - selectedCards.Count).ToString();
    }

    public void ClearSelection() {
        while (selectedCards.Count > 0)
            selectedCards[0].SelectCard();
    }
    public void ConfirmSelection() {
        foreach (CardDisplayController card in selectedCards) {
            Debug.Log("<color=purple>Card added to deck</color>: " + card.CardData.Name 
                + " added to " + card.CardData.Affinity + " deck");
            switch(card.CardData.Affinity) {
                case Enums.Affinity.Chalice:
                    PlayerData.ChaliceCards.Add(card.CardData);
                    break;
                case Enums.Affinity.Pentacle:
                    PlayerData.PentacleCards.Add(card.CardData);
                    break;
                case Enums.Affinity.Staff:
                    PlayerData.StaffCards.Add(card.CardData);
                    break;
                case Enums.Affinity.Sword:
                    PlayerData.SwordCards.Add(card.CardData);
                    break;
            }
        }
        selectedCards.Clear();
        GameObject cardDisplay;
        while (draftCards.Count > 0) {
            cardDisplay = draftCards[0];
            draftCards.RemoveAt(0);
            Destroy(cardDisplay.gameObject);
        }
        while (currentCards.Count > 0) {
            cardDisplay = currentCards[0];
            currentCards.RemoveAt(0);
            Destroy(cardDisplay.gameObject);
        }
        confirmButton.interactable = false;
        UIManager.Instance.EndDraft();
    }

    public void SortCurrentCards (int sortBy) {
        //Remove the current display of cards
        GameObject cardDisplay;
        while (currentCards.Count > 0) {
            cardDisplay = currentCards[0];
            currentCards.RemoveAt(0);
            Destroy(cardDisplay.gameObject);
        }

        //Grab card selection
        List<Card> cards = new List<Card>();
        switch ((Enums.Affinity) sortBy) {
            case Enums.Affinity.Chalice:
                cards.AddRange(PlayerData.ChaliceCards);
                break;
            case Enums.Affinity.Pentacle:
                cards.AddRange(PlayerData.PentacleCards);
                break;
            case Enums.Affinity.Staff:
                cards.AddRange(PlayerData.StaffCards);
                break;
            case Enums.Affinity.Sword:
                cards.AddRange(PlayerData.SwordCards);
                break;
            default:
                cards.AddRange(PlayerData.ChaliceCards);
                cards.AddRange(PlayerData.PentacleCards);
                cards.AddRange(PlayerData.StaffCards);
                cards.AddRange(PlayerData.SwordCards);
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
            currentCards.Add(display);
            display.transform.SetParent(currentCardsDisplay);
        }
    }
}
