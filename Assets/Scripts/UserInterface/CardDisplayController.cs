using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Draggable))]
public class CardDisplayController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
   
    public static GameObject cardDisplayPrefab;

    internal int Handx;
    private bool isSelected;
    private Card cardData;

    [Header("Card Display")]
    [SerializeReference] private TextMeshProUGUI name;
    [SerializeReference] private TextMeshProUGUI description;
    [SerializeReference] private Image front;
    [SerializeReference] private Image[] icons = new Image[5];
    [SerializeReference] private Image[] iconsBack = new Image[5];

    [Header("Display Properties")]
    [SerializeReference] private bool isCombatCard;
    [SerializeReference] private bool isSelectionCard;

    public bool Selected { get { return isSelected; } }

    //The public facing card of this class. This is where all the cards data (from _cardData) is returned
    public Card CardData {
        get{return cardData;}
        set {
            cardData = value;
            name.text = cardData.Name;

            if (!cardData.CorruptionPassDescription.Equals("") || !cardData.CorruptionFailDescription.Equals("")) {
                description.text = "Corruption Check:" + '\n'
                    + "Pass - " + cardData.CorruptionPassDescription + '\n'
                    + "Fail - " + cardData.CorruptionFailDescription + '\n' + '\n'
                    + cardData.Description;
            } else description.text = cardData.Description;

            if (cardData.Affinity == Enums.Affinity.Chalice) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Chalice");
            else if (cardData.Affinity == Enums.Affinity.Pentacle) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Pentacle");
            else if (cardData.Affinity == Enums.Affinity.Staff) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Staff");
            else if (cardData.Affinity == Enums.Affinity.Sword) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Sword");
            else if (cardData.Affinity == Enums.Affinity.Fool) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Fool");
            else if (cardData.Affinity == Enums.Affinity.Magician) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Magician");
            else if (cardData.Affinity == Enums.Affinity.Priestess) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Priestess");

            for (int i = 0; i < 5; i++) {
                if (cardData.Icons[i] != Enums.CardEffects.None) {
                    iconsBack[i].enabled = true;
                    iconsBack[i].sprite = GetSprite(cardData.IconsBack[i]);
                    icons[i].enabled = true;
                    icons[i].sprite = GetSprite(cardData.Icons[i]);
                }
            }
        }
    }

    public IEnumerator Play(PartyCharacter character) {
        if(character.Incapacitated){
            //Display some error
            StartCoroutine(CombatUIManager.Instance.DisplayMessage($"{character.data.name} is incapacitated and cannot play a card"));
            var card = CardData;
            GameManager.Instance.PlaceCardInHand(card);
            GameManager.Instance.RemoveCardFromHand(this);
            yield break;
        }
        character.CardToPlay = CardData;
        GetComponent<Draggable>().enabled = false;
        //Activate the cards designate targets function.
        yield return ResolveTargets();
        CombatUIManager.Instance.PlayerInfo[character.position].ChangeAction(character.Action, Enums.Action.Card);
        cardData.Caster = character;
        Destroy(gameObject);
    }

    public void SelectCard() {
        isSelected = !isSelected;
        if (isSelected) {
            front.sprite = Resources.Load<Sprite>("UserInterface/Card Fronts/Blank");
            DeckBuilder.Instance.AddSelection(this);
        } else {
            if (cardData.Affinity == Enums.Affinity.Chalice) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Chalice");
            else if (cardData.Affinity == Enums.Affinity.Pentacle) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Pentacle");
            else if (cardData.Affinity == Enums.Affinity.Staff) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Staff");
            else if (cardData.Affinity == Enums.Affinity.Sword) front.sprite
                    = Resources.Load<Sprite>("UserInterface/Card Fronts/Sword");
            DeckBuilder.Instance.RemoveSelection(this);
        }
    }

    
    public void Start() {
        //If this card is being used for combat, then set up the draggable
        if (isCombatCard) {
            GetComponent<Draggable>().enabled = true;
            GetComponent<Draggable>().zone = GameObject.FindGameObjectWithTag("HandDisplay").GetComponent<DropZone>();
            GetComponent<Draggable>().returnDropZone = GetComponent<Draggable>().zone;

            GetComponent<Draggable>().onDragStop += (drag, drop) => {
                if (drop != null && drop.GetComponent<PartyCharacter>() != null) {
                    if (cardData.Affinity == drop.GetComponent<PartyCharacter>().data.PrimaryAffinity ||
                        cardData.Affinity == drop.GetComponent<PartyCharacter>().data.SecondaryAffinity)
                        StartCoroutine(Play(drop.GetComponent<PartyCharacter>()));
                    else {
                        StartCoroutine(CombatUIManager.Instance.DisplayMessage(drop.GetComponent<PartyCharacter>().data.Name
                            + " cannot play " + cardData.Affinity + " cards", 3f));
                        GetComponent<Draggable>().zone = GameObject.FindGameObjectWithTag("HandDisplay").GetComponent<DropZone>();
                        GetComponent<Draggable>().returnDropZone = GetComponent<Draggable>().zone;
                        transform.SetParent(GetComponent<Draggable>().zone.transform);
                    }
                } else if (drop != null) {
                    if (drop.gameObject.transform.tag.Equals("DiscardDrop")) {
                        GameManager.Instance.Discard(CardData);
                        CombatUIManager.Instance.Hand.DisplayedCards.Remove(this);
                        Destroy(gameObject);
                    } else {
                        GetComponent<Draggable>().zone = GameObject.FindGameObjectWithTag("HandDisplay").GetComponent<DropZone>();
                        GetComponent<Draggable>().returnDropZone = GetComponent<Draggable>().zone;
                        transform.SetParent(GetComponent<Draggable>().zone.transform);
                    }
                }
            };
        }
        //If this card is being used for card selection, then set up being able to select it
        else if (isSelectionCard) {
            GetComponent<Button>().onClick.AddListener(delegate { SelectCard(); });
        }
        RectTransform rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(0.95f, 0.95f, 0.95f);
    }

    // Delete cards once targets have been designated
    public IEnumerator ResolveTargets() {
        CombatUIManager.Instance.ToggleEndTurnButton(false);
        CombatUIManager.Instance.EnableDrag = false;
        this.GetComponent<RectTransform>().position = new Vector3(175, 200, 0);

        Debug.Log($"<color=blue>{CardData.Name} </color>Designating target...");
        yield return CardData.DesignateTargets();
        GameManager.Instance.RemoveCardFromHand(this);

        //AudioManager.audioMgr.PlayUISFX("PlaceCard");

        CombatUIManager.Instance.EnableDrag = true;
        CombatUIManager.Instance.ToggleEndTurnButton(true);
    }

    //Create a new display of the card selected
    public static CardDisplayController CreateCard(Card card){

        if (cardDisplayPrefab == null)
            cardDisplayPrefab = Resources.Load<GameObject>("UserInterface/CardDisplayCombat");
        
        var cardGameObject = Instantiate<GameObject>(cardDisplayPrefab, Vector3.zero, Quaternion.identity);

        var cardDisplay = cardGameObject.GetComponent<CardDisplayController>();

        cardDisplay.CardData = card;

        //Debug.Log($"CDC: Creating card {card}");

        return cardDisplay;
    }

    private Sprite GetSprite(Enums.CardEffects cardEffects) {
        switch (cardEffects) {
            case Enums.CardEffects.Armored:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Armored");
            case Enums.CardEffects.Bleed:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Bleed");
            case Enums.CardEffects.Blind:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Blind");
            case Enums.CardEffects.Bolster:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Bolster");
            case Enums.CardEffects.Burn:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Burn");
            case Enums.CardEffects.Cleanse:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Cleanse");
            case Enums.CardEffects.Cold:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Cold");
            case Enums.CardEffects.Corrupt:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Corrupt");
            case Enums.CardEffects.Cripple:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Cripple");
            case Enums.CardEffects.Curse:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Curse");
            case Enums.CardEffects.Discard:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Discard");
            case Enums.CardEffects.Draw:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Draw");
            case Enums.CardEffects.Fire:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Fire");
            case Enums.CardEffects.Fortify:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Fortify");
            case Enums.CardEffects.Haste:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Haste");
            case Enums.CardEffects.Heal:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Heal");
            case Enums.CardEffects.Insert:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Insert");
            case Enums.CardEffects.Lightning:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Lightning");
            case Enums.CardEffects.Nature:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Nature");
            case Enums.CardEffects.Nullify:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Nullify");
            case Enums.CardEffects.Physical:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Physical");
            case Enums.CardEffects.Poison:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Poison");
            case Enums.CardEffects.Protect:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Protect");
            case Enums.CardEffects.Rejuvenate:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Rejuvenate");
            case Enums.CardEffects.Reshuffle:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Reshuffle");
            case Enums.CardEffects.Revive:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Revive");
            case Enums.CardEffects.Shroud:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Shroud");
            case Enums.CardEffects.Silence:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Silence");
            case Enums.CardEffects.Sleep:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Sleep");
            case Enums.CardEffects.Soothe:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Soothe");
            case Enums.CardEffects.Stun:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Stun");
            case Enums.CardEffects.Summon:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Summon");
            case Enums.CardEffects.Taunt:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Taunt");
            case Enums.CardEffects.Weaken:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Weaken");
            default:
                return Resources.Load<Sprite>("UserInterface/Card Icons/Blank");
        }
    }

    private Sprite GetSprite(Enums.CardIconBack cardIconBack) {
        switch (cardIconBack) {
            case Enums.CardIconBack.Corruption_Fail:
                return Resources.Load<Sprite>("UserInterface/Card Fronts/Icon Back Cor Fail");
            case Enums.CardIconBack.Corruption_Pass:
                return Resources.Load<Sprite>("UserInterface/Card Fronts/Icon Back Cor Pass");
            case Enums.CardIconBack.Card:
                switch (CardData.Affinity) {
                    case Enums.Affinity.Chalice:
                        return Resources.Load<Sprite>("UserInterface/Card Fronts/Icon Back Chalice");
                    case Enums.Affinity.Pentacle:
                        return Resources.Load<Sprite>("UserInterface/Card Fronts/Icon Back Pentacle");
                    case Enums.Affinity.Staff:
                        return Resources.Load<Sprite>("UserInterface/Card Fronts/Icon Back Staff");
                    case Enums.Affinity.Sword:
                        return Resources.Load<Sprite>("UserInterface/Card Fronts/Icon Back Sword");
                    default:
                        return Resources.Load<Sprite>("UserInterface/Card Fronts/Icon Back Boss");
                }
            default:
                return Resources.Load<Sprite>("UserInterface/Card Fronts/Icon Back Boss");
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        RectTransform rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
    public void OnPointerExit(PointerEventData eventData) {
        RectTransform rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(0.95f, 0.95f, 0.95f);
    }
}
