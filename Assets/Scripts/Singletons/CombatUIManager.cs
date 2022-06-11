using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUIManager : MonoBehaviour {

    private static CombatUIManager instance;

    [Header("UI Elements")]
    [SerializeField] private DisplayText displayText;
    [SerializeField] private GameObject CardRevealDisplay;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private Button[] cardSlots = new Button[4];
    [SerializeField] private TextMeshProUGUI[] cardTracker = new TextMeshProUGUI[4];

    [Header("Character Info")]
    [SerializeField] private List<CharacterDisplayController> playerInfo;
    [SerializeField] private List<BossHealth> enemyHealth;

    [Header("Card Places")]
    [SerializeField] private HandDisplayController hand;
    [SerializeField] private GameObject cardDropZone;
    [SerializeField] private GameObject cardDiscardDropZone;

    [Header("UI Menus")]
    [SerializeField] private GameObject combatUI;
    [SerializeField] private GameObject symbolsUI;

    private bool enableDrag = false;
    private DamageText PopupDamageText;

    public static CombatUIManager Instance { get { return instance; } }
    public List<CharacterDisplayController> PlayerInfo { get { return playerInfo; } }
    public HandDisplayController Hand { get { return hand; } }
    public bool EnableDrag { get { return enableDrag; } }

    void Awake () {
        if (instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this);
        PopupDamageText = Resources.Load<DamageText>("Prefabs/DamagePopUp");
    }

    public void StartDraw() {
        for (int i = 0; i < 4; i++) {
            cardSlots[i].interactable = true;
        }
    }

    public void EndDraw() {
        for (int i = 0; i < 4; i++) {
            cardSlots[i].interactable = false;
        }
        UpdateCards();
    }

    public void StartPlanning() {
        endTurnButton.interactable = true;
        enableDrag = true;
    }

    public void EndPlanning() {
        endTurnButton.interactable = false;
        enableDrag = false;
        GameManager.Instance.EndPlanning();
    }

    public void SetDamageText (int value, Transform location) {
        DamageText instance = Instantiate(PopupDamageText);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(this.transform, false);
        instance.transform.position = screenPos;
        instance.SetText(Mathf.Abs(value).ToString());
    }
    
    public void SetDamageText (int value, Transform location, Color32 color) {
        DamageText instance = Instantiate(PopupDamageText);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(this.transform, false);
        instance.transform.position = screenPos;
        instance.SetText(Mathf.Abs(value).ToString());
        instance.SetColor(color);
    }

    public IEnumerator DisplayMessage (string msg, float duration = 1f) {
        displayText.SetMessage(msg);
        yield return new WaitForSeconds(duration);
        displayText.StartFade();
    }

    public void SetMessage(string msg){
        displayText.SetMessage(msg);
    }

    public void HideMessage(){
        displayText.StartFade();
    }

    public IEnumerator RevealCard(Card card, float duration = 1f){
        CardDisplayController display = CardDisplayController.CreateCard(card);
        display.GetComponent<Draggable>().followMouse = false;

        RectTransform cardRectTransform = display.GetComponent<RectTransform>();
        RectTransform DisplayArea = CardRevealDisplay.GetComponent<RectTransform>();
        cardRectTransform.SetParent(DisplayArea.transform);
        cardRectTransform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(duration);
        displayText.SetMessage("Press any key to continue");
        while(!Input.anyKey){
            yield return new WaitForEndOfFrame();
        }
        displayText.StartFade();

        Destroy(display.gameObject);
    }

    //The list given will given will be modified to contain only the selected card
    public IEnumerator DisplayChoice(List<Card> choices){

        List<CardDisplayController> displays = new List<CardDisplayController>();
        int selectedIndex = -1;
        
        CardRevealDisplay.active = true;
        CardRevealDisplay.GetComponent<Canvas>().enabled = true;

        foreach (Card card in choices){
            CardDisplayController display = CardDisplayController.CreateCard(card);
            display.GetComponent<Draggable>().followMouse = false;
            display.GetComponent<Draggable>().planningPhaseOnly = false;

            display.transform.SetParent(displayText.transform);

            RectTransform cardRectTransform = display.GetComponent<RectTransform>();
            RectTransform DisplayArea = CardRevealDisplay.GetComponent<RectTransform>();
            cardRectTransform.SetParent(DisplayArea.transform);


            displays.Add(display);
            display.GetComponent<Draggable>().onDragStart += (drag, drop) => {
                selectedIndex = displays.IndexOf(display);
            };
        }

        while(selectedIndex == -1){
            yield return new WaitForEndOfFrame();
        }

        var choice = choices[selectedIndex];
        choices.Clear();
        choices.Add(choice);

        CardRevealDisplay.GetComponent<Canvas>().enabled = false;
        CardRevealDisplay.active = false;

        foreach (CardDisplayController display in displays){
            Destroy(display.gameObject);
        }
        
    }

    public void SetInfo(Character pChar, int index) {
        playerInfo[index].Character = pChar;
        playerInfo[index].StatusDisplay.Character = pChar;
    }

    public void SetInfo(EnemyCharacter eChar, int index) {
        if (index > -1) {
            enemyHealth[index].Character = eChar;
            enemyHealth[index].StatusDisplay.Character = eChar;
        }
    }

    public void UpdateCards() {
        cardTracker[0].text = GameManager.Instance.decks[Enums.Affinity.Chalice].CardList.Count.ToString();
        cardTracker[1].text = GameManager.Instance.decks[Enums.Affinity.Pentacle].CardList.Count.ToString();
        cardTracker[2].text = GameManager.Instance.decks[Enums.Affinity.Staff].CardList.Count.ToString();
        cardTracker[3].text = GameManager.Instance.decks[Enums.Affinity.Sword].CardList.Count.ToString();
    }
    public void UpdateCharacters() {
        for (int i = 0; i < playerInfo.Count; i++) {
            playerInfo[i].StatusDisplay.UpdateEffects();
        }
        for (int i = 0; i < enemyHealth.Count; i++) {
            enemyHealth[i].StatusDisplay.UpdateEffects();
        }
    }

    public void ToggleEndTurnButton (bool toggle) {
        endTurnButton.interactable = toggle;
    }

    public void ToggleSymbolsUI (bool toggle) {
        symbolsUI.SetActive(toggle);
        combatUI.SetActive(!toggle);
    }

    public void Discard(bool toggle) {
        cardDiscardDropZone.SetActive(toggle);
        enableDrag = toggle;
        if (toggle)
            Debug.Log($"<color=Yellow>Discard Enabled</color>");
    }
}
