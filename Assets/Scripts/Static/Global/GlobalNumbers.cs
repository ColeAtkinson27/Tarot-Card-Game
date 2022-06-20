using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>Hold all of the static readonly values for various classes.</summary> */
public static class GlobalNumbers {
    public static readonly float NPC_INTERACTION_DISTANCE = 2f;

    //Interaction times
    /** <summary>The rate it takes for the player to climb a wall.</summary> */
    public static readonly float CLIMB_INTERACTION_RATE = 1f;
    /** <summary>The time it takes for the player to jump across a short gap.</summary> */
    public static readonly float JUMP_INTERACTION_TIME = 1f;
    /** <summary>The time it takes for the player to jump down a short ledge.</summary> */
    public static readonly float LEDGE_DOWN_INTERACTION_TIME = 1f;
    /** <summary>The rate it takes for the player to fall down from a high ledge.</summary> */
    public static readonly float LEDGE_FALL_INTERACTION_RATE = 5f;
    /** <summary>The time it takes for the player to climb up onto a ledge.</summary> */
    public static readonly float LEDGE_UP_INTERACTION_TIME = 1f;
    /** <summary>The rate it takes for the player to shimmy across a wall.</summary> */
    public static readonly float SHIMMY_INTERACTION_RATE = 1f;
    /** <summary>The rate it takes for the player to swing across a large gap.</summary> */
    public static readonly float SWING_INTERACTION_RATE = 6f;
}
