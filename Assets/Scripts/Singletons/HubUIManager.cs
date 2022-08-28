using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubUIManager : MonoBehaviour {

    [Header("Menus")]
    [SerializeField] private GameObject BaseMenu;
    [SerializeField] private GameObject MapMenu;

    [Header("Map")]
    [SerializeField] private Button[] mapButtons = new Button[22];

    private void Start() {
        for (int i = 0; i < 6; i++) {
            PlayerData.partyHP[i] = DataManager.Instance.PartyMember(i).MaxHP;
        }
        PlayerData.partyST = new int[] { 25, 25, 25, 25, 25, 25 };
    }

    public void ToggleMap() {
        BaseMenu.SetActive(!BaseMenu.activeSelf);
        MapMenu.SetActive(!MapMenu.activeSelf);
        CheckMapButtons();
    }

    private void CheckMapButtons() {
        mapButtons[0].interactable = PlayerData.CheckFlag("Fool Defeated");
        mapButtons[1].interactable = PlayerData.CheckFlag("Magician Defeated");
        mapButtons[2].interactable = PlayerData.CheckFlag("Priestess Defeated");
        mapButtons[3].interactable = PlayerData.CheckFlag("Empress Defeated");
        mapButtons[4].interactable = PlayerData.CheckFlag("Emperor Defeated");
        mapButtons[5].interactable = PlayerData.CheckFlag("Hierophant Defeated");
        mapButtons[6].interactable = PlayerData.CheckFlag("Lovers Defeated");
        mapButtons[7].interactable = PlayerData.CheckFlag("Chariot Defeated");
        mapButtons[8].interactable = PlayerData.CheckFlag("Strength Defeated");
        mapButtons[9].interactable = PlayerData.CheckFlag("Hermit Defeated");
        mapButtons[10].interactable = PlayerData.CheckFlag("Fortune Defeated");
        mapButtons[11].interactable = PlayerData.CheckFlag("Justice Defeated");
        mapButtons[12].interactable = PlayerData.CheckFlag("Hanged Man Defeated");
        mapButtons[13].interactable = PlayerData.CheckFlag("Death Defeated");
        mapButtons[14].interactable = PlayerData.CheckFlag("Temperance Defeated");
        mapButtons[15].interactable = PlayerData.CheckFlag("Devil Defeated");
        mapButtons[16].interactable = PlayerData.CheckFlag("Tower Defeated");
        mapButtons[17].interactable = PlayerData.CheckFlag("Star Defeated");
        mapButtons[18].interactable = PlayerData.CheckFlag("Moon Defeated");
        mapButtons[19].interactable = PlayerData.CheckFlag("Sun Defeated");
        mapButtons[20].interactable = PlayerData.CheckFlag("Judgement Defeated");
    }

    public void SelectLevel(int levelID) {
        LevelDirectory.CurrentLevel = levelID - 1;
        SceneController.Instance.NextLevel();
    }
}
