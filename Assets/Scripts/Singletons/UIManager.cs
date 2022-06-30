using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private static UIManager instance;

    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private DisplayText locationTitle;
    [SerializeField] private DeckBuilder draftManager;

    [Header("Pause")]
    [SerializeField] private CardMenusDisplay cardDisplay;
    [SerializeField] private List<CharacterMenusDisplay> characterDisplays = new List<CharacterMenusDisplay>();

    [Header("Curtains")]
    [SerializeField] private Image curtainLeft;
    [SerializeField] private Image curtainRight;

    private bool disableMenu = false;
    private bool menuOpen = false;

    public static UIManager Instance { get { return instance; } }

    void Awake() {
        if (instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this);
        StartCoroutine(OpenCurtains());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !disableMenu)
            StartCoroutine(ToggleMenu());
    }

    public IEnumerator DisplayLocationName (string name, float duration = 5f) {
        locationTitle.SetMessage(name);
        yield return new WaitForSeconds(duration);
        locationTitle.StartFade();
        locationTitle.SetMessage("");
    }

    private IEnumerator ToggleMenu() {
        disableMenu = true;
        menuOpen = !menuOpen;
        if (menuOpen) {
            yield return CloseCurtains();
            pauseMenu.SetActive(true);
            DisplayPauseMenu();
        } else {
            pauseMenu.SetActive(false);
            yield return OpenCurtains();
        }
        disableMenu = false;
    }

    private void DisplayPauseMenu() {
        bool[] inParty = new bool[6];
        for (int i = 0; i < 4; i++) {
            characterDisplays[i].SetCharacter(DataManager.Instance.PartyMember(PlayerData.party[i]));
            inParty[(PlayerData.party[i]-1)] = true;
        }
        int charNotInPartyDisplay = 4;
        for (int i = 0; i < inParty.Length; i++) {
            if (charNotInPartyDisplay == inParty.Length) break;
            if (!inParty[i]) {
                characterDisplays[charNotInPartyDisplay].SetCharacter(DataManager.Instance.PartyMember(i + 1));
                charNotInPartyDisplay++;
            }
        }
        cardDisplay.SortCards(0);
    }

    public IEnumerator OpenCurtains() {
        float count = 0;
        Vector3 leftStart = new Vector3(Screen.width / 4, Screen.height / 2, 0);
        Vector3 rightStart = new Vector3(Screen.width * 3 / 4, Screen.height / 2, 0);
        Vector3 leftEnd = new Vector3(-Screen.width * 1.2f / 4, Screen.height / 2, 0);
        Vector3 rightEnd = new Vector3(Screen.width * 5.2f / 4, Screen.height / 2, 0);
        while (count < 1) {
            curtainLeft.rectTransform.position = Vector3.Lerp(leftStart, leftEnd, count);
            curtainRight.rectTransform.position = Vector3.Lerp(rightStart, rightEnd, count);
            count += Time.deltaTime / GlobalNumbers.CURTAIN_MOVE_RATE;
            yield return null;
        }
        curtainLeft.rectTransform.position = new Vector3(-Screen.width * 1.2f / 4, Screen.height / 2, 0);
        curtainRight.rectTransform.position = new Vector3(Screen.width * 5.2f / 4, Screen.height / 2, 0);
        SceneController.Instance.Player.enabled = true;
        SceneController.Instance.Camera.enabled = true;
    }
    public IEnumerator CloseCurtains() {
        SceneController.Instance.Player.enabled = false;
        SceneController.Instance.Camera.enabled = false;
        float count = 0;
        Vector3 leftStart = new Vector3(-Screen.width * 1.2f / 4, Screen.height / 2, 0);
        Vector3 rightStart = new Vector3(Screen.width * 5.2f / 4, Screen.height / 2, 0);
        Vector3 leftEnd = new Vector3(Screen.width / 4, Screen.height / 2, 0);
        Vector3 rightEnd = new Vector3(Screen.width * 3 / 4, Screen.height / 2, 0);
        while (count < 1) {
            curtainLeft.rectTransform.position = Vector3.Lerp(leftStart, leftEnd, count);
            curtainRight.rectTransform.position = Vector3.Lerp(rightStart, rightEnd, count);
            count += Time.deltaTime / GlobalNumbers.CURTAIN_MOVE_RATE;
            yield return null;
        }
        curtainLeft.rectTransform.position = new Vector3(Screen.width / 4, Screen.height / 2, 0);
        curtainRight.rectTransform.position = new Vector3(Screen.width * 3 / 4, Screen.height / 2, 0);
    }

    public void ExitToMenu() {
        SceneController.Instance.MainMenu();
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void StartDraft(CardDraftPool pool) {
        draftManager.gameObject.SetActive(true);
        draftManager.StartDraft(pool);
        SceneController.Instance.Player.gameObject.SetActive(false);
        SceneController.Instance.Camera.gameObject.SetActive(false);
    }

    public void EndDraft() {
        draftManager.gameObject.SetActive(false);
        SceneController.Instance.Player.gameObject.SetActive(true);
        SceneController.Instance.Camera.gameObject.SetActive(true);
        CombatStart.EndCombat();
        StartCoroutine(OpenCurtains());
    }
}
