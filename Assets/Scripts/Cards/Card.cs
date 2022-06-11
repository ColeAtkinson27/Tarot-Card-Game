using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Card", menuName = "Card")]
[System.Serializable]
public class Card : ScriptableObject {
    //[Header("Card Info")]
    [SerializeField] private string cardName;
    [TextArea] [SerializeField] private string cardDescription;
    [SerializeField] private string cardCorruptionPassDescription;
    [SerializeField] private string cardCorruptionFailDescription;
    [SerializeField] private string cardFlavor;
    [SerializeField] private Enums.Affinity cardAffinity;
    [SerializeField] private int cardAllyTargets;
    [SerializeField] private int cardEnemyTargets;
    [SerializeField] private Color cardColor;
    [SerializeField] private bool exiled;
    [SerializeField] private bool bossCard;

    //[Header("Card Art")]
    [SerializeField] private Enums.CardEffects[] cardIcons = new Enums.CardEffects[5];
    [SerializeField] private Enums.CardIconBack[] cardBackIcons = new Enums.CardIconBack[5];
    [SerializeField] private string cardAnimation = "";

    public List<CardEffect> cardEffects = new List<CardEffect>();
    public List<CardEffect> cardCorPass = new List<CardEffect>();
    public List<CardEffect> cardCorFail = new List<CardEffect>();

    private List<Character> allyTargets = new List<Character>();
    private List<Character> enemyTargets = new List<Character>();
    private Character caster;

    public string Name { get { return cardName; } }
    public string Description { get { return cardDescription; } }
    public string Flavor { get { return cardFlavor; } }
    public Enums.Affinity Affinity { get { return cardAffinity; } }
    public Character Caster { get { return caster; } set { caster = value; } }
    public Color Color
    {
        get { return cardColor; }
        set{cardColor = value;}
    }
    public string CorruptionPassDescription { get { return cardCorruptionPassDescription; } }
    public string CorruptionFailDescription { get { return cardCorruptionFailDescription; } }

    public bool Exiled { get { return exiled; } }
    public bool BossCard { get { return bossCard; } }
    public Enums.CardEffects[] Icons { get { return cardIcons; } }
    public Enums.CardIconBack[] IconsBack { get { return cardBackIcons; } }

    //Play the card
    public IEnumerator Activate () {
        //Check for a corruption check and execute
        if (cardCorPass.Count > 0 || cardCorFail.Count > 0) {
            for (int j = 0; j < cardCorPass.Count; j++)
                cardCorPass[j].caster = caster;
            for (int j = 0; j < cardCorFail.Count; j++)
                cardCorFail[j].caster = caster;

            //If its a boss card, select every player target and exectue
            if (bossCard)
                for (int i = 0; i < allyTargets.Count; i++)
                    if (allyTargets[i].CorruptionCheck())
                        for (int j = 0; j < cardCorPass.Count; j++)
                            yield return cardCorPass[j].Activate(allyTargets[i]);
                    else
                        for (int j = 0; j < cardCorFail.Count; j++)
                            yield return cardCorFail[j].Activate(allyTargets[i]);
            //If its a player card, then corruption check the caster
            else if (caster.CorruptionCheck())
                for (int j = 0; j < cardCorPass.Count; j++)
                    yield return cardCorPass[j].Activate(caster);
            else
                for (int j = 0; j < cardCorFail.Count; j++)
                    yield return cardCorFail[j].Activate(caster);
        }

        //Go through each effect and execute
        for (int i = 0; i < cardEffects.Count; i++) {
            cardEffects[i].caster = caster;
            //Assign targets
            if (cardEffects[i].targetType == Enums.Target.None)
                yield return cardEffects[i].Activate();
            if (cardEffects[i].targetType == Enums.Target.Self)
                yield return cardEffects[i].Activate(Caster);
            else if (cardEffects[i].targetType == Enums.Target.Ally)
                yield return cardEffects[i].Activate(allyTargets);
            else if (cardEffects[i].targetType == Enums.Target.Enemy)
                yield return cardEffects[i].Activate(enemyTargets);
            else if (cardEffects[i].targetType == Enums.Target.All_Ally)
                yield return cardEffects[i].Activate(GameManager.Instance.Party());
            else if (cardEffects[i].targetType == Enums.Target.All_Enemy) {
                List<Character> enTargs = new List<Character>();
                foreach (Character c in GameManager.Instance.Foes())
                    enTargs.Add(c);
                yield return cardEffects[i].Activate(enTargs);
            }
        }
    }

    public IEnumerator DesignateTargets() {
        GameManager.Instance.ActionsEnabled = false;
        if (cardAllyTargets > 0) {
            //Make sure there aren't more targets to choose than available.
            int survivingAllies = 0;
            for (int i = 0; i < GameManager.Instance.Party().Count; i++)
                if (!GameManager.Instance.Party()[i].Defeated) survivingAllies++;
            if (cardAllyTargets > survivingAllies)
                cardAllyTargets = survivingAllies;
            //Get the boss to choose each ally they will target
            if (bossCard) {
                allyTargets.Clear();
                Character targ;
                int index;
                for (int i = 0; i < cardAllyTargets; i++) {
                    do {
                        index = UnityEngine.Random.Range(0, 4);
                    } while (GameManager.Instance.Party()[index].Defeated
                    || GameManager.Instance.Party()[index].Status.Shrouded > 0
                    || allyTargets.Contains(GameManager.Instance.Party()[index]));
                    allyTargets.Add(GameManager.Instance.Party()[index]);
                }
            }
            //Get player to choose each ally they will target
            else {
                yield return Targetable.GetTargetable(Enums.TargetType.Allies, "Select Ally", cardAllyTargets);
                allyTargets.Clear();
                foreach (ITargetable itarg in Targetable.currentTargets) allyTargets.Add((Character)itarg);
            }
        }

        if (cardEnemyTargets > 0) {
            //Make sure there aren't more targets to choose than available.
            int survivingEnemies = 0;
            for (int i = 0; i < GameManager.Instance.Foes().Count; i++)
                if (!GameManager.Instance.Foes()[i].Defeated) survivingEnemies++;
            if (cardEnemyTargets > survivingEnemies)
                cardEnemyTargets = survivingEnemies;

            //Get the boss to choose each ally they will target
            enemyTargets.Clear();
            if (bossCard) {
                Character targ;
                int index;
                for (int i = 0; i < cardEnemyTargets; i++) {
                    do {
                        index = UnityEngine.Random.Range(0, GameManager.Instance.Foes().Count);
                    } while (GameManager.Instance.Foes()[index].Defeated
                    || enemyTargets.Contains(GameManager.Instance.Foes()[index]));
                    enemyTargets.Add(GameManager.Instance.Foes()[index]);
                }
            }
            //Get player to choose each enemy they will target
            else {
                yield return Targetable.GetTargetable(Enums.TargetType.Foes, "Select Enemy", cardEnemyTargets);
                enemyTargets.Clear();
                foreach (ITargetable itarg in Targetable.currentTargets) enemyTargets.Add((Character)itarg);
            }
        }
        CombatUIManager.Instance.SetMessage("");
        GameManager.Instance.ActionsEnabled = true;
    }

    public void Exile() {
        exiled = true;
    }
    public void AddCardEffectMaker(int listNo) {
        if (listNo == 0)
            cardEffects.Add(new CardEffect());
        else if (listNo == 1)
            cardCorPass.Add(new CardEffect());
        else if (listNo == 2)
            cardCorFail.Add(new CardEffect());
    }
    public CardEffect GetEffect(int index, int listNo) {
        if (listNo == 0)
            return cardEffects[index];
        else if (listNo == 1)
            return cardCorPass[index];
        else if (listNo == 2)
            return cardCorFail[index];
        else
            return null;
    }
}
