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

    [SerializeField] private string currentBoss;
    [SerializeField] private List<SceneObject> levels;

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
    }

    private void Start() {
        LoadScene(PlayerData.location);
    }

    public void LoadScene(string sceneName) {
        try {
            for (int i = 0; i < levels.Count; i++)
                if (levels[i].name == sceneName) return;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            Debug.Log("<color=yellow>Loaded Scene: </color>" + sceneName);
        } catch (Exception e) {
            Debug.Log("<color=red>Load scene error:</color> " + e.ToString() + "\n" + e.StackTrace);
            SceneManager.LoadScene(0);
        }
    }

    public void LoadScene (string sceneName, Vector3 pPos, Quaternion pRot, Vector3 cPos, float cRot) {
        PlayerCoords = pPos;
        PlayerRot = pRot;
        CameraCoords = cPos;
        CameraRot = cRot;
        CameraTilt = Camera.CurrentGameTilt;
        CameraZoom = Camera.CurrentDis;

        try {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        } catch (Exception e) {
            Debug.Log("<color=red>Load scene error:</color> " + e.ToString() + "\n" + e.StackTrace);
            SceneManager.LoadScene(0);
        }
    }

    public void UnloadScene(string sceneName) {
        try {
            for (int i = 0; i < levels.Count; i++)
                if (levels[i].Scene.Equals(sceneName)) {
                    levels.RemoveAt(i);
                    SceneManager.UnloadSceneAsync(sceneName);
                    return;
                }
        } catch (Exception e) {
            Debug.Log("<color=red>Unload scene error:</color> " + e.ToString() + "\n" + e.StackTrace);
        }
    }

    /**
     * <summary>Opens a combat scene and sets up the encounter.</summary>
     * <param name="encounterNo">The encounter number listed in the Encounter Collection.</param>
     */
    public void EnterCombatScene (Enums.Affinity boss) {
        switch(boss) {
            case Enums.Affinity.Fool:
                currentBoss = "Fight Fool";
                break;
            case Enums.Affinity.Magician:
                currentBoss = "Fight Magician";
                break;
            case Enums.Affinity.Priestess:
                currentBoss = "Fight Priestess";
                break;
            default:
                Debug.Log("<color=red>Load boss error:</color> " + boss + " is not a valid boss.");
                return;
        }
        Player.gameObject.SetActive(false);
        Camera.gameObject.SetActive(false);
        foreach (SceneObject so in levels) {
            so.gameObject.SetActive(false);
        }
        SceneManager.LoadScene(currentBoss, LoadSceneMode.Additive);
    }

    /**
     * <summary>Closes the combat scene and resumes the current base scene.</summary>
     */
    public void ExitCombatScene () {
        try {
            SceneManager.UnloadSceneAsync(currentBoss);
            Player.gameObject.SetActive(true);
            Camera.gameObject.SetActive(true);
            foreach (SceneObject so in levels) {
                so.gameObject.SetActive(true);
            }
        } catch (Exception e) {
            Debug.Log("<color=red>Unload boss error:</color> " + e.ToString() + "\n" + e.StackTrace);
        }
    }


    public void ReturnToMainMenu () {
        SceneManager.LoadScene(0);
    }

    public void AddScene (SceneObject scene) {
        levels.Add(scene);
    }

    /**
     * <summary>Scene Controller singleton.</summary>
     */
    public static SceneController Instance { get { return instance; } }
    //public SceneObject CurrentScene { get { return baseScene; } }
    public PlayerMovement Player { get; set; }
    public CameraMovement Camera { get; set; }

    public Vector3 PlayerCoords { get; set; }
    public Quaternion PlayerRot { get; set; }
    public Vector3 CameraCoords { get; set; }
    public float CameraRot { get; set; }
    public float CameraTilt { get; set; }
    public float CameraZoom { get; set; }
}
