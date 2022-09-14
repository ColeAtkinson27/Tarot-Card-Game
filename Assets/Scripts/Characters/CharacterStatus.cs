using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus {

    private Character character;

    //Miscellaneous Effects
    private bool advance = false;
    private bool slow = false;

    //Positive Effects
    private int armor = 0;
    private int haste = 0;
    private int shroud = 0;
    private int taunt = 0;

    //Negative Effects
    private int blind = 0;
    private int curse = 0;
    private int mark = 0;
    private int silence = 0;
    private int sleep = 0;
    private int stun = 0;

    //Stat Effects
    private int atkBonus = 0;
    private int defBonus = 0;

    //DoTs
    private List<DoT> bleeds = new List<DoT>();
    private List<DoT> burns = new List<DoT>();
    private List<DoT> poisons = new List<DoT>();
    private List<DoT> rejuvenations = new List<DoT>();

    public bool Advancing { get { return advance; } set { advance = value; } }
    public bool Slowed { get { return slow; } set { slow = value; } }

    public int Armored { get { return armor; } set { armor = value; } }

    public int Hasted { get { return haste; } set { haste = value; } }
    public int Shrouded { get { return shroud; } set { shroud = value; } }
    public int Taunting { get { return taunt; } set { taunt = value; } }

    public int Blinded { get { return blind; } set { blind = value; } }
    public int Cursed { get { return curse; } set { curse = value; } }
    public int Marked { get { return mark; } set { mark = value; } }
    public int Sleeping { get { return sleep; } set { sleep = value; } }
    public int Silenced { get { return silence; } set { silence = value; } }
    public int Stunned { get { return stun; } set { stun = value; } }

    public int ATKBonus { get { return atkBonus; } set { atkBonus = value; } }
    public int DEFBonus { get { return defBonus; } set { defBonus = value; } }

    public List<DoT> Bleeds { get { return bleeds; } }
    public List<DoT> Burns { get { return burns; } }
    public List<DoT> Poisons { get { return poisons; } }
    public List<DoT> Rejuvenations { get { return rejuvenations; } }

    public CharacterStatus(Character c) {
        character = c;
    }

    public void AddDot(Enums.CardEffects effect, int damage, int turns) {
        switch (effect) {
            case Enums.CardEffects.Bleed:
                bleeds.Add(new DoT(damage, turns));
                break;
            case Enums.CardEffects.Burn:
                burns.Add(new DoT(damage, turns));
                break;
            case Enums.CardEffects.Poison:
                poisons.Add(new DoT(damage, turns));
                break;
            case Enums.CardEffects.Rejuvenate:
                rejuvenations.Add(new DoT(damage, turns));
                break;
        }
    }

    public IEnumerator ResolveEffects() {
        if (character.Defeated) yield break;
        try {
            advance = false;
            slow = false;
            //Positive effects
            if (haste > 0) haste--;
            if (shroud > 0) shroud--;
            if (taunt > 0) taunt--;
            //Negative effects
            if (blind > 0) blind--;
            if (curse > 0) curse--;
            if (mark > 0) mark--;
            if (stun > 0) {
                stun--;
                yield return DataManager.Instance.PlayParticle(character, Enums.CardEffects.Stun);
                yield return new WaitForSeconds(0.5f);
            }
            if (silence > 0) silence--;
            if (sleep > 0) sleep--;
            //DoTs
            for (int i = 0; i < bleeds.Count; i++) {
                yield return DataManager.Instance.PlayParticle(character, Enums.CardEffects.Bleed);
                yield return new WaitForSeconds(0.5f);
                character.Health -= bleeds[i].Damage;
                if (character.Defeated) {
                    Dead();
                    yield break;
                }
                bleeds[i].Turns--;
                if (bleeds[i].Turns <= 0) {
                    bleeds.Remove(bleeds[i]);
                    i--;
                }
            }
            for (int i = 0; i < burns.Count; i++) {
                yield return DataManager.Instance.PlayParticle(character, Enums.CardEffects.Burn);
                yield return new WaitForSeconds(0.5f);
                character.Health -= burns[i].Damage;
                if (character.Defeated) {
                    Dead();
                    yield break;
                }
                burns[i].Turns--;
                if (burns[i].Turns <= 0) {
                    burns.Remove(burns[i]);
                    i--;
                }
            }
            for (int i = 0; i < poisons.Count; i++) {
                yield return DataManager.Instance.PlayParticle(character, Enums.CardEffects.Poison);
                yield return new WaitForSeconds(0.5f);
                character.Health -= poisons[i].Damage;
                if (character.Defeated) {
                    Dead();
                    yield break;
                }
                poisons[i].Turns--;
                if (poisons[i].Turns <= 0) {
                    poisons.Remove(poisons[i]);
                    i--;
                }
            }
            for (int i = 0; i < rejuvenations.Count; i++) {
                yield return DataManager.Instance.PlayParticle(character, Enums.CardEffects.Rejuvenate);
                yield return new WaitForSeconds(0.5f);
                character.Health += rejuvenations[i].Damage;
                rejuvenations[i].Turns--;
                if (rejuvenations[i].Turns <= 0) {
                    rejuvenations.Remove(rejuvenations[i]);
                    i--;
                }
            }
        } finally { }
    }

    public void Dead() {
        advance = false;
        slow = false;
        armor = 0;
        haste = 0;
        shroud = 0;
        taunt = 0;
        blind = 0;
        curse = 0;
        mark = 0;
        stun = 0;
        silence = 0;
        sleep = 0;
        bleeds.Clear();
        burns.Clear();
        poisons.Clear();
        rejuvenations.Clear();
    }
}

public class DoT {
    public int Damage;
    public int Turns;

    public DoT(int damage, int turns) {
        Damage = damage; Turns = turns;
    }
}
