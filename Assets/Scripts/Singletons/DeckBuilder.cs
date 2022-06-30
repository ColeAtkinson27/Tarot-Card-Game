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

    [SerializeField] private RectTransform draftCardsDisplay;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI cardsLeftText;
    [SerializeField] private CardMenusDisplay cardDisplay;

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
        cardDisplay.SortCards(0);
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
        int equippedCards = PlayerData.EquippedCards;
        foreach (CardDisplayController card in selectedCards) {
            Debug.Log("<color=purple>Card added to deck</color>: " + card.CardData.Name 
                + " added to " + card.CardData.Affinity + " deck");
            switch(card.CardData.Affinity) {
                case Enums.Affinity.Chalice:
                    PlayerData.ChaliceCardsOwned.Add(card.CardData);
                    if (equippedCards < GlobalNumbers.MAX_DECK_SIZE) {
                        PlayerData.ChaliceCards.Add(card.CardData);
                        equippedCards++;
                    }
                    break;
                case Enums.Affinity.Pentacle:
                    PlayerData.PentacleCardsOwned.Add(card.CardData);
                    if (equippedCards < GlobalNumbers.MAX_DECK_SIZE) {
                        PlayerData.PentacleCards.Add(card.CardData);
                        equippedCards++;
                    }
                    break;
                case Enums.Affinity.Staff:
                    PlayerData.StaffCardsOwned.Add(card.CardData);
                    if (equippedCards < GlobalNumbers.MAX_DECK_SIZE) {
                        PlayerData.StaffCards.Add(card.CardData);
                        equippedCards++;
                    }
                    break;
                case Enums.Affinity.Sword:
                    PlayerData.SwordCardsOwned.Add(card.CardData);
                    if (equippedCards < GlobalNumbers.MAX_DECK_SIZE) {
                        PlayerData.SwordCards.Add(card.CardData);
                        equippedCards++;
                    }
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
        confirmButton.interactable = false;
        UIManager.Instance.EndDraft();
    }
}
