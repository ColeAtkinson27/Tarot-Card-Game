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
        PlayerData.ChaliceCards.AddRange(chalice);
        PlayerData.PentacleCards.AddRange(pentacle);
        PlayerData.StaffCards.AddRange(staff);
        PlayerData.SwordCards.AddRange(sword);
    }
}
