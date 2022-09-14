using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character {
    [SerializeField] private int healthBar;
    protected Deck deck;

    private int speed;
    public int Speed { get { return speed; } set { speed = value; } }
    public override Card CardToPlay {
        get {
            return _cardToPlay;
        }

        set {
            var newCard = value;
            if(_cardToPlay != null){
                deck.DiscardList.Add(_cardToPlay);
            }
            _cardToPlay = newCard;
            if(_cardToPlay != null){
                Action = Enums.Action.Card;
            } else {
                Action = Enums.Action.Attack;
            }
        }
    }
    public int HealthBar { get { return healthBar; } }
    public override void Awake() {
        status = new CharacterStatus(this);
        health = data.MaxHP;
        corruption = 0;
        Action = Enums.Action.Attack;

        deck = new Deck(data.Deck);
        foreach (Card c in deck.CardList)
            c.Caster = this;
        animator = GetComponentInChildren<Animator>();
    }

    public override void Start() {
        base.Start();
        //if (data.color != null && highlight != null)
        //    highlight.GetComponent<ParticleSystem>().startColor = Color.white;
    }

    public override IEnumerator GetTurn(){
        deck.Shuffle();
        if (Action != Enums.Action.Stunned && Health > 0) {
            if (deck.CardList.Count == 0) deck.Reshuffle();
            CardToPlay = deck.Draw();
            yield return CombatUIManager.Instance.RevealCard(CardToPlay);
            Debug.Log($"{name} playing card {CardToPlay.Name}");
            CombatUIManager.Instance.DisplayMessage($"{name} plays {CardToPlay.Name}");
            yield return CardToPlay.DesignateTargets();
            yield return CardToPlay.Activate(this);
            deck.CardList.Remove(CardToPlay);
            deck.DiscardList.Add(CardToPlay);
        }
        yield return ResolveEffects();
    }
    public override void EndResolvePhase() {
        Action = Enums.Action.Card;
        base.EndResolvePhase();
    }
}
