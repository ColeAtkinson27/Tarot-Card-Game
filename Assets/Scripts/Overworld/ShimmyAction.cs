using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShimmyAction : EnvironmentAction {

    [SerializeField] private Transform endTarget;
    [SerializeField] private Transform ledge;
    [SerializeField] private bool moveRight;

    public Transform EndTarget { get { return endTarget; } }

    protected override IEnumerator Action () {
        isMoving = true;
        SceneController.Instance.Player.enabled = false;
        PlayerMovement player = SceneController.Instance.Player;
        player.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

        //Move to transform
        float x, z;
        if (freezeAxes[0]) z = player.transform.position.z; else z = ledge.position.z;
        if (freezeAxes[2]) x = player.transform.position.x; else x = ledge.position.x;
        Vector3 startPos = player.transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        player.transform.LookAt(targetPos);
        float time = 0f;
        player.Animator.SetBool("isRunning", true);
        while (time < 1) {
            time += Time.deltaTime;
            player.transform.position = Vector3.Lerp(startPos, targetPos, time);
            yield return null;
        }
        player.Animator.SetBool("isRunning", false);

        //Set target positions
        if (freezeAxes[0]) z = player.transform.position.z; else z = ledge.position.z;
        if (freezeAxes[2]) x = player.transform.position.x; else x = ledge.position.x;
        startPos = player.transform.position;
        targetPos = new Vector3(x, player.transform.position.y, z);
        Vector3[] targetPositions = new Vector3[2];

        player.transform.LookAt(LookAtPos());
        if (moveRight) {
            player.transform.Rotate(new Vector3(0, -90, 0));
            targetPositions[0] = endTarget.position;
            targetPositions[1] = Target.GetComponent<ShimmyAction>().EndTarget.position;
        } else {
            player.transform.Rotate(new Vector3(0, 90, 0));
            targetPositions[0] = Target.GetComponent<ShimmyAction>().EndTarget.position;
            targetPositions[1] = endTarget.position;
        }

        player.Animator.SetBool("isShimmying", true);
        player.Animator.SetTrigger("InteractionStart");
        //Move Player towards the wall
        time = 0f;
        while (time < 1) {
            time += Time.deltaTime * (GlobalNumbers.LEDGE_UP_INTERACTION_TIME);
            player.transform.position = Vector3.Lerp(startPos, targetPos, time);
            yield return null;
        }
        
        //Move Player to ledge position
        /**time = 0f;
        startPos = player.position;
        if (moveRight) {
            targetPos = targetPos + (player.transform.right * 0.25f);
            while (time < 1) {
                time += Time.deltaTime * (GlobalNumbers.LEDGE_UP_INTERACTION_TIME);
                player.position = Vector3.Lerp(startPos, targetPos, time);
                yield return null;
            }
        } else {
            targetPos = targetPos - (player.transform.right * 0.25f);
            while (time < 1) {
                time += Time.deltaTime * (GlobalNumbers.LEDGE_UP_INTERACTION_TIME);
                player.position = Vector3.Lerp(startPos, targetPos, time);
                yield return null;
            }
        }*/

        SceneController.Instance.Player.Shimmying(targetPositions);
        SceneController.Instance.Player.enabled = true;
        isMoving = false;
    }
}
