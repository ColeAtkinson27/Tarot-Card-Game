using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbAction : EnvironmentAction {
    
    [SerializeField] private bool climbWall;
    [SerializeField] private float[] targetHeights = new float[2];

    void Start () {
        if (climbWall) {
            targetHeights[0] = transform.position.y + 0.25f;
            targetHeights[1] = Target.position.y - 3f;
        } else {
            targetHeights[0] = Target.position.y + 0.25f;
            targetHeights[1] = transform.position.y - 3f;
        }
    }

    protected override IEnumerator Action () {
        isMoving = true;
        SceneController.Instance.Player.enabled = false;
        PlayerMovement player = SceneController.Instance.Player;
        player.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

        //Set Player position
        float x, z;
        if (freezeAxes[0]) x = player.transform.position.x; else x = transform.position.x;
        if (freezeAxes[2]) z = player.transform.position.z; else z = transform.position.z;
        player.transform.position = new Vector3(x, player.transform.position.y, z);

        Vector3 startPos = player.transform.position;
        Vector3 targetPos;

        //Set Player Climb Location
        if (climbWall)
            targetPos = new Vector3(x, targetHeights[0] + 0.1f, z);
        else {
            if (freezeAxes[0]) x = player.transform.position.x; else x = Target.position.x;
            if (freezeAxes[2]) z = player.transform.position.z; else z = Target.position.z;
            targetPos = new Vector3(x, targetHeights[1] - 0.05f, z);
        }
        player.transform.LookAt(LookAtPos());
        if (!climbWall)
            player.transform.Rotate(new Vector3(0, 180, 0));

        //Move Player to climb position
        float time = 0f;
        player.Animator.SetBool("isClimbing", true);
        if (climbWall)
            player.Animator.SetBool("ClimbAtTop", false);
        else
            player.Animator.SetBool("ClimbAtTop", true);
        player.Animator.SetTrigger("InteractionStart");
        if (climbWall) {
            while (time < 1) {
                time += Time.deltaTime * (GlobalNumbers.LEDGE_UP_INTERACTION_TIME * 3);
                player.transform.position = Vector3.Lerp(startPos, targetPos, time);
                yield return null;
            }
        } else {
            while (time < 1) {
                time += Time.deltaTime * (GlobalNumbers.LEDGE_UP_INTERACTION_TIME);
                player.transform.position = Vector3.Lerp(startPos, targetPos, time);
                yield return null;
            }
        }
        SceneController.Instance.Player.Climbing(targetHeights);
        SceneController.Instance.Player.enabled = true;
        isMoving = false;
    }
}
