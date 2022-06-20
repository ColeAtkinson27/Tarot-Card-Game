using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * <summary>Controls player movement in the overworld.</summary>
 */
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float DistanceFromLeft;
    [SerializeField] private float DistanceFromRight;

    [SerializeField] private bool isClimbing;
    [SerializeField] private bool isShimmying;

    [Header("Run values")]
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float walkTurnSpeed;
    [SerializeField] public float runTurnSpeed;

    /** <summary>How fast the player moves in the overworld.</summary> */
    [SerializeField] public float moveSpeed;
    /** <summary>How fast the player turns when moving.</summary> */
    [SerializeField] public float turnSpeed;

    [Header("Climbing Properties")]
    [SerializeField] private float climbSpeed;
    [SerializeField] private float bottomY;
    [SerializeField] private float topY;

    [Header("Shimmying Properties")]
    [SerializeField] private float shimmySpeed;
    [SerializeField] private Vector3 leftTarget;
    [SerializeField] private Vector3 rightTarget;

    [Header ("Indicators")]
    [SerializeField] public GameObject interactionBubble;

    /** <summary>The animation controller attatched to the game object.</summary> */
    [SerializeField] private Animator animator;
    /** <summary>The rigid body used to move the game object.</summary> */
    private NavMeshAgent navAgent;
    /** <summary>The main camera attatched to the player. Used for rotation.</summary> */
    private Transform mainCamera;

    /** <summary>The targeted quaternion to rotate towards.</summary> */
    private Quaternion targetRotation;
    /** <summary>The XY input from the controller/keyboard.</summary> */
    private Vector2 input;

    /** <summary>The targeted angle the player is facing relative to the camera.</summary> */
    private float angle;

    private bool disable;

    public Animator Animator { get { return animator; } }

    void Start() {
        try { SceneController.Instance.Player = this; } catch (System.Exception e) {
            Debug.Log("<color=red>Player controller setup error:</color> Please return to main menu to fix issue.");
        }
        try {
            animator = GetComponentInChildren<Animator>();
            navAgent = GetComponent<NavMeshAgent>();
            mainCamera = Camera.main.transform;
        } catch (System.Exception e) {
            Debug.Log("<color=red>Player controller setup error:</color> " + e.ToString() + "\n" + e.StackTrace);
        }
    }

    void Update() {
        if (Input.GetAxis("Sprint") != 0) {
            moveSpeed = runSpeed;
            turnSpeed = runTurnSpeed;
        } else if (moveSpeed == runSpeed) {
            moveSpeed = walkSpeed;
            turnSpeed = walkTurnSpeed;
        }
    }

    void FixedUpdate() {
        try {
            if (!disable) {
                GetInput();
                if (isClimbing) {
                    transform.position += Vector3.up * climbSpeed * input.y;
                    animator.SetFloat("ClimbDir", input.y);
                    if (transform.position.y >= topY) {
                        disable = true;
                        StartCoroutine(EndClimb(true));
                    } else if (transform.position.y <= bottomY) {
                        disable = true;
                        StartCoroutine(EndClimb(false));
                    }
                } else if (isShimmying) {
                    transform.position += transform.right * shimmySpeed * input.x;
                    DistanceFromLeft = Vector3.Distance(transform.position, leftTarget);
                    DistanceFromRight = Vector3.Distance(transform.position, rightTarget);
                    if (input.x > 0)
                        animator.SetBool("ShimmyDir", false);
                    else if (input.x < 0)
                        animator.SetBool("ShimmyDir", true);
                    if (DistanceFromLeft < 0.25) {
                        disable = true;
                        StartCoroutine(EndShimmy());
                    } else if (DistanceFromRight < 0.25) {
                        disable = true;
                        StartCoroutine(EndShimmy());
                    }
                } else {
                    if (Mathf.Abs(input.x) != 0 || Mathf.Abs(input.y) != 0) {
                        CalculateDirection();
                        Rotate();
                        Move();
                    }
                }
                if (Mathf.Abs(input.x) == 0 && Mathf.Abs(input.y) == 0) {
                    animator.SetBool("isRunning", false);
                    return;
                }
                animator.SetBool("isRunning", true);
            }
        } catch (System.Exception e) {
            Debug.Log("<color=red>Player controller update error:</color> " + e.ToString() + "\n" + e.StackTrace);
        }
    }

    //====================//
    //====CALCULATIONS====//
    //====================//

    /** <summary>Uses the camera to determine the current direction of the player.</summary> */
    private void CalculateDirection () {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += mainCamera.eulerAngles.y;
    }
    /** <summary>Retrieves the input from the X and Y axes to be used.</summary> */
    private void GetInput () {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

    //=====================//
    //======TRANSFORM======//
    //=====================//

    /** <summary>Rotates the player towards the targeted rotation.</summary> */
    private void Rotate () {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, 
            targetRotation, turnSpeed * Time.deltaTime);
    }
    /** <summary>Moves the player forward, relative to the rotation and camera.</summary> */
    private void Move () {
        navAgent.velocity = transform.forward * moveSpeed * Time.deltaTime;
        //transform.position += transform.forward * moveSpeed 
        //    * Time.deltaTime;
    }

    //=====================//
    //=====INTERACTIONS====//
    //=====================//

    public void Climbing (float[] targetHeights) {
        animator.SetBool("isRunning", false);
        if (targetHeights.Length < 2) {
            Debug.Log("<color=red>Player controller climbing error:</color> Please include at least 2 floats.");
            return;
        }
        isClimbing = true;
        bottomY = targetHeights[0];
        topY = targetHeights[1];
    }

    internal void Shimmying (Vector3[] targetPositions) {
        animator.SetBool("isRunning", false);
        if (targetPositions.Length < 2) {
            Debug.Log("<color=red>Player controller shimmying error:</color> Please include at least 2 floats.");
            return;
        }
        isShimmying = true;
        leftTarget = targetPositions[0];
        rightTarget = targetPositions[1];
    }

    private IEnumerator EndClimb (bool atTop) {
        animator.SetBool("isRunning", false);
        isClimbing = false;
        Vector3 startPos = transform.position;
        Vector3 targetPos;
        if (atTop)
            targetPos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z) + (transform.forward * 0.5f);
        else
            targetPos = new Vector3(transform.position.x, transform.position.y -0.25f, transform.position.z) + (-transform.forward * 0.5f);

        float time = 0f;
        if (atTop) {
            animator.SetBool("ClimbAtTop", true);
            animator.SetBool("isClimbing", false);
            while (time < 1) {
                time += Time.deltaTime * (GlobalNumbers.LEDGE_UP_INTERACTION_TIME);
                transform.position = Vector3.Lerp(startPos, targetPos, time);
                yield return null;
            }
        } else {
            animator.SetBool("ClimbAtTop", false);
            animator.SetBool("isClimbing", false);
            while (time < 1) {
                time += Time.deltaTime * (GlobalNumbers.LEDGE_UP_INTERACTION_TIME);
                transform.position = Vector3.Lerp(startPos, targetPos, time);
                yield return null;
            }
        }
        navAgent.enabled = true;
        disable = false;
    }

    private IEnumerator EndShimmy () {
        isShimmying = false;
        animator.SetBool("isRunning", false);
        Vector3 startPos = transform.position;
        Vector3 targetPos = transform.position - (transform.forward * 0.5f);

        float time = 0f;
        animator.SetBool("isShimmying", false);
        while (time < 1) {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, time);
            yield return null;
        }
        navAgent.enabled = true;
        disable = false;
    }
}
