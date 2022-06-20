using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
 * <summary>Holds the player's save data from a file.</summary>
 */
[Serializable]
public class PlayerSaveData {

    public int HP { get; set; }
    public int MP { get; set; }
    public int MaxHP { get; set; }
    public int MaxMP { get; set; }

    /** <summary>The name of the scene the player saved in.</summary> */
    public string SceneName { get; set; }
    /** <summary>The total length of time the player has played the current save state.</summary> */
    public int PlayTimeInSeconds;
    /** <summary>The x,y,z coordinates of the player's position within the saved scene.</summary> */
    private float[] PlayerCoords = new float[3];
    /** <summary>The x,y,z,w values of the player's rotation within the saved scene.</summary> */
    private float[] PlayerRot = new float[4];
    /** <summary>The angle of the camera's orbit around the player within the saved scene.</summary> */
    public float CameraRot = 0f;

    /** <summary>The list of booleans used to keep track of game progress.</summary> */
    public List<KeyValuePair<string, bool>> gameChecks = new List<KeyValuePair<string, bool>>();

    /**
     * <summary>Converts the player's transform location into a serializable form.</summary>
     * <param name="coord">The player's trasform location.</param>
     */
    public void SetPlayerCoords (Vector3 coord) {
        PlayerCoords[0] = coord.x;
        PlayerCoords[1] = coord.y;
        PlayerCoords[2] = coord.z;
    }
    /**
     * <summary>Converts the player's transform rotation into a serializable form.</summary>
     * <param name="rot">The player's transform rotation.</param>
     */
    public void SetPlayerRot (Quaternion rot) {
        PlayerRot[0] = rot.x;
        PlayerRot[1] = rot.y;
        PlayerRot[2] = rot.z;
        PlayerRot[3] = rot.w;
    }

    /**
     * <summary>Returns the player's saved location.</summary>
     * <returns>The player's saved transform location.</returns>
     */
    public Vector3 GetPlayerCoords() {
        return new Vector3(PlayerCoords[0], PlayerCoords[1], PlayerCoords[2]);
    }
    /**
     * <summary>Returns the player's saved rotation.</summary>
     * <returns>The player's saved transform rotation.</returns>
     */
    public Quaternion GetPlayerRot () {
        return new Quaternion(PlayerRot[0], PlayerRot[1], PlayerRot[2], PlayerRot[3]);
    }
}
