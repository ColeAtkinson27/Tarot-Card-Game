using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData {
    public static int location = 1;

    public static int[] party = new int[4];
    public static int[] partyHP = new int[6];
    public static int[] partyST = new int[6];

    private static int[] savedHP = new int[6];
    private static int[] savedST = new int[6];

    public static List<Card> ChaliceCards = new List<Card>();
    public static List<Card> PentacleCards = new List<Card>();
    public static List<Card> StaffCards = new List<Card>();
    public static List<Card> SwordCards = new List<Card>();

    public static List<Card> ChaliceCardsOwned = new List<Card>();
    public static List<Card> PentacleCardsOwned = new List<Card>();
    public static List<Card> StaffCardsOwned = new List<Card>();
    public static List<Card> SwordCardsOwned = new List<Card>();

    private static List<Flag> GameFlags = new List<Flag>();

    public static int EquippedCards { get { return ChaliceCards.Count + PentacleCards.Count + StaffCards.Count + SwordCards.Count; } }

    public static void SetPlayerStats (List<Character> partyMembers) {
        int charIndex = 0;
        for (int i = 0; i < 4; i++) {
            charIndex = party[i];
            partyHP[charIndex] = partyMembers[i].Health;
            partyST[charIndex] = partyMembers[i].Corruption;
        }
    }

    public static bool CheckFlag(string flag) {
        foreach (Flag f in GameFlags) {
            if (f.key.Equals(flag))
                return true;
        }
        return false;
    }

    public static void SetFlag(string flag, bool value) {
        foreach (Flag fl in GameFlags) {
            if (fl.key.Equals(flag)) {
                fl.value = value;
                return;
            }
        }
        Flag f = new Flag(flag, value);
        GameFlags.Add(f);
    }

    public static void CheckpointSave() {
        for (int i = 0; i < 6; i++) {
            savedHP[i] = partyHP[i];
            savedST[i] = partyST[i];
        }
    }

    public static void RevertCheckpointSave() {
        for (int i = 0; i < 6; i++) {
            partyHP[i] = savedHP[i];
            partyST[i] = savedST[i];
        }
    }
}

public class Flag {
    private string flagName;
    private bool flagValue;

    public Flag(string key, bool value) {
            flagName = key;
            flagValue = value;
    }

    public string key { get { return flagName; } }
    public bool value { get { return flagValue; } set { flagValue = value; } }
}
