using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInsert : MonoBehaviour {

    public List<Card> chalice;
    public List<Card> pentacle;
    public List<Card> staff;
    public List<Card> sword;
    // Start is called before the first frame update
    void Awake() {
        PlayerData.ChaliceCardsOwned = chalice;
        PlayerData.PentacleCardsOwned = pentacle;
        PlayerData.StaffCardsOwned = staff;
        PlayerData.SwordCardsOwned = sword;
        PlayerData.ChaliceCards = new List<Card>(PlayerData.ChaliceCardsOwned);
        PlayerData.PentacleCards = new List<Card>(PlayerData.PentacleCardsOwned);
        PlayerData.StaffCards = new List<Card>(PlayerData.StaffCardsOwned);
        PlayerData.SwordCards = new List<Card>(PlayerData.SwordCardsOwned);
        PlayerData.party[0] = 3;
        PlayerData.party[1] = 4;
        PlayerData.party[2] = 2;
        PlayerData.party[3] = 5;
    }
}
