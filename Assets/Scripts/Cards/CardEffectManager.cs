using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardEffectManager {
    public static IEnumerator Afflict(Character target, Enums.CardEffects effectType, int value) {
        switch (effectType) {
            //Positive Effects
            case Enums.CardEffects.Armored:
                target.Status.Armored += value;
                break;
            case Enums.CardEffects.Protect: break;
            case Enums.CardEffects.Taunt: break;
            case Enums.CardEffects.Haste: break;
            case Enums.CardEffects.Shroud:
                target.Status.Shrouded += value;
                break;

            //Negative Effects
            case Enums.CardEffects.Blind: break;
            case Enums.CardEffects.Curse: break;
            case Enums.CardEffects.Mark: break;
            case Enums.CardEffects.Silence: break;
            case Enums.CardEffects.Sleep: break;
            case Enums.CardEffects.Stun:
                if (target.GetComponent<EnemyCharacter>() != null) {
                    int chance = Random.Range(0, 100);
                    if (chance > 50) {
                        target.Status.Stunned += value;
                        target.Action = Enums.Action.Stunned;
                        break;
                    }
                }
                target.Status.Stunned += value;
                target.Action = Enums.Action.Stunned;
                break;

            //Status Effects
            case Enums.CardEffects.Bolster:
                target.Status.ATKBonus += value;
                break;
            case Enums.CardEffects.Cripple:
                target.Status.ATKBonus -= value;
                break;
            case Enums.CardEffects.Fortify:
                target.Status.DEFBonus += value;
                break;
            case Enums.CardEffects.Weaken:
                target.Status.DEFBonus -= value;
                break;
        }
        yield return null;
    }

    public static IEnumerator DoT(Character target, Enums.CardEffects effectType, int value, int turns) {
        target.Status.AddDot(effectType, value, turns);
        yield return null;
    }

    public static IEnumerator Cleanse(Character target, bool cleanseHarmful) {
        throw new System.NotImplementedException();
    }

    public static IEnumerator Corrupt(Character target, bool corrupt, int value) {
        if (corrupt)
            target.Corruption += value;
        else
            target.Corruption -= value;
        yield return null;
    }

    public static IEnumerator Damage(Character caster, Character target, Enums.CardEffects effectType, int die, int sides, int bonus) {
        Damage damage = new Damage(die, sides, bonus);
        int damVal = damage.Value;

        //If healing, break the coroutine
        if (effectType == Enums.CardEffects.Heal) {
            target.Health += damVal;
            yield break;
        }
        
        //Calculate atk/def bonuses
        int modifier = 0;
        if (caster.Status.ATKBonus != 0) {
            if (caster.Status.ATKBonus > 0) {
                modifier++;
                caster.Status.ATKBonus--;
            } else {
                modifier--;
                caster.Status.ATKBonus++;
            }
        } else if (target.Status.DEFBonus != 0) {
            if (target.Status.DEFBonus > 0) {
                modifier--;
                target.Status.DEFBonus--;
            } else {
                modifier++;
                target.Status.DEFBonus++;
            }
        }
        if (modifier == -2) damVal /= 4;
        else if (modifier == -1) damVal /= 2;
        else if (modifier == 1) damVal = (int)(damVal * 1.5f);
        else if (modifier == 2) damVal *= 2;

        //Check for weaknesses and resistances
        switch (effectType) {
            case Enums.CardEffects.Cold:
                if (target.data.Weaknesses.Contains(Enums.DamageType.Cold)) damVal *= 2;
                else if (target.data.Resistances.Contains(Enums.DamageType.Cold)) damVal /= 2;
                break;
            case Enums.CardEffects.Fire:
                if (target.data.Weaknesses.Contains(Enums.DamageType.Fire)) damVal *= 2;
                else if (target.data.Resistances.Contains(Enums.DamageType.Fire)) damVal /= 2;
                break;
            case Enums.CardEffects.Lightning:
                if (target.data.Weaknesses.Contains(Enums.DamageType.Lightning)) damVal *= 2;
                else if (target.data.Resistances.Contains(Enums.DamageType.Lightning)) damVal /= 2;
                break;
            case Enums.CardEffects.Nature:
                if (target.data.Weaknesses.Contains(Enums.DamageType.Nature)) damVal *= 2;
                else if (target.data.Resistances.Contains(Enums.DamageType.Nature)) damVal /= 2;
                break;
            case Enums.CardEffects.Physical:
                if (target.data.Weaknesses.Contains(Enums.DamageType.Physical)) damVal *= 2;
                else if (target.data.Resistances.Contains(Enums.DamageType.Physical)) damVal /= 2;
                break;
        }

        target.Health -= damVal;
        yield return null;
    }

    public static IEnumerator Discard(int numberToDiscard) {
        for (int i = 0; i < numberToDiscard; i++) {
            if (CombatUIManager.Instance.Hand.DisplayedCards.Count > 0) {
                CombatUIManager.Instance.Discard(true);
                int cardsInHand = CombatUIManager.Instance.Hand.DisplayedCards.Count;
                yield return CombatUIManager.Instance.DisplayMessage("Discard a card", 3f);
                //Wait untill one card has been drawn
                while (CombatUIManager.Instance.Hand.DisplayedCards.Count != cardsInHand - 1)
                    yield return new WaitForEndOfFrame();
                //Disable draw buttons
                CombatUIManager.Instance.Discard(false);
            }
        }
        yield return null;
    }

    public static IEnumerator Draw(int numberToDraw) {
        for (int i = 0; i < numberToDraw; i++) {
            yield return GameManager.Instance.ExecuteDrawPhase();
            GameManager.Instance.Phase = Enums.GameplayPhase.Resolve;
        }
    }

    public static IEnumerator Reshuffle() {
        GameManager.Instance.decks[Enums.Affinity.Chalice].Reshuffle();
        GameManager.Instance.decks[Enums.Affinity.Pentacle].Reshuffle();
        GameManager.Instance.decks[Enums.Affinity.Staff].Reshuffle();
        GameManager.Instance.decks[Enums.Affinity.Sword].Reshuffle();
        yield return null;
    }

    public static IEnumerator Revive(Character target, int healthPercentage) {
        throw new System.NotImplementedException();
    }
}
