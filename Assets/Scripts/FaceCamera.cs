using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    void Update() {
        try {
            transform.LookAt(Camera.current.transform);
        } catch (System.Exception e) { }
    }
}
