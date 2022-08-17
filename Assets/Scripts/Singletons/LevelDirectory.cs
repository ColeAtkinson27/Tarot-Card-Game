using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirectory : MonoBehaviour {

    private static LevelDirectory instance;
    public static LevelDirectory Instance { get { return instance; } }
    void Awake() {
        if (instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this);
    }

    private int[] Bosses = { 0, 5, 10 };
    private int[] Checkpoints = { 4, 9 };

    public void OpenLevel() {
        if (PlayerData.location < 0 || PlayerData.location > 10) {
            Debug.Log("<color=red></color>");
            return;
        }
        if (PlayerData.location == 1) { }
        for (int i = 0; i < Bosses.Length; i++) { }
        for (int i = 0; i < Checkpoints.Length; i++) { }
    }
}
