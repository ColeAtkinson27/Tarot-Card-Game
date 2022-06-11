using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus {

    private Character character;

    //Positive Effects
    private int armor = 0;
    private int shroud = 0;

    //Negative Effects
    private int stun = 0;

    //Stat Effects
    private int atkBonus = 0;
    private int defBonus = 0;

    //DoTs
    private List<DoT> burns = new List<DoT>();
    private List<DoT> rejuvenations = new List<DoT>();

    public int Armored { get { return armor; } set { armor = value; } }
    public int Shrouded { get { return shroud; } set { shroud = value; } }

    public int Stunned { get { return stun; } set { stun = value; } }

    public int ATKBonus { get { return atkBonus; } set { atkBonus = value; } }
    public int DEFBonus { get { return defBonus; } set { defBonus = value; } }

    public List<DoT> Burns { get { return burns; } }
    public List<DoT> Rejuvenations { get { return rejuvenations; } }

    public CharacterStatus(Character c) {
        character = c;
    }

    public void AddDot(Enums.CardEffects effect, int damage, int turns) {
        switch (effect) {
            case Enums.CardEffects.Burn:
                for (int i = 0; i < burns.Count; i++) {
                    if (burns[i].Damage == damage) {
                        burns[i].Turns += turns;
                        break;
                    }
                }
                burns.Add(new DoT (damage, turns));
                break;
            case Enums.CardEffects.Rejuvenate:
                for (int i = 0; i < Rejuvenations.Count; i++) {
                    if (rejuvenations[i].Damage == damage) {
                        rejuvenations[i].Turns += turns;
                        break;
                    }
                }
                rejuvenations.Add(new DoT(damage, turns));
                break;
        }
    }

    public void ResolveEffects() {
        //Positive effects
        if (shroud > 0) shroud--;
        //Negative effects
        if (stun > 0) stun--;
        //DoTs
        for (int i = 0; i < burns.Count; i++) {
            character.Health -= burns[i].Damage;
            burns[i].Turns--;
            if (burns[i].Turns <= 0) {
                burns.Remove(burns[i]);
                i--;
            }
        }
        for (int i = 0; i < rejuvenations.Count; i++) {
            character.Health += rejuvenations[i].Damage;
            rejuvenations[i].Turns--;
            if (rejuvenations[i].Turns <= 0) {
                rejuvenations.Remove(rejuvenations[i]);
                i--;
            }
        }
    }
}

public class DoT {
    public int Damage;
    public int Turns;

    public DoT(int damage, int turns) {
        Damage = damage; Turns = turns;
    }
}
