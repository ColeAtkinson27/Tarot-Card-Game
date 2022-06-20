using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>Holds saved game data across all saves used for the Save/Load screen.</summary> */
[Serializable]
public class SaveMetaData {
    /** <summary>Total play time of each saved game.</summary> */
    public int[] SavesPlayTime = new int[8];
    /** <summary>Total unlocked cards of each saved game (x >= 0).</summary> */
    public int[] SavesCardProg = new int[8];
}
