﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad {
    public static PlayerSaveData player = new PlayerSaveData();
    public static SaveMetaData saveMetas = new SaveMetaData();

    private static int startTime;

    public static void SaveMeta () {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/tarotSaves.smd");
        bf.Serialize(file, saveMetas);
        file.Close();
    }
    public static void LoadMeta() {
        if (File.Exists(Application.persistentDataPath + "/tarotSaves.smd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/tarotSaves.smd", FileMode.Open);
            try {
                saveMetas = (SaveMetaData)bf.Deserialize(file);
                file.Close();
            } catch (Exception e) {
                file.Close();
                SaveMeta();
            }
        }
    }

    public static void Save (int slot) {
        if (slot >= 0 && slot < 8) {
            SaveGameState();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/playerData" + slot + ".tgd");
            bf.Serialize(file, player);
            file.Close();

            saveMetas.SavesPlayTime[slot] = player.PlayTimeInSeconds;
            SaveMeta();
            Debug.Log("<color=green>Game Saved:</color> Game saved to slot " + slot + ". Difficulty = " + saveMetas.SavesDifficulty[slot] + ".");
        } else {
            Debug.Log("<color=red>Game Save error:</color> Failed to save game to slot " + slot + ". Slot out of bounds.");
        }
	}

	public static void Load (int slot) {
        if (slot >= 0 && slot < 8) {
            if (File.Exists(Application.persistentDataPath + "/playerData" + slot + ".tgd")) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/playerData" + slot + ".tgd", FileMode.Open);
                player = (PlayerSaveData)bf.Deserialize(file);
                file.Close();
                LoadGameState();
            }
        } else {
            Debug.Log("<color=red>Game Load error:</color> Failed to load game from slot " + slot + ". Slot out of bounds.");
        }
    }

    public static void Delete (int slot) {
        if (slot >= 0 && slot < 8) {
            if (File.Exists(Application.persistentDataPath + "/playerData" + slot + ".tgd")) {
                File.Delete(Application.persistentDataPath + "/playerData" + slot + ".tgd");
                saveMetas.SavesDifficulty[slot] = 0;
                saveMetas.SavesPlayTime[slot] = 0;
                saveMetas.SavesProgress[slot] = 0;
                SaveMeta();
            }
        } else {
            Debug.Log("<color=red>Game Delete error:</color> Failed to load game from slot " + slot + ". Slot out of bounds.");
        }
    }

    private static void SaveGameState () {
        int timestamp = (int) Time.realtimeSinceStartup;
        player.PlayTimeInSeconds += timestamp - startTime;
        startTime = timestamp;
    }

    private static void LoadGameState () {
        try {
            startTime = (int)Time.realtimeSinceStartup;
            LevelDirectory.CurrentLevel = player.CurrentLevel;
            SceneController.Instance.LoadLevel();
        } catch (Exception e) {
            Debug.Log("<color=red>Game Load error:</color> " + e.ToString() + "\n" + e.StackTrace);
            SceneController.Instance.ReturnToMainMenu();
            //UICanvas.Instance.DisplayMessage("Error loading save file.");
        }
    }
}
