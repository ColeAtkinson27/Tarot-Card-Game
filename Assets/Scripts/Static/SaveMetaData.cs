using System;
using static Constants;

/** <summary>Holds saved game data across all saves used for the Save/Load screen.</summary> */
[Serializable]
public class SaveMetaData {
    /** <summary>Difficulty of each saved game.</summary> */
    public int[] SavesDifficulty = new int[MAX_SAVE_SLOTS];
    /** <summary>Total play time of each saved game.</summary> */
    public int[] SavesPlayTime = new int[MAX_SAVE_SLOTS];
    /** <summary>Total collection progress of each saved game (x >= 0).</summary> */
    public int[] SavesProgress = new int[MAX_SAVE_SLOTS];
}
