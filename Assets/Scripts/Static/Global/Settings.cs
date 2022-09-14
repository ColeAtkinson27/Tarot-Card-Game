using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>Holds information about the game settings.</summary> */
public static class Settings {

    /** <summary>The speed of rotation when turning the camera around the player.</summary> */
    public static float CameraSensitivity { get; set; }
    /** <summary>The direction the camera turns horizontally. When false, Q turns CCW and E turns CW.</summary> */
    public static bool InvertCameraX { get; set; }
    public static bool InvertCameraY { get; set; }
    /** <summary>Applies subtitles during animated cutscenes.</summary> */
    public static bool Subtitles { get; set; }
    /** <summary>The speed of text as it is written on the screen. Setting to 0 turns off scrolling text.</summary> */
    public static int TextSpeed { get; set; }

    /** <summary>Controls all volume in the game.</summary> */
    public static float MasterVolume { get; set; }
    /** <summary>Controls music volume in the game.</summary> */
    public static float MusicVolume { get; set; }
    /** <summary>Controls sound effects and ambience volume in the game.</summary> */
    public static float EffectsVolume { get; set; }
    /** <summary>Controls character dialogue volume in the game.</summary> */
    public static float VoiceVolume { get; set; }

    /** <summary>The game window state of windowed or fullscreen.</summary> */
    public static bool Fullscreeen { get; set; }
    /** <summary>The window size in windowed mode.</summary> */
    public static int Resolution { get; set; }
    /** <summary>The monitor to display the game window.</summary> */
    public static int Monitor { get; set; }
    /** <summary>The visual quality of textures, lighting, shadows, particles, etc.</summary> */
    public static int Quality { get; set; }
    /** <summary>The gamma of the screen.</summary> */
    public static float Brightness { get; set; }

    /** <summary>Loads the players settings saved in the Player Preferences.</summary> */
    public static void LoadSettings () {
        //Check if Settings have been set up.
        //If no --> set default settings
        //If yes --> load default settings
        if (PlayerPrefs.GetString("Player Prefs Exists").Equals(null)) {
            CameraSensitivity = 0.55f;
            InvertCameraX = false;
            InvertCameraY = false;
            Subtitles = false;
            TextSpeed = 2;
            MasterVolume = 1f;
            MusicVolume = 1f;
            EffectsVolume = 1f;
            VoiceVolume = 1f;
            Fullscreeen = true;
            Resolution = 0;
            Monitor = 0;
            Quality = 0;
            Brightness = 0.55f;
            PlayerPrefs.SetString("Player Prefs Exists", "Exists");
            ConfirmSettings();
        } else {
            int boolBuffer;
            CameraSensitivity = PlayerPrefs.GetFloat("Camera Sensitivity");
            boolBuffer = PlayerPrefs.GetInt("Invert Camera X");
            if (boolBuffer == 1)
                InvertCameraX = true;
            else
                InvertCameraX = false;
            boolBuffer = PlayerPrefs.GetInt("Invert Camera Y");
            if (boolBuffer == 1)
                InvertCameraY = true;
            else
                InvertCameraY = false;
            boolBuffer = PlayerPrefs.GetInt("Subtitles");
            if (boolBuffer == 1)
                Subtitles = true;
            else
                Subtitles = false;
            TextSpeed = PlayerPrefs.GetInt("Text Speed");

            MasterVolume = PlayerPrefs.GetFloat("Master Volume");
            MusicVolume = PlayerPrefs.GetFloat("Music Volume");
            EffectsVolume = PlayerPrefs.GetFloat("Effects Volume");
            VoiceVolume = PlayerPrefs.GetFloat("Voice Volume");

            boolBuffer = PlayerPrefs.GetInt("Fullscreen");
            if (boolBuffer == 1)
                Fullscreeen = true;
            else
                Fullscreeen = false;
            Resolution = PlayerPrefs.GetInt("Resolution");
            Monitor = PlayerPrefs.GetInt("Monitor");
            Quality = PlayerPrefs.GetInt("Quality");
            Brightness = PlayerPrefs.GetFloat("Brightness");
        }
        SetProperties();
        Debug.Log("<color=green>Player settings loaded.</color>");
    }
    /** <summary>Saves the current game settings into the Player Preferences</summary> */
    public static void ConfirmSettings () {
        PlayerPrefs.SetFloat("Camera Sensitivity", CameraSensitivity);
        if (InvertCameraX)
            PlayerPrefs.SetInt("Invert Camera X", 1);
        else
            PlayerPrefs.SetInt("Invert Camera X", 0);
        if (InvertCameraY)
            PlayerPrefs.SetInt("Invert Camera Y", 1);
        else
            PlayerPrefs.SetInt("Invert Camera Y", 0);
        if (Subtitles)
            PlayerPrefs.SetInt("Subtitles", 1);
        else
            PlayerPrefs.SetInt("Subtitles", 0);
        PlayerPrefs.SetInt("Text Speed", TextSpeed);

        PlayerPrefs.SetFloat("Master Volume", MasterVolume);
        PlayerPrefs.SetFloat("Music Volume", MusicVolume);
        PlayerPrefs.SetFloat("Effects Volume", EffectsVolume);
        PlayerPrefs.SetFloat("Voice Volume", VoiceVolume);

        if (Fullscreeen)
            PlayerPrefs.SetInt("Fullscreen", 1);
        else
            PlayerPrefs.SetInt("Fullscreen", 0);
        PlayerPrefs.SetInt("Resolution", Resolution);
        PlayerPrefs.SetInt("Monitor", Monitor);
        PlayerPrefs.SetInt("Quality", Quality);
        PlayerPrefs.SetFloat("Brightness", Brightness);
        PlayerPrefs.Save();
    }
    /** <summary>Applies any relevant settings to the game application.</summary> */
    public static void SetProperties () {
        Screen.fullScreen = Fullscreeen;
        Screen.brightness = Brightness;
        //RenderSettings.ambientLight = new Color(Settings.Brightness, Settings.Brightness, Settings.Brightness);
    }
}
