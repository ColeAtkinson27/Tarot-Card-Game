using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour {

    [SerializeField] private string scene = "Empty";
    [SerializeField] private List<string> connectors = new List<string>();
    [SerializeField] private GameObject lighting;

    public string Scene { get { return scene; } }

    void Start() {
        SceneController.Instance.AddScene(this);
        for (int i = 0; i < connectors.Count; i++)
            SceneController.Instance.LoadScene(connectors[i]);
    }

    public void UseLighting(bool useLighting) {
        lighting.SetActive(useLighting);
    }
}
