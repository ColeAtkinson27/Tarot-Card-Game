using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    private static UIManager instance;

    [SerializeField] private DeckBuilder draftManager;

    public static UIManager Instance { get { return instance; } }

    void Awake() {
        if (instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this);
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
    }
}
