using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour { 
    private static GameManager instance;

    [Header("Arena Information")]
    [SerializeField] private Transform[] playerPositions = new Transform[4];
    [SerializeField] private Transform lookPosition;
    [SerializeField] private CardDraftPool Reward;

    [Header("Combatants")]
    [SerializeField] private List<Character> party = new List<Character>();
    [SerializeField] private List<EnemyCharacter> foes = new List<EnemyCharacter>();
    [SerializeField] private List<ITurnExecutable> turns = new List<ITurnExecutable>();

    [Header("Turn Information")]
    [SerializeField] private Enums.GameplayPhase phase;
    //Speeds the delay between phases, should also be applied to animations
    [SerializeField] private float speedScale;
    [SerializeField] private bool actionsEnabled = false;

    [Header("Card Information")]
    public Dictionary<Enums.Affinity, Deck> decks = new Dictionary<Enums.Affinity, Deck>();

    private int turnNumber = 0;
    private IEnumerator battleEnumerator;
    private bool gameOver = false;

    public delegate void PhaseChangeHandler(Enums.GameplayPhase phase);
    public event PhaseChangeHandler onPhaseChange;

    public static GameManager Instance { get { return instance; } }
    public Character Party(int index) {
        if (index < 4) return party[index];
        else return null;
    }
    public List<Character> Party() { return new List<Character>(party); }
    public Character Foes(int index) {
        if (index < foes.Count) return foes[index];
        else return null;
    }
    public List<EnemyCharacter> Foes() { return new List<EnemyCharacter>(foes); }
    public Enums.GameplayPhase Phase { get { return phase; } set { phase = value; } }
    public bool ActionsEnabled { get { return actionsEnabled; } set { actionsEnabled = value; } }

    private void Awake() {
        if (instance != null) {
            Destroy(this);
        }
        instance = this;
    }
    void Start() {
        StartCoroutine(UIManager.Instance.OpenCurtains());
        InitializeCharacters();
        InitializeDecks();
        battleEnumerator = ExecuteBattle();
        StartCoroutine(battleEnumerator);
    }

    //Loops through turns until the battleEnumerator is stopped (By CheckGameOver)
    public IEnumerator ExecuteBattle() {
        foreach (Character c in party)
            turns.Add(c);
        foreach (EnemyCharacter c in foes) {
            AddOpponent(c);
            turns.Insert(c.data.Speed, c);
        }
        if (decks[Enums.Affinity.Chalice].CardList.Count == 16) {
            Draw(Enums.Affinity.Chalice);
            Draw(Enums.Affinity.Chalice);
            Draw(Enums.Affinity.Chalice);
            Draw(Enums.Affinity.Chalice);
        } else {
            Draw(Enums.Affinity.Chalice);
            Draw(Enums.Affinity.Pentacle);
            Draw(Enums.Affinity.Staff);
            Draw(Enums.Affinity.Sword);
        }
        while (true) {
            yield return ExecutePlanning();
            yield return ExecuteTurn();
            yield return ExecuteDrawPhase();
        }
        
    }

    // Planning phase
    public IEnumerator ExecutePlanning() {
        phase = Enums.GameplayPhase.Planning;
        CombatUIManager.Instance.StartPlanning();
        actionsEnabled = true;

        //For each turn in turnSlots, enabled return card button
            //If character is not defeated, enable their character button.
            //If the current list of foes does not contain the character type used for this turn, then brighten their turn slot.

        if (onPhaseChange != null) {
            onPhaseChange(phase);
        }
        Debug.Log("<color=yellow>Planning Phase</color>");
        while(phase == Enums.GameplayPhase.Planning) {
            yield return new WaitForEndOfFrame();
        }
        CombatUIManager.Instance.EndPlanning();
    }

    // Turn/action phase
    public IEnumerator ExecuteTurn() {
        turnNumber++;

        actionsEnabled = false;
        //UI turn resolving starts
        phase = Enums.GameplayPhase.Resolve;
        
        Debug.Log("<color=yellow>Resolving Phase</color>");
        //turns = new List<ITurnExecutable>();

        //For each turn in turnSlots, add a turn to the turns list
        if (onPhaseChange != null){
            onPhaseChange(phase);
        }

        int count = 0;

        foreach(ITurnExecutable turn in turns) {
            CharacterDisplayController CurrentCharDisplay = null;
            count++;
            
            if(CurrentCharDisplay != null)
                CurrentCharDisplay.highlightTurn(true);

            yield return turn.GetTurn();
            if(gameOver){
                yield break;
            }

            if (CurrentCharDisplay != null)
                CurrentCharDisplay.highlightTurn(false);
        }

        //Discards all cards that were played
        foreach(ITurnExecutable turn in turns){
            Character c = turn as Character;
            if(c != null){
                c.EndResolvePhase();
            }
        }
        CombatUIManager.Instance.UpdateCharacters();
    }

    //Executes while turn is in draw phase
    public IEnumerator ExecuteDrawPhase() {
        phase = Enums.GameplayPhase.Draw;
        Debug.Log("<color=yellow>Draw Phase</color>:");
        if (onPhaseChange != null) {
            onPhaseChange(phase);
        }

        if (decks[Enums.Affinity.Chalice].CardList.Count > 0 ||
            decks[Enums.Affinity.Pentacle].CardList.Count > 0 ||
            decks[Enums.Affinity.Staff].CardList.Count > 0 ||
            decks[Enums.Affinity.Sword].CardList.Count > 0) {
            actionsEnabled = true;

            StartCoroutine(CombatUIManager.Instance.DisplayMessage("Draw a card", 3f));
            CombatUIManager.Instance.StartDraw();
                int cardsInHand = CombatUIManager.Instance.Hand.DisplayedCards.Count;
                //Wait untill one card has been drawn
                while (CombatUIManager.Instance.Hand.DisplayedCards.Count != cardsInHand + 1)
                yield return new WaitForEndOfFrame();
                //Disable draw buttons
            CombatUIManager.Instance.EndDraw();
        } else { Debug.Log("Skipping Draw Phase. No cards to draw."); }

        phase = Enums.GameplayPhase.Planning;
    }

    //Checks if the game is over. Should be called whenever a character or foe is Defeated
    public void CheckGameOver()
    {
        bool playerDefeated = true;

        foreach(Character partyMember in party)
        {
            playerDefeated = playerDefeated && partyMember.Defeated;
        }

        if(playerDefeated)
        {
            StopCoroutine(battleEnumerator);
            Debug.Log("Game Over! TPK");
            //Return to main menu ui
            StartCoroutine(GameOverScreen());
        }

        bool foesDefeated = true;
        foreach(Character foe in foes)
        {
            foesDefeated = foesDefeated && foe.Defeated;
        }

        if(foesDefeated)
        {
            StopCoroutine(battleEnumerator);
            Debug.Log("Game Over! Defeated enemies");
            //Card Drafting ui
            StartCoroutine(GameWinScreen());

        }
        gameOver = foesDefeated || playerDefeated;
    }

    public IEnumerator GameWinScreen(){
        yield return CombatUIManager.Instance.DisplayMessage("Congratulations", 6f);
        yield return UIManager.Instance.CloseCurtains();
        UIManager.Instance.StartDraft(Reward);
        SceneController.Instance.ExitCombatScene();
    }

    public IEnumerator GameOverScreen(){
        yield return CombatUIManager.Instance.DisplayMessage("Everyone has fallen...", 2f);
        yield return CombatUIManager.Instance.DisplayMessage("Your soul was claimed by the denizen", 4f);
        yield return SceneController.Instance.ReturnToMainMenu();
    }

    //UI function, is called when the player presses the end planning button
    public void EndPlanning(){
        if(phase == Enums.GameplayPhase.Planning) {
            phase = Enums.GameplayPhase.Resolve;
            //AudioManager.audioMgr.PlayUISFX("PaperInteraction");
        }
    }
    public void Draw (int deckToDrawFrom) {
        Draw((Enums.Affinity)deckToDrawFrom);
    }

    public void Draw(Enums.Affinity deckToDrawFrom) {
        var card = decks[deckToDrawFrom].Draw();
        PlaceCardInHand(card);
        CombatUIManager.Instance.UpdateCards();
    }

    //Return card to discard pile. Note: doesn't remove from hand
    public void Discard(Card card){
        if(card == null) return;
        if (card.Exiled) return;
        decks[card.Affinity].DiscardList.Add(card);
    }

    //Remove card display from hand: Note: doesn't discard
    public void PlaceCardInHand(Card c){
        CombatUIManager.Instance.Hand.AddCard(CardDisplayController.CreateCard(c));
    }


    public void RemoveCardFromHand(CardDisplayController cd){
        CombatUIManager.Instance.Hand.RemoveCard(cd);
    }
    
    //Links data from the inspector's characters and enum's class.
    public void InitializeDecks() {
        Debug.Log("Creating Decks");
        decks[Enums.Affinity.Chalice] = new Deck(PlayerData.ChaliceCards);
        decks[Enums.Affinity.Pentacle] = new Deck(PlayerData.PentacleCards);
        decks[Enums.Affinity.Staff] = new Deck(PlayerData.StaffCards);
        decks[Enums.Affinity.Sword] = new Deck(PlayerData.SwordCards);

        decks[Enums.Affinity.Chalice].Shuffle();
        decks[Enums.Affinity.Pentacle].Shuffle();
        decks[Enums.Affinity.Staff].Shuffle();
        decks[Enums.Affinity.Sword].Shuffle();

        CombatUIManager.Instance.UpdateCards();
    }

    //Initialize each character in party list established. 
    public void InitializeCharacters() {
        Debug.Log("Creating player characters");
        PartyCharacter newchar;
        for (int i = 0; i < 4; i++) {
            switch (PlayerData.party[i]) {
                case 1:
                    newchar = (Instantiate(Resources.Load("Prefabs/Combat Characters/Player Chalice Pentacle")) 
                        as GameObject).GetComponent<PartyCharacter>();
                    break;
                case 2:
                    newchar = (Instantiate(Resources.Load("Prefabs/Combat Characters/Player Chalice Staff")) 
                        as GameObject).GetComponent<PartyCharacter>();
                    break;
                case 3:
                    newchar = (Instantiate(Resources.Load("Prefabs/Combat Characters/Player Chalice Sword")) 
                        as GameObject).GetComponent<PartyCharacter>();
                    break;
                case 4:
                    newchar = (Instantiate(Resources.Load("Prefabs/Combat Characters/Player Pentacle Staff")) 
                        as GameObject).GetComponent<PartyCharacter>();
                    break;
                case 5:
                    newchar = (Instantiate(Resources.Load("Prefabs/Combat Characters/Player Pentacle Sword")) 
                        as GameObject).GetComponent<PartyCharacter>();
                    break;
                case 6:
                    newchar = (Instantiate(Resources.Load("Prefabs/Combat Characters/Player Staff Sword")) 
                        as GameObject).GetComponent<PartyCharacter>();
                    break;
                default:
                    continue;
            }
            party.Add(newchar);
            CombatUIManager.Instance.SetInfo(newchar, i);
            newchar.transform.position = playerPositions[i].position;
            newchar.transform.SetParent(transform);
            newchar.position = i;
            if (lookPosition != null)
                newchar.transform.LookAt(lookPosition);
            else
                newchar.transform.LookAt(Vector3.zero);
        }
    }

    //If looking for a child gameobject, find the gameobject by name and return the object (if none found, return null.
    public GameObject getChildGameObject(GameObject source, string name)
    {
        Transform[] children = source.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.gameObject.name == name) return child.gameObject;
        }

        return null;
    }

    public void AddOpponent(EnemyCharacter eCharacter) {
        if (!foes.Contains(eCharacter))
            foes.Add(eCharacter);
        CombatUIManager.Instance.SetInfo(eCharacter, eCharacter.HealthBar);
    }
}

//Interface inherited by anything that can take a turn
public interface ITurnExecutable {

    //Returns an ienumerator with the runtime logic of the object's turn that is executed when it's turn is resolved
    IEnumerator GetTurn();
}