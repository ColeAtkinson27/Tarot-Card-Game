using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpAction : EnvironmentAction {

    protected override IEnumerator Action () {
        isMoving = true;
        SceneController.Instance.Player.enabled = false;
        PlayerMovement player = SceneController.Instance.Player;
        Vector3 startPos = player.transform.position;
        Vector3 targetPos = GetPos();
        float time = 0f;

        player.GetComponent<NavMeshAgent>().enabled = false;
        player.transform.LookAt(LookAtPos());

        player.Animator.SetBool("isRunning", false);
        player.Animator.SetTrigger("Jump");
        while (time < 1) {
            time += Time.deltaTime / GlobalNumbers.JUMP_INTERACTION_TIME;
            player.transform.position = Vector3.Lerp(startPos, targetPos, time);
            yield return null;
        }
        isMoving = false;
        player.GetComponent<NavMeshAgent>().enabled = true;
        SceneController.Instance.Player.enabled = true;
    }
}
