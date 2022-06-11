using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class CharacterDisplayController : MonoBehaviour, IPointerClickHandler {
    [Header("Text")]
    [SerializeReference] private TextMeshProUGUI healthLabel;
    [SerializeReference] private TextMeshProUGUI corruptionLabel;
    [SerializeReference] private TextMeshProUGUI nameLabel;

    [Header("Status Images")]
    [SerializeReference] private Image deathMark;
    [SerializeReference] private Image profile;

    [SerializeReference] private GameObject turnHighlight;
    [SerializeReference] private Color DefaultColor;
    [SerializeField] private StatusDisplayController statusDisplay;

    private Enums.Action prevAction;

    private bool returnCard = true;
    private bool draw = false;

    public Button actionButton;

    public StatusDisplayController StatusDisplay { get { return statusDisplay; } }

    //private Dictionary<string, StatusEffectDisplay> statusEffects;

    [SerializeField]
    private Character character;
    public Character Character {
        get{
            return character;
        }
        set{
            character = value;
            //Set the fields based on the character data
            nameLabel.text = character.data.Name;
            profile.sprite = character.data.Avatar;
            //Subscribe to character events to continually update
            character.onStatChange += (string statName, ref int oldValue, ref int newValue) => {
                if (statName == "health") ChangeHealth(newValue);
                else if (statName == "corruption") ChangeCorruption(newValue);
            };

            ChangeHealth(Character.Health);
            ChangeCorruption(Character.Corruption);

            character.onActionChange += ChangeAction;
        }
    }
    public void ChangeHealth(int currentHealth) {
        if (Character.Defeated == true || Character.Health == 0) deathMark.enabled = true;
        else deathMark.enabled = false;
        healthLabel.text = currentHealth + "/" + Character.data.MaxHP;
    }
    public void ChangeCorruption(int currentCorruption) {
        corruptionLabel.text = currentCorruption.ToString() +"%";
    }

    public void ChangeAction(Enums.Action oldAction, Enums.Action newAction) {
        if(oldAction == newAction && oldAction != Enums.Action.Card) return;

        //ActionDisplay.text = "";

        switch (newAction){
            case Enums.Action.Attack:
                actionButton.gameObject.SetActive(false);
                break;
            case Enums.Action.Card:
                if (character.CardToPlay != null) {
                    actionButton.gameObject.SetActive(true);
                    switch (character.CardToPlay.Affinity) {
                        case Enums.Affinity.Chalice:
                            actionButton.image.sprite = Resources.Load<Sprite>("UserInterface/Card Icons/Icon_Chalice");
                            break;
                        case Enums.Affinity.Pentacle:
                            actionButton.image.sprite = Resources.Load<Sprite>("UserInterface/Card Icons/Icon_Pentacle");
                            break;
                        case Enums.Affinity.Staff:
                            actionButton.image.sprite = Resources.Load<Sprite>("UserInterface/Card Icons/Icon_Staff");
                            break;
                        case Enums.Affinity.Sword:
                            actionButton.image.sprite = Resources.Load<Sprite>("UserInterface/Card Icons/Icon_Sword");
                            break;
                    }
                }
                break;
            case Enums.Action.Stunned:
                actionButton.gameObject.SetActive(false);
                break;
            case Enums.Action.Draw:
                break;
        }

    }

    public void Start(){
        //actionButton.onClick.AddListener(() => {
        //    CheckAction();
        //});
    }

    public void highlightTurn(bool state) {
        if (state)
            this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        else
            this.GetComponent<Image>().color = new Color(1f, 1f, 1f, .75f);
    }

    //Toggles the graphic raycast component on all other (Slightly jank, a better method probably exists)
    private void ToggleRayCastOnOthers(bool enabled){
        var gr = GetComponent<GraphicRaycaster>();
        bool myToggleState = gr.enabled;
        //foreach(TurnOrderSlot slot in GameManager.manager.turnSlots){
        //    slot.currentTurnDraggable.GetComponent<GraphicRaycaster>().enabled = enabled;
        //}
        gr.enabled = myToggleState;
    }

    //Reset the character back to attacking if their display is right clicked
    public void OnPointerClick(PointerEventData d){
        if (GameManager.Instance.ActionsEnabled)
            if(d.button == PointerEventData.InputButton.Right && Character.CardToPlay != null){
                GameManager.Instance.PlaceCardInHand(Character.CardToPlay);
                Character.CardToPlay = null;
            }
    }

    //Reset the character back to attacking if their action button is clicked
    public void CheckAction() {
        if (GameManager.Instance.ActionsEnabled)
            if (Character.CardToPlay != null && returnCard == true) {
                GameManager.Instance.PlaceCardInHand(Character.CardToPlay);
                Character.CardToPlay = null;
            }
    }
}
