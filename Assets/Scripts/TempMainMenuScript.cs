using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMainMenuScript : MonoBehaviour {
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject HelpMenu;
    [SerializeField] private GameObject[] HelpPages = new GameObject[10];
    [SerializeField] private GameObject StoryMenu;
    [SerializeField] private List<GameObject> StoryPages = new List<GameObject>();
    [SerializeField] private GameObject CardsMenu;
    [SerializeField] private GameObject SymbolsMenu;
    [SerializeField] private GameObject ChangelogMenu;
    private int helpIndex;
    private int storyIndex;

    public void NewGame() {
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

    public void Exit() { Application.Quit(); }
    public void Help(bool toggle) {
        HelpMenu.SetActive(toggle);
        MainMenu.SetActive(!toggle);
        helpIndex = 0;
        SetHelpPage();
    }
    public void Story(bool toggle) {
        StoryMenu.SetActive(toggle);
        MainMenu.SetActive(!toggle);
        storyIndex = 0;
        SetStoryPage();
    }

    public void Cards(bool toggle) {
        CardsMenu.SetActive(toggle);
        MainMenu.SetActive(!toggle);
    }
    public void Symbols(bool toggle) {
        SymbolsMenu.SetActive(toggle);
        MainMenu.SetActive(!toggle);
    }
    public void Changelog(bool toggle) {
        ChangelogMenu.SetActive(toggle);
        MainMenu.SetActive(!toggle);
    }

    public void SetHelpPage() {
        for (int i = 0; i < 10; i++) {
            HelpPages[i].SetActive(false);
        }
        HelpPages[helpIndex].SetActive(true);
    }

    public void HelpNextPage() {
        if (helpIndex >= 9) return;
        helpIndex++;
        SetHelpPage();
    }
    public void HelpPreviousPage() {
        if (helpIndex == 0) return;
        helpIndex--;
        SetHelpPage();
    }

    public void SetStoryPage() {
        for (int i = 0; i < StoryPages.Count; i++) {
            StoryPages[i].SetActive(false);
        }
        StoryPages[storyIndex].SetActive(true);
    }
    public void SetStoryPage(int index) {
        storyIndex = index;
        SetStoryPage();
    }

    public void StoryNextPage() {
        if (storyIndex >= (StoryPages.Count - 1)) return;
        storyIndex++;
        SetStoryPage();
    }
    public void StoryPreviousPage() {
        if (storyIndex == 0) return;
        storyIndex--;
        SetStoryPage();
    }
}
