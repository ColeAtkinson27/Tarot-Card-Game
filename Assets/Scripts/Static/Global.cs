using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global {
    public static int Difficulty { get; private set; }

    public static void SetDifficulty (int difficulty) {
        if (difficulty > 0 && difficulty <= 3)
            Difficulty = difficulty;
    }
}