using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>Controls the camera movement for the player in the overworld.</summary> */
public class CameraMovement : MonoBehaviour {

    /** <summary>The target the camera is following.</summary> */
    [SerializeField] private Transform target;

    [SerializeField] private float MOUSE_CAMERA_SPEED = 3f;
    [Header("Position and Orbit Settings")]
    /** <summary>Controls the speed of the camera while moving.</summary> */
    [SerializeField] private float smoothLook = 3f;
    /** <summary>Controls the speed of the camera while orbiting.</summary> */
    [SerializeField] private float smoothRotate = 50f;
    /** <summary>The distance from the target while playing the game.</summary> */
    [SerializeField] private float gameDistance = 5;
    /** <summary>The distance from the target while the game is paused.</summary> */
    [SerializeField] private float pauseDistance = 8;
    /** <summary>The current orbit of the camera around the target.</summary> */
    [SerializeField] private float orbitRotation;
    /** <summary>The target orbit of the camera around the target.</summary> */
    [SerializeField] private float targetOrbitRotation;
    /** <summary>The tilt of the camera from looking at the target.</summary> */
    [SerializeField] private float cameraTilt;
    /** <summary>The tilt of the camera from looking at the target.</summary> */
    [SerializeField] private float targetTilt;
    /** <summary>The maximum tilt of the camera, for how high the camera can look.</summary> */
    [SerializeField] private float maxTilt;
    /** <summary>The minimum tilt of the camera, for how low the camera can look.</summary> */
    [SerializeField] private float minTilt;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float zoomSpeed;

    [Header("Camera and Player Information")]
    /** <summary>The transform object to center the screen while playing the game.</summary> */
    [SerializeField] private Transform standardTarget;
    /** <summary>The transform object to center the screen while the game is paused.</summary> */
    [SerializeField] private Transform pausedTarget;
    /** <summary>The vertical distance between the camera the target object during the game.</summary> */
    [SerializeField] private float gameOffsetHeight = 3f;
    /** <summary>The vertical distance between the camera the target object when the game is paused.</summary> */
    [SerializeField] private float pauseOffsetHeight = 0f;

    [Header("Debug Settings")]
    /** <summary>Debug option. Displays camera lines while in the editor.</summary> */
    [SerializeField] private bool drawLines = true;

    /** <summary>The camera collider, which handles physics against walls.</summary> */
    private CameraCollision camCollider;

    /** <summary>The target rotation to rotate the camera towards.</summary> */
    private Quaternion targetRotation;

    /** <summary>The input position for the camera based on the player's input.</summary> */
    private Vector3 inputDestination = Vector3.zero;
    /** <summary>The desired position for the camera to follow. Stops moving when input collides with a wall.</summary> */
    private Vector3 collidingDestination = Vector3.zero;
    /** <summary>The current set destination for the camera.</summary> */
    private Vector3 destination = Vector3.zero;
    /** <summary>The target position to move the camera towards</summary> */
    private Vector3 targetPosition = Vector3.zero;
    /** <summary>The camera's current velocity.</summary> */
    private Vector3 velocity = Vector3.zero;

    /** <summary>The input from the controller/keyboard.</summary> */
    private float input;
    private float tiltInput;

    /** <summary>The distance from the target. Changes when colliding with walls.</summary> */
    [SerializeField] private float inputedDistance = 5;
    /** <summary>The distance from the target. Changes when colliding with walls.</summary> */
    [SerializeField] private float minDistance = 5;
    /** <summary>The current set distance from the target.</summary> */
    [SerializeField] private float distanceFromTarget = 5;
    /** <summary>The current set height offset from the target.</summary> */
    private float offsetHeight;

    /** <summary>Checks whether or not the game and camera are paused.</summary> */
    public bool Paused { get; private set; }
    public float CurrentDis { get { return inputedDistance; } set { inputedDistance = value; } }
    public float TargetDis { get { return distanceFromTarget; } }

    void Start() {
        try { SceneController.Instance.Camera = this; } catch (System.Exception e) {
            Debug.Log("<color=red>Camera Controller setup error:</color> Please return to main menu to fix issue.");
        }
        try {
            target = standardTarget;
            offsetHeight = gameOffsetHeight;
            camCollider = GetComponent<CameraCollision>();

            MoveWithTarget();
        } catch (System.Exception e) {
            Debug.Log("<color=red>Camera Controller setup error:</color> " + e.ToString() + "\n" + e.StackTrace);
        }
        maxZoom = gameDistance;
        camCollider.UpdateCameraClipPoints(destination, transform.rotation, ref camCollider.inputCameraClipPoints, true);
        camCollider.UpdateCameraClipPoints(collidingDestination, transform.rotation, ref camCollider.collidingCameraClipPoints);
        camCollider.UpdateCameraClipPoints(transform.position, transform.rotation, ref camCollider.cameraClipPoints);
    }

    void Update () {
        //Move and rotate camera
        GetInput();
        RotateAroundTarget();
        LookAtTarget();
    }

    void FixedUpdate() {
        //Check if camera is colliding with terrain and adjust
        //Update camera's view points
        camCollider.UpdateCameraClipPoints(destination, transform.rotation, ref camCollider.inputCameraClipPoints);
        camCollider.UpdateCameraClipPoints(inputDestination, transform.rotation, ref camCollider.collidingCameraClipPoints);
        camCollider.UpdateCameraClipPoints(transform.position, transform.rotation, ref camCollider.cameraClipPoints);

        //Draw Debug lines
        if (drawLines) {
            for (int i = 0; i < 5; i++) {
                Debug.DrawLine(target.position, camCollider.collidingCameraClipPoints[i], Color.blue);
                Debug.DrawLine(target.position, camCollider.inputCameraClipPoints[i], Color.red);
                Debug.DrawLine(target.position, camCollider.cameraClipPoints[i], Color.green);
            }
        }

        MoveWithTarget();

        camCollider.CheckColliding(targetPosition);
        camCollider.CheckMinColliding(targetPosition);
        inputedDistance = camCollider.GetAdjustedDistanceWithRayFrom(targetPosition);
        minDistance = camCollider.GetMinDistanceWithRayFrom(targetPosition) - 0.3f;
    }

    /** 
     * <summary>Changes the camera position and angle when pausing the game.</summary>
     * <param name="state">Whether or not the game is being paused.</param>
     */
    public void Pause (bool state) {
        Paused = state;
        if (state == true) {
            target = pausedTarget;
            distanceFromTarget = pauseDistance;
            offsetHeight = pauseOffsetHeight;
        } else if (state == false) {
            target = standardTarget;
            distanceFromTarget = gameDistance;
            offsetHeight = gameOffsetHeight;
        }
    }

    /**
     * <summary>Follow the player as they move.</summary>
     */
    private void MoveWithTarget () {
        targetPosition = target.position + new Vector3(0, offsetHeight, 0);
        destination = Quaternion.Euler(targetTilt, targetOrbitRotation, 0) * -Vector3.forward * distanceFromTarget;
        destination += target.position;
        inputDestination = Quaternion.Euler(targetTilt, targetOrbitRotation, 0) * -Vector3.forward * distanceFromTarget;
        inputDestination += target.position;
        collidingDestination = Quaternion.Euler(targetTilt, targetOrbitRotation, 0) * -Vector3.forward * distanceFromTarget;
        collidingDestination += target.position;

        //Check if camera is colliding with wall and stop input rotation. Else, use expected position
        if (camCollider.colliding) {
            inputDestination = Quaternion.Euler(targetTilt, orbitRotation, 0) * -Vector3.forward * inputedDistance;
            inputDestination += target.position;
            collidingDestination = Quaternion.Euler(targetTilt, orbitRotation, 0) * -Vector3.forward * inputedDistance;
            collidingDestination += target.position;
            targetOrbitRotation = orbitRotation;

            if (camCollider.minColliding) {
                collidingDestination = Quaternion.Euler(targetTilt, orbitRotation, 0) * -Vector3.forward * minDistance;
                collidingDestination += target.position;
                transform.position = Vector3.Lerp(transform.position, collidingDestination, smoothLook * Time.deltaTime);
            } else
                transform.position = Vector3.Lerp(transform.position, inputDestination, smoothLook * Time.deltaTime);
        } else {
            targetOrbitRotation = orbitRotation;
            transform.position = Vector3.Lerp(transform.position, destination, smoothLook * Time.deltaTime);
        }
        if (cameraTilt > maxTilt)
            cameraTilt = maxTilt;
        else if (cameraTilt < minTilt)
            cameraTilt = minTilt;
        targetTilt = cameraTilt;
    }
    /**
     * <summary>Rotate the camera to look at the player as they move.</summary>
     */
    private void LookAtTarget () {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothLook * Time.deltaTime);

        bool fadeDist = false;
        if (distanceFromTarget <= 1f && distanceFromTarget >= 0.5f)
            fadeDist = true;

        distanceFromTarget += -Input.GetAxis("Mouse Wheel") * zoomSpeed;
        if (distanceFromTarget < minZoom)
            distanceFromTarget = minZoom;
        else if (distanceFromTarget > maxZoom)
            distanceFromTarget = maxZoom;

        if (distanceFromTarget <= 1f && distanceFromTarget >= 0.5f)
            fadeDist = true;
        
        //if (fadeDist == true) {
        //    Material[] mats = SceneController.Instance.Player.GetComponent<Renderer>().materials;
        //    Color color;
        //    if (distanceFromTarget > 1) {
        //        for (int i = 0; i < mats.Length; i++) {
        //            color = mats[i].color;
        //            color.a = 1;
        //            mats[i].color = color;
        //        }
        //    } else if (distanceFromTarget < 0.5f) {
        //        for (int i = 0; i < mats.Length; i++) {
        //            color = mats[i].color;
        //            color.a = 0;
        //            mats[i].color = color;
        //        }
        //    } else {
        //        for (int i = 0; i < mats.Length; i++) {
        //            color = mats[i].color;
        //            color.a = (distanceFromTarget - 0.5f) * 2;
        //            mats[i].color = color;
        //        }
        //    }
        //    SceneController.Instance.Player.GetComponent<Renderer>().materials = mats;
        //}
    }

    /** <summary>Rotates the camera around the target</summary> */
    private void RotateAroundTarget () {
            if (Settings.InvertCameraX)
                orbitRotation += -input * smoothRotate * Time.deltaTime * (0.55f * 2);
            else
                orbitRotation += input * smoothRotate * Time.deltaTime * (0.55f * 2);

            if (Settings.InvertCameraY)
                cameraTilt += tiltInput * smoothRotate * Time.deltaTime * (0.55f * 2);
            else
                cameraTilt += -tiltInput * smoothRotate * Time.deltaTime * (0.55f * 2);
    }

    private void GetInput() {
        if (Input.GetMouseButton(0)) {
            input = Input.GetAxis("Camera Pan Mouse") * MOUSE_CAMERA_SPEED;
            tiltInput = Input.GetAxis("Camera Tilt") * MOUSE_CAMERA_SPEED;
        } else {
            input = Input.GetAxis("Camera Pan");
            tiltInput = 0;
        }
    }

    /** <summary>The current orbit around the target</summary> */
    public float CurrentGameOrbit {
        get { return orbitRotation; }
        set { orbitRotation = value; }
    }

    public float CurrentGameTilt {
        get { return cameraTilt; }
        set { cameraTilt = value; }
    }
}
