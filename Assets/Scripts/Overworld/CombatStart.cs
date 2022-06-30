using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStart : MonoBehaviour {

    static CombatStart currentFight = null;
    [SerializeField] private Enums.Affinity encounter;

    void OnTriggerEnter(Collider collider) {
        Debug.Log("<color=green>Fight Collision Detected</color>");
        if (collider.gameObject.tag == "Player")
            StartCoroutine(StartCombat());
    }

    private IEnumerator StartCombat() {
        GetComponent<Collider>().enabled = false;
        currentFight = this;
        yield return SceneController.Instance.EnterCombatScene(encounter);
    }

    public static void EndCombat() {
        if (currentFight)
            Destroy(currentFight.gameObject);
    }
}
