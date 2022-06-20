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
        PlayerData.ChaliceCards = chalice;
        PlayerData.PentacleCards = pentacle;
        PlayerData.StaffCards = staff;
        PlayerData.SwordCards = sword;
    }
}
