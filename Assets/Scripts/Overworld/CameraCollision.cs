using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {
    public LayerMask collisionLayer;

    [SerializeField] private float clippingValue;
    [SerializeField] private float collisionCushion = 2f;

    public bool colliding = false;
    public bool minColliding = false;
    public Vector3[] cameraClipPoints;
    public Vector3[] inputCameraClipPoints;
    public Vector3[] collidingCameraClipPoints;

    private Camera camera;
    private CameraMovement cameraMovement;

    void Awake() {
        camera = GetComponent<Camera>();
        cameraMovement = GetComponent<CameraMovement>();
        cameraClipPoints = new Vector3[5];
        inputCameraClipPoints = new Vector3[5];
        collidingCameraClipPoints = new Vector3[5];
        clippingValue = 1 + camera.nearClipPlane;
    }

    public bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition) {
        Ray ray;
        float distance;
        for (int i = 0; i < clipPoints.Length; i++) {
            ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
            distance = Vector3.Distance(clipPoints[i], fromPosition);
            if (Physics.Raycast(ray, distance, collisionLayer))
                return true;
        }
        return false;
    }

    public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray) {
        if (!camera)
            return;

        intoArray = new Vector3[5];
        float z = camera.nearClipPlane;
        float x = Mathf.Tan(camera.fieldOfView / clippingValue * Mathf.Deg2Rad) * z;
        float y = x / camera.aspect;

        //Top left
        intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition;
        //Top right
        intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;
        //Bottom left
        intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;
        //Bottom right
        intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition;
        //Camera Position
        intoArray[4] = cameraPosition;
        //- camera.transform.forward
    }

    public void UpdateCameraClipPoints (Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray, bool cushion) {
        UpdateCameraClipPoints(cameraPosition, atRotation, ref intoArray);
        collisionCushion = cameraMovement.CurrentDis / cameraMovement.TargetDis * 2;
        if (true) {
            for (int i = 0; i < 4; i++) {
                intoArray[i].x *= collisionCushion;
                intoArray[i].y *= collisionCushion;
            }
        }
    }

    public float GetAdjustedDistanceWithRayFrom(Vector3 from) {
        float distance = -1;
        Ray ray;
        RaycastHit hit;
        for (int i = 0; i < inputCameraClipPoints.Length; i++) {
            ray = new Ray(from, inputCameraClipPoints[i] - from);
            if (Physics.Raycast(ray, out hit)) {
                if (distance == -1)
                    distance = hit.distance;
                else {
                    if (hit.distance < distance)
                        distance = hit.distance;
                }
            }
        }

        if (distance == -1)
            return 0;
        return distance;
    }

    public float GetMinDistanceWithRayFrom (Vector3 from) {
        float distance = -1;
        Ray ray;
        RaycastHit hit;
        for (int i = 0; i < collidingCameraClipPoints.Length; i++) {
            ray = new Ray(from, collidingCameraClipPoints[i] - from);
            if (Physics.Raycast(ray, out hit)) {
                if (distance == -1)
                    distance = hit.distance;
                else {
                    if (hit.distance < distance)
                        distance = hit.distance;
                }
            }
        }

        if (distance == -1)
            return 0;
        return distance;
    }

    public void CheckColliding (Vector3 targetPosition) {
        if (CollisionDetectedAtClipPoints(inputCameraClipPoints, targetPosition))
            colliding = true;
        else
            colliding = false;
    }

    public void CheckMinColliding (Vector3 targetPosition) {
        if (CollisionDetectedAtClipPoints(collidingCameraClipPoints, targetPosition))
            minColliding = true;
        else
            minColliding = false;
    }

}
