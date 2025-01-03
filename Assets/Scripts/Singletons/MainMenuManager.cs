using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Constants;
using static Global;

/**<summary>The script that handles the game's main menu.</summary>*/
public class MainMenuManager : MonoBehaviour {

    private static MainMenuManager instance;
    public static MainMenuManager Instance { get { return instance; } }

    [Header("Menus")]
    /**<summary>The buttons menu on the left side of the screen.</summary>*/
    [SerializeField] private GameObject SideMenu;
    /**<summary>The menu displaying the player's saved games.</summary>*/
    [SerializeField] private GameObject SavesMenu;
    /**<summary>The menu displaying the game's settings.</summary>*/
    [SerializeField] private GameObject SettingsMenu;
    /**<summary>The menu displaying the help information.</summary>*/
    [SerializeField] private GameObject HelpMenu;
    /**<summary>The menu displaying the game's credits.</summary>*/
    [SerializeField] private GameObject CreditsMenu;
    /**<summary>The menu displaying the game's changelog.</summary>*/
    [SerializeField] private GameObject ChangelogMenu;

    [Header("Save Slot Menu")]
    /**<summary>The buttons to select the various save slots.</summary>*/
    [SerializeField] private List<SaveSlotButton> SaveSlotButtons;
    /**<summary>The button to load the selected save slot.</summary>*/
    [SerializeField] private Button LoadSaveButton;
    /**<summary>The button to delete the selected save slot.</summary>*/
    [SerializeField] private Button DeleteSaveButton;
    /**<summary>The popup menu for making a new save game.</summary>*/
    [SerializeField] private GameObject NewSaveMenu;
    /**<summary>The default difficulty toggle when making a new save game.</summary>*/
    [SerializeField] private Toggle defaultDifficultyToggle;

    private void Awake () {
        if (instance != null) {
            Destroy(this);
        }
        instance = this;
        SaveLoad.LoadMeta();
    }

    //===============//
    //=====MENUS=====//
    //===============//

    /** <summary>Toggles the save slots menu and initializes it to load a saved game.</summary> */
    public void ToggleLoadGameMenu() {
        for (int i = 0; i < SaveSlotButtons.Count; i++) {
            SaveSlotButtons[i].UpdateInformation();
        }

        NewSaveMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        HelpMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ChangelogMenu.SetActive(false);
        SavesMenu.SetActive(!SavesMenu.activeSelf);
    }

    /** <summary>Toggles the settings menu on or off.</summary> */
    public void ToggleSettingsMenu () {
        SavesMenu.SetActive(false);
        HelpMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ChangelogMenu.SetActive(false);
        SettingsMenu.SetActive(!SettingsMenu.activeSelf);
    }

    /** <summary>Toggles the help menu on or off.</summary> */
    public void ToggleHelpMenu () {
        SavesMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ChangelogMenu.SetActive(false);
        HelpMenu.SetActive(!HelpMenu.activeSelf);
    }

    /** <summary>Toggles the Credits menu on or off.</summary> */
    public void ToggleCreditsMenu () {
        SavesMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        HelpMenu.SetActive(false);
        ChangelogMenu.SetActive(false);
        CreditsMenu.SetActive(!CreditsMenu.activeSelf);
    }

    /** <summary>Toggles the changelog menu on or off.</summary> */
    public void ToggleChangelogMenu () {
        SavesMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        HelpMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ChangelogMenu.SetActive(!ChangelogMenu.activeSelf);
    }

    /** <summary>Closes the application.</summary> */
    public void Exit() {
        Application.Quit();
    }


    //====================//
    //=====SAVE SLOTS=====//
    //====================//

    /**<summary>The currently selected save slot.</summary>*/
    private int selectedSave;

    /**<summary>Select a save slot in the menu.</summary>*/
    public void SelectSaveSlot (int slotNum) {
        LoadSaveButton.interactable = false;
        DeleteSaveButton.interactable = false;
        NewSaveMenu.SetActive(false);

        if (slotNum > 0 && slotNum <= MAX_SAVE_SLOTS) {
            selectedSave = slotNum;
            if (SaveLoad.saveMetas.SavesDifficulty[slotNum - 1] > 0) {
                LoadSaveButton.interactable = true;
                DeleteSaveButton.interactable = true;
            } else {
                defaultDifficultyToggle.isOn = true;
                NewSaveMenu.SetActive(true);
            }
        }
    }

    /**<summary>Sets the difficulty when making a new game.</summary>*/
    public void SetDifficulty(int difficultyLevel) {
        Global.SetDifficulty(difficultyLevel);
    }

    /**<summary>Load the selected save slot</summary>*/
    public void LoadSelectedSave() {
        SaveLoad.Load(selectedSave - 1);
    }

    /**<summary>Delete the selected save slot</summary>*/
    public void DeleteSelectedSave () {
        SaveLoad.Delete(selectedSave - 1);
        for (int i = 0; i < SaveSlotButtons.Count; i++) {
            SaveSlotButtons[i].UpdateInformation();
        }
    }

    /**<summary>Create a new save file and launch the game.</summary>*/
    public void CreateNewGame() {
        SaveLoad.saveMetas.SavesDifficulty[selectedSave - 1] = Difficulty;
        SaveLoad.Save(selectedSave);

        LevelDirectory.CurrentLevel = 0;

        PlayerData.party = new int[] { 2, 3, 1, 4 };
        PlayerData.partyHP = new int[6];
        for (int i = 0; i < 6; i++) {
            Debug.Log("<color=green>" + DataManager.Instance.PartyMember(i).Name + "</color> " + DataManager.Instance.PartyMember(i).MaxHP);
            PlayerData.partyHP[i] = DataManager.Instance.PartyMember(i).MaxHP;
        }
        PlayerData.partyST = new int[] { 25, 25, 25, 25, 25, 25 };

        StartCoroutine(SceneController.Instance.OpenGame());
    }
}
