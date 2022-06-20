using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnvironmentAction : MonoBehaviour {

    [SerializeField] private Transform targetPosition;
    [SerializeField] protected bool[] freezeAxes = new bool[3];

    private Collider collider;
    protected bool inRange;

    static protected bool isMoving;
    static protected bool pressed;

    void Awake() {
        collider = GetComponent<Collider>();
    }

    void Update() {
        if (inRange)
            if (Input.GetAxisRaw("Interaction") != 0) {
                if (!pressed && !isMoving) {
                    pressed = true;
                    StartCoroutine(Action());
                }
            }  else pressed = false; 
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            SceneController.Instance.Player.interactionBubble.SetActive(true);
            inRange = true;
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            SceneController.Instance.Player.interactionBubble.SetActive(false);
            inRange = false;
        }
    }

    protected virtual IEnumerator Action () {
        SceneController.Instance.Player.GetComponent<NavMeshAgent>().enabled = false;
        SceneController.Instance.Player.transform.LookAt(GetPos());
        SceneController.Instance.Player.transform.position = GetPos();
        SceneController.Instance.Player.GetComponent<NavMeshAgent>().enabled = true;
        yield return null;
    }

    protected Vector3 GetPos() {
        float x, y, z;

        if (freezeAxes[0]) x = SceneController.Instance.Player.transform.position.x;
        else x = targetPosition.position.x;

        if (freezeAxes[1]) y = SceneController.Instance.Player.transform.position.y;
        else y = targetPosition.position.y;

        if (freezeAxes[2]) z = SceneController.Instance.Player.transform.position.z;
        else z = targetPosition.position.z;

        return new Vector3(x, y, z);
    }

    protected Vector3 LookAtPos () {
        float x, y, z;

        if (freezeAxes[0]) x = SceneController.Instance.Player.transform.position.x;
        else x = targetPosition.position.x;

        y = SceneController.Instance.Player.transform.position.y;

        if (freezeAxes[2]) z = SceneController.Instance.Player.transform.position.z;
        else z = targetPosition.position.z;

        return new Vector3(x, y, z);
    }

    protected Transform Target { get { return targetPosition; } }
}
