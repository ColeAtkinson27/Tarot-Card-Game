using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LedgeAction : EnvironmentAction {

    [SerializeField] private bool climbLedge;
    [SerializeField] private bool tallDrop;

    void Awake() {
        if (climbLedge && tallDrop)
            Debug.Log(name + " has both climb ledge and tall drop enabled. Please fix this issue.");
    }

    protected override IEnumerator Action () {
        isMoving = true;
        SceneController.Instance.Player.enabled = false;
        PlayerMovement player = SceneController.Instance.Player;
        Vector3 startPos = player.transform.position;
        Vector3 targetPos = GetPos();
        float time = 0f;

        player.GetComponent<NavMeshAgent>().enabled = false;

        float x, z;
        if (freezeAxes[0]) x = player.transform.position.x; else x = transform.position.x;
        if (freezeAxes[2]) z = player.transform.position.z; else z = transform.position.z;
        player.transform.position = new Vector3(x, player.transform.position.y, z);
        player.transform.LookAt(LookAtPos());

        if (tallDrop) {
            targetPos = (LookAtPos() - player.transform.position) + player.transform.position;
            while (time < 1) {
                time += Time.deltaTime * (GlobalNumbers.LEDGE_DOWN_INTERACTION_TIME * 3);
                player.transform.position = Vector3.Lerp(startPos, targetPos, time);
                yield return null;
            }
            time = 0;
            startPos = player.transform.position;
            targetPos = GetPos();
            float totalTime = Vector3.Distance(player.transform.position, targetPos) / GlobalNumbers.LEDGE_FALL_INTERACTION_RATE;
            while (time < totalTime) {
                time += Time.deltaTime;
                player.transform.position = Vector3.Lerp(startPos, targetPos, time / totalTime);
                yield return null;
            }
        } else {
            if (climbLedge) {
                player.Animator.SetBool("LedgeClimbUp", true);
                player.Animator.SetTrigger("LedgeAction");
                while (time < 1) {
                    time += Time.deltaTime / GlobalNumbers.LEDGE_UP_INTERACTION_TIME;
                    player.transform.position = Vector3.Lerp(startPos, targetPos, time);
                    yield return null;
                }
            } else {
                player.Animator.SetBool("LedgeClimbUp", false);
                player.Animator.SetTrigger("LedgeAction");
                while (time < 1) {
                    time += Time.deltaTime / GlobalNumbers.LEDGE_DOWN_INTERACTION_TIME;
                    player.transform.position = Vector3.Lerp(startPos, targetPos, time);
                    yield return null;
                }
            }
        }
        isMoving = false;
        player.GetComponent<NavMeshAgent>().enabled = true;
        SceneController.Instance.Player.enabled = true;
    }
}
