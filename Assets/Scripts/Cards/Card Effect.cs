using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>The base class for all other card effects.</summary> */
[System.Serializable]
public class CardEffect {
    public int value1;
    public int value2;
    public int value3;
    public string stringValue;
    public Enums.Target targetType;

    public Character caster;
    private Enums.CardEffects effectType;

    public Enums.CardEffects EffectType { 
        get { return effectType; } 
        set { effectType = value; }
    }

    public IEnumerator Activate () {
        yield return ApplyEffect(null);
    }

    public IEnumerator Activate(List<Character> targets) {
        foreach (Character c in targets)
            yield return Activate(c);
    }

    public IEnumerator Activate(Character target) {
        //Check if target is defeated
        if (target.Defeated) yield break;
        yield return ApplyEffect(target);
    }

    private IEnumerator ApplyEffect(Character target) {
        switch (effectType) {
            case Enums.CardEffects.None: break;
            case Enums.CardEffects.Armored:
                yield return CardEffectManager.Afflict(target, effectType, value1);
                break;
            case Enums.CardEffects.Bleed:
                yield return CardEffectManager.DoT(target, effectType, value1, value2);
                break;
            case Enums.CardEffects.Blind:
                yield return CardEffectManager.Afflict(target, effectType, value2);
                break;
            case Enums.CardEffects.Bolster:
                yield return CardEffectManager.Afflict(target, effectType, value1);
                break;
            case Enums.CardEffects.Burn:
                yield return CardEffectManager.DoT(target, effectType, value1, value2);
                break;
            case Enums.CardEffects.Cleanse:
                yield return CardEffectManager.Cleanse(target, true);
                break;
            case Enums.CardEffects.Cold:
                yield return CardEffectManager.Damage(caster, target, effectType, value1, value2, value3);
                break;
            case Enums.CardEffects.Corrupt:
                yield return CardEffectManager.Corrupt(target, true, value3);
                break;
            case Enums.CardEffects.Cripple:
                yield return CardEffectManager.Afflict(target, effectType, value1);
                break;
            case Enums.CardEffects.Curse:
                yield return CardEffectManager.Afflict(target, effectType, value2);
                break;
            case Enums.CardEffects.Discard:
                yield return CardEffectManager.Discard(value1);
                break;
            case Enums.CardEffects.Draw:
                yield return CardEffectManager.Draw(value1);
                break;
            case Enums.CardEffects.Fire:
                yield return CardEffectManager.Damage(caster, target, effectType, value1, value2, value3);
                break;
            case Enums.CardEffects.Fortify:
                yield return CardEffectManager.Afflict(target, effectType, value1);
                break;
            case Enums.CardEffects.Haste:
                yield return CardEffectManager.Afflict(target, effectType, value1);
                break;
            case Enums.CardEffects.Heal:
                yield return CardEffectManager.Damage(caster, target, effectType, value1, value2, value3);
                break;
            case Enums.CardEffects.Insert: break;
            case Enums.CardEffects.Lightning:
                yield return CardEffectManager.Damage(caster, target, effectType, value1, value2, value3);
                break;
            case Enums.CardEffects.Mark:
                yield return CardEffectManager.Afflict(target, effectType, value2);
                break;
            case Enums.CardEffects.Nature:
                yield return CardEffectManager.Damage(caster, target, effectType, value1, value2, value3);
                break;
            case Enums.CardEffects.Nullify:
                yield return CardEffectManager.Cleanse(target, false);
                break;
            case Enums.CardEffects.Physical:
                yield return CardEffectManager.Damage(caster, target, effectType, value1, value2, value3);
                break;
            case Enums.CardEffects.Poison:
                yield return CardEffectManager.DoT(target, effectType, value1, value2);
                break;
            case Enums.CardEffects.Protect:
                yield return CardEffectManager.Afflict(target, effectType, value2);
                break;
            case Enums.CardEffects.Rejuvenate:
                yield return CardEffectManager.DoT(target, effectType, value1, value2);
                break;
            case Enums.CardEffects.Reshuffle:
                yield return CardEffectManager.Reshuffle();
                break;
            case Enums.CardEffects.Revive:
                yield return CardEffectManager.Revive(target, value3);
                break;
            case Enums.CardEffects.Shroud:
                yield return CardEffectManager.Afflict(target, effectType, value2);
                break;
            case Enums.CardEffects.Silence:
                yield return CardEffectManager.Afflict(target, effectType, value2);
                break;
            case Enums.CardEffects.Sleep:
                yield return CardEffectManager.Afflict(target, effectType, value2);
                break;
            case Enums.CardEffects.Soothe:
                yield return CardEffectManager.Corrupt(target, false, value3);
                break;
            case Enums.CardEffects.Stun:
                yield return CardEffectManager.Afflict(target, effectType, value2);
                break;
            case Enums.CardEffects.Summon: break;
            case Enums.CardEffects.Taunt:
                yield return CardEffectManager.Afflict(target, effectType, value2);
                break;
            case Enums.CardEffects.Weaken:
                yield return CardEffectManager.Afflict(target, effectType, value1);
                break;
        }
        yield return new WaitForSeconds(0.25f);
        CombatUIManager.Instance.UpdateCharacters();
    }
}
