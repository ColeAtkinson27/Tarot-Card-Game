using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianCharacter : EnemyCharacter {

    private void Awake() {
        health = data.MaxHP;
        corruption = 0;
        Action = Enums.Action.Attack;

        deck = new Deck(data.Deck);
        foreach (Card c in deck.CardList)
            c.Caster = this;
    }

    public override IEnumerator GetTurn() {
        deck.Shuffle();
        if (Action != Enums.Action.Stunned && Health > 0) {
            if (deck.CardList.Count == 0) deck.Reshuffle();
            CardToPlay = deck.Draw();
            yield return CombatUIManager.Instance.RevealCard(CardToPlay);
            Debug.Log($"{name} playing card {CardToPlay.Name}");
            CombatUIManager.Instance.DisplayMessage($"{name} plays {CardToPlay.Name}");
            yield return CardToPlay.DesignateTargets();
            yield return CardToPlay.Activate();
            deck.CardList.Remove(CardToPlay);
            deck.DiscardList.Add(CardToPlay);
        }
        ResolveEffects();
    }
}
