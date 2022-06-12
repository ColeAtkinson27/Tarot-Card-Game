using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums {

    public enum CardEffects {
        None,
        Advance,
        Armored,
        Bleed,
        Blind,
        Bolster,
        Burn,
        Cleanse,
        Cold,
        Corrupt,
        Cripple,
        Curse,
        Discard,
        Draw,
        Fire,
        Fortify,
        Haste,
        Heal,
        Insert,
        Lightning,
        Mark,
        Nature,
        Nullify,
        Physical,
        Poison,
        Protect,
        Rejuvenate,
        Reshuffle,
        Revive,
        Shroud,
        Silence,
        Sleep,
        Slow,
        Soothe,
        Stun,
        Summon,
        Taunt,
        Weaken
    }

    /** <summary>A list of possible character affinities.</summary> */
    public enum Affinity {
        None,
        Chalice,
        Pentacle,
        Staff,
        Sword,
        Fool,
        Magician,
        Priestess,
        Empress
    };

    /** <summary>A list of possible actions a character can take</summary> */
    public enum Action {
        Attack,
        Card,
        Draw,
        Stunned,
        Silenced
    }

    /** <summary>The types of effects on values in card effects.</summary> */
    public enum Modifier {
        Add,
        Divide,
        Multiply,
        Subtract
    }

    /** <summary>A list of factors that can affect card effects.</summary> */
    public enum ModifierFactors {
        Cards_Played,
        Corruption,
        Hand_Size,
        Health,
        Marked,
        Discards,
        Enemies
    }

    /** <summary>Targeting options for card effects</summary> */
    public enum Target {
        None,
        Self,
        Ally,
        Enemy,
        All_Ally,
        All_Enemy,
        Defeated_Ally
    }

    /** <summary>The types of effects on values in card effects.</summary> */
    public enum VitalityType {
        Health,
        Corruption
    }

    public enum GameplayPhase {
        Planning,
        Resolve,
        Draw
    }

    
    public enum TargetType{
        Any,
        Allies,
        Foes
    }

    public enum DamageType {
        Physical,
        Cold,
        Fire,
        Lightning,
        Nature
    }

    public enum CardIconBack {
        None,
        Card,
        Corruption_Fail,
        Corruption_Pass
    }
}
