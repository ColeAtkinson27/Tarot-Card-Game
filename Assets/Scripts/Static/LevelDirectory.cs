using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelDirectory {

    private static int currentLevel = 0;
    public static int CurrentLevel { 
        get { return currentLevel; } 
        set { currentLevel = value; } 
    }

    private static List<Level> Levels;

    public static void Initialize() {
            if (Levels == null) {
                Levels = new List<Level>();
                TextAsset levelsFile = (TextAsset)Resources.Load("CSV/levels");
                string[] levels = levelsFile.text.Split('\n');
                foreach (string level in levels) {
                    try {
                        string[] properties = level.Split(',');
                    if (properties.Length > 1) {
                            if (!properties[0].Equals("ID")) {
                                Level newItem = new Level();
                                newItem.levelID = int.Parse(properties[0]);
                                newItem.sceneName = properties[1];
                                newItem.backdrop = properties[2];
                                if (properties.Length > 3) {
                                    newItem.encounters = new List<string>();
                                    for (int i = 3; i < properties.Length; i++) {
                                        newItem.encounters.Add(properties[i]);
                                    }
                                }
                                Levels.Add(newItem);
                            }
                        }
                    } catch (System.Exception e) {
                        Debug.Log("Levels CSV read error: " + e.ToString() + "\n" + e.StackTrace);
                    }
            }
            Debug.Log("<color=green>Level Directory loaded.</color> " + Levels.Count + " levels");
        }
        
    }

    public static string GetSceneName() {
        return Levels[currentLevel].sceneName;
    }
    public static string GetSceneBackdrop() {
        return Levels[currentLevel].backdrop;
    }
    public static List<int> GetEncounter() {
        if (Levels[currentLevel].encounters == null || Levels[currentLevel].encounters.Count == 0) return null;

        int eNum = Random.Range(0, Levels[currentLevel].encounters.Count);
        string encounterInfo = Levels[currentLevel].encounters[eNum];
        string[] properties = encounterInfo.Split('|');
        List<int> enemies = new List<int>();

        foreach (string e in properties) { enemies.Add(int.Parse(e)); }
        return enemies;
    }

    private static string GetEnemy(int enemyNo) {
        switch(enemyNo) {
            case 1: return "Enchanted Tome";
            case 2: return "Animated Armor";
            case 3: return "Faithful Zealot";
            case 4: return "Virtuous Adherent";
            case 5: return "Pain-Drunk Apostle";
            default: return null;
        }
    }

    public static void LastCheckpoint() {
        bool checkpointFound = false;
        while (!checkpointFound) {
            switch (Levels[currentLevel].sceneName) {
                case "Combat":
                    currentLevel -= 1;
                    break;
                case "Rest Area":
                    checkpointFound = true;
                    currentLevel -= 1;
                    break;
                default:
                    checkpointFound = true;
                    currentLevel = -1;
                    break;
            }
        }
        SceneController.Instance.NextLevel();
    }
}

public class Level {
    public int levelID;
    public string sceneName;
    public string backdrop;
    public List<string> encounters;
}
