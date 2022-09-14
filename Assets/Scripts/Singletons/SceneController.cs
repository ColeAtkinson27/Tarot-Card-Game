using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * <summary>Opens and closes scenes and base UI menus.</summary>
 */
public class SceneController : MonoBehaviour {

    /**
     * <summary>Scene Controller singleton.</summary>
     */
    private static SceneController instance;

    [SerializeField] private string currentScene;

    /**
     * <summary>The base overworld scene that is currently in use</summary>
     */
    //[SerializeField] private SceneObject baseScene;

    void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (this != instance) {
            Destroy(gameObject);
        }
        Settings.LoadSettings();
        LevelDirectory.Initialize();
    }

    public IEnumerator OpenGame() {
        SceneManager.LoadScene("Base");
        while(!SceneManager.GetSceneByName("Base").IsValid()) yield return null;
        LoadLevel();
    }

    public void LoadLevel() {
        try {
            currentScene = LevelDirectory.GetSceneName();
            SceneManager.LoadScene(currentScene, LoadSceneMode.Additive);
            Debug.Log("<color=yellow>Loaded Scene: </color>" + currentScene);
        } catch (Exception e) {
            Debug.Log("<color=red>Load scene error:</color> " + e.ToString() + "\n" + e.StackTrace);
            SceneManager.LoadScene(0);
        }
    }

    public void UnloadLevel() {
        try {
            SceneManager.UnloadSceneAsync(currentScene);
        } catch (Exception e) {
            Debug.Log("<color=red>Unload scene error:</color> " + e.ToString() + "\n" + e.StackTrace);
            SceneManager.LoadScene(0);
        }
    }

    public void NextLevel() {
        UnloadLevel();
        LevelDirectory.CurrentLevel++;
        LoadLevel();
    }

    public IEnumerator ReturnToMainMenu () {
        yield return UIManager.Instance.CloseCurtains();
        MainMenu();
    }

    public void MainMenu() {
        currentScene = "";
        SceneManager.LoadScene(0);
    }

    /**
     * <summary>Scene Controller singleton.</summary>
     */
    public static SceneController Instance { get { return instance; } }
}
