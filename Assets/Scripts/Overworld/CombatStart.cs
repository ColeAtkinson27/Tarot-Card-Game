using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStart : MonoBehaviour {

    [SerializeField] private Enums.Affinity encounter;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player")
            StartCombat();
    }

    private void StartCombat() {
        SceneController.Instance.EnterCombatScene(encounter);
        Destroy(gameObject);
    }
}
