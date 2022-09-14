using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
 * <summary>Holds the player's save data from a file.</summary>
 */
[Serializable]
public class PlayerSaveData {

    /** <summary>The index of the level the player saved in.</summary> */
    public int CurrentLevel { get; set; }
    /** <summary>The total length of time the player has played the current save state.</summary> */
    public int PlayTimeInSeconds { get; set; }

    /** <summary>The list of booleans used to keep track of game progress.</summary> */
    public List<KeyValuePair<string, bool>> gameChecks = new List<KeyValuePair<string, bool>>();
}
