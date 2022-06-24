using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoad : MonoBehaviour {

    [SerializeField] private List<string> loadLevels = new List<string>();
    [SerializeField] private List<string> unloadLevels = new List<string>();

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player")
            StartLoad();
    }

    private void StartLoad() {
        for (int i = 0; i < unloadLevels.Count; i++)
            SceneController.Instance.UnloadScene(unloadLevels[i]);
        for (int i = 0; i < loadLevels.Count; i++)
            SceneController.Instance.LoadScene(loadLevels[i]);
    }
}
