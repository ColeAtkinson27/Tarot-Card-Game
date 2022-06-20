using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStart : MonoBehaviour {

    [SerializeField] private Enums.Affinity encounter;

    void OnTriggerEnter(Collider collider) {
        Debug.Log("<color=green>Fight Collision Detected</color>");
        if (collider.gameObject.tag == "Player")
            StartCombat();
    }

    private void StartCombat() {
        SceneController.Instance.EnterCombatScene(encounter);
        Destroy(gameObject);
    }
}
