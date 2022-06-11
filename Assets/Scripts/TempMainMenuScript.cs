using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMainMenuScript : MonoBehaviour {
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject HelpMenu;
    [SerializeField] private GameObject CardsMenu;
    [SerializeField] private GameObject SymbolsMenu;

    public void MultiDeckStart() { SceneManager.LoadScene(1); }
    public void SingleDeckStart() { SceneManager.LoadScene(2); }
    public void Exit() { Application.Quit(); }
    public void Help(bool toggle) {
        HelpMenu.SetActive(toggle);
        MainMenu.SetActive(!toggle);
    }

    public void Cards(bool toggle) {
        CardsMenu.SetActive(toggle);
        MainMenu.SetActive(!toggle);
    }
    public void Symbols(bool toggle) {
        SymbolsMenu.SetActive(toggle);
        MainMenu.SetActive(!toggle);
    }
}
