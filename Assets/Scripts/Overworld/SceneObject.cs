using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour {

    [SerializeField] private string scene = "Empty";

    public string Scene { get { return scene; } }

    void Start() {
        SceneController.Instance.AddScene(this);
    }
}
