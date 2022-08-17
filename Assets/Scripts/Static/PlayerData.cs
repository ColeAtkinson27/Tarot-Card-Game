using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData {
    public static int location = 1;

    public static int[] party = new int[4];
    public static int[] partyHP = new int[4];
    public static int[] partyST = new int[4];

    public static List<Card> ChaliceCards = new List<Card>();
    public static List<Card> PentacleCards = new List<Card>();
    public static List<Card> StaffCards = new List<Card>();
    public static List<Card> SwordCards = new List<Card>();

    public static List<Card> ChaliceCardsOwned = new List<Card>();
    public static List<Card> PentacleCardsOwned = new List<Card>();
    public static List<Card> StaffCardsOwned = new List<Card>();
    public static List<Card> SwordCardsOwned = new List<Card>();

    public static int EquippedCards { get { return ChaliceCards.Count + PentacleCards.Count + StaffCards.Count + SwordCards.Count; } }
}
