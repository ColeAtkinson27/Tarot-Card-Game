using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestUIManager : MonoBehaviour {

    [Header("Characters")]
    [SerializeField] private List<Character> playerCharacters;
    [SerializeField] private List<CharacterDisplayController> playerInfo;

    [Header("Base Menu")]
    [SerializeField] private GameObject baseMenu;
    [SerializeField] private Button[] healButtons;
    [SerializeField] private Button continueButton;

    [Header("Character Menu")]
    [SerializeField] private GameObject characterMenu;

    private void Start() {
        PlayerData.CheckpointSave();
        StartCoroutine(UIManager.Instance.OpenCurtains());
        for (int i = 0; i < 6; i++) {
            playerCharacters[i].Health = PlayerData.partyHP[i];
            playerCharacters[i].Corruption = PlayerData.partyST[i];
        }

        for (int i = 0; i < 4; i++) {
            Debug.Log("<color=red>Rest Area slot " + i + "</color><color=green>" + playerCharacters[PlayerData.party[i]].data.Name + "</color> " + playerCharacters[PlayerData.party[i]].data.MaxHP);
            SetInfo(playerCharacters[PlayerData.party[i]], i);
        }
    }

    public void UpdateCharacters() {
        for (int i = 0; i < playerInfo.Count; i++) {
            playerInfo[i].StatusDisplay.UpdateEffects();
        }
    }

    public void SetInfo(Character pChar, int index) {
        playerInfo[index].Character = pChar;
        playerInfo[index].StatusDisplay.Character = pChar;
    }

    //==============//
    //=====BASE=====//
    //==============//

    public void RegenHP () {
        for (int i = 0; i < 6; i++) {
            PlayerData.partyHP[i] = DataManager.Instance.PartyMember(i).MaxHP;
            playerCharacters[i].Health = PlayerData.partyHP[i];
        }
        for (int i = 0; i < healButtons.Length; i++) { healButtons[i].interactable = false; }
        continueButton.interactable = true;
    }

    public void CleanseST() {
        for (int i = 0; i < 6; i++) {
            PlayerData.partyST[i] = 0;
            playerCharacters[i].Corruption = PlayerData.partyST[i];
        }
        for (int i = 0; i < healButtons.Length; i++) { healButtons[i].interactable = false; }
        continueButton.interactable = true;
    }

    public void BothHPST() {
        int heal;
        for (int i = 0; i < 6; i++) {
            heal = (DataManager.Instance.PartyMember(i).MaxHP - PlayerData.partyHP[i]) / 2;
            PlayerData.partyHP[i] += heal;
            Mathf.Clamp(PlayerData.partyHP[i], 0, DataManager.Instance.PartyMember(i).MaxHP);
            playerCharacters[i].Health = PlayerData.partyHP[i];

            heal = PlayerData.partyST[i] / 2;
            PlayerData.partyST[i] -= heal;
            Mathf.Clamp(PlayerData.partyST[i], 0, 100);
            playerCharacters[i].Corruption = PlayerData.partyST[i];
        }

        for (int i = 0; i < healButtons.Length; i++) { healButtons[i].interactable = false; }
        continueButton.interactable = true;
    }

    public void ContinueToNextLevel() {
        SceneController.Instance.NextLevel();
    }

    public void ReturnToHub() {
        LevelDirectory.CurrentLevel = 0;
        SceneController.Instance.NextLevel();
    }
}
