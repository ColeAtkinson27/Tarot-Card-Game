using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoad : MonoBehaviour {
    [SerializeField] private string locationDisplayName;
    [SerializeField] private List<string> loadLevels = new List<string>();
    [SerializeField] private List<string> unloadLevels = new List<string>();

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player")
            StartLoad();
    }

    private void StartLoad() {
        if (!locationDisplayName.Equals("")) {
            if (!locationDisplayName.Equals(PlayerData.location)) {
                //PlayerData.location = locationDisplayName;
                StartCoroutine(UIManager.Instance.DisplayLocationName(locationDisplayName));
            }
        }
        for (int i = 0; i < unloadLevels.Count; i++)
            SceneController.Instance.UnloadScene(unloadLevels[i]);
        for (int i = 0; i < loadLevels.Count; i++)
            SceneController.Instance.LoadScene(loadLevels[i]);
    }
}
