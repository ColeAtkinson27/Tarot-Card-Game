using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMainMenuScript : MonoBehaviour {
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject HelpMenu;
    [SerializeField] private GameObject[] HelpPages = new GameObject[10];
    [SerializeField] private GameObject CardsMenu;
    [SerializeField] private GameObject SymbolsMenu;
    private int helpIndex;

    public void NewGame() {
        PlayerData.location = "Jester's Playground";
        StartCoroutine(SceneController.Instance.OpenGame("Base Scene")); 
    }

    public void BossRush() {
        PlayerData.location = "BossRushArena";
        StartCoroutine(SceneController.Instance.OpenGame("BossRush"));
    }

    public void Exit() { Application.Quit(); }
    public void Help(bool toggle) {
        HelpMenu.SetActive(toggle);
        MainMenu.SetActive(!toggle);
        helpIndex = 0;
        SetHelpPage();
    }

    public void Cards(bool toggle) {
        CardsMenu.SetActive(toggle);
        MainMenu.SetActive(!toggle);
    }
    public void Symbols(bool toggle) {
        SymbolsMenu.SetActive(toggle);
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
}
