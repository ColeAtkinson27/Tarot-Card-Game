using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Flags {
    private static List<KeyVal<string, bool>> booleans;
    private static List<KeyVal<string, int>> integers;

    public static void Initialize() {
        booleans = new List<KeyVal<string, bool>>();
        integers = new List<KeyVal<string, int>>();
    }

    /**
     * <summary>Checks the chosen boolean flag as true or false.</summary>
     * <param name="name">The name of the flag to check.</param>
     * <returns>The value of the chosen flag. Returns False if flag does not exist.</returns>
     */
    public static bool CheckBoolFlag (string name) {
        try { 
            for (int i = 0; i < booleans.Count; i++) {
                if (booleans[i].ID.Equals(name))
                    return booleans[i].Value;
            }
            return false;
        } catch (System.Exception e) {
            Debug.Log("Flags boolean error:" + e.ToString() + "\n" + e.StackTrace);
            return false;
        }
    }

    /**
     * <summary>Checks the chosen integer flag for a value.</summary>
     * <param name="name">The name of the flag to check.</param>
     * <returns>The value of the chosen flag. Returns 0 if flag does not exist.</returns>
     */
    public static int CheckIntFlag (string name) {
        try {
            for (int i = 0; i < integers.Count; i++) {
                if (integers[i].ID.Equals(name))
                    return integers[i].Value;
            }
            return 0;
        } catch (System.Exception e) {
            Debug.Log("Flags integers error:" + e.ToString() + "\n" + e.StackTrace);
            return 0;
        }
    }

    /**
     * <summary>Creates a new boolean flag to track, setting the default state to False.</summary>
     * <param name="name">The name of the new flag.</param>
     */
    public static void AddBoolFlag (string name) {
        for (int i = 0; i < booleans.Count; i++)
            if (booleans[i].ID.Equals(name))
                return;
        booleans.Add(new KeyVal<string, bool>(name, false));
        Debug.Log("Added new bool flag: " + name);
    }

    /**
     * <summary>Creates a new boolean flag to track.</summary>
     * <param name="name">The name of the new flag.</param>
     * <param name="value">The value of the new flag.</param>
     */
    public static void AddBoolFlag (string name, bool value) {
        for (int i = 0; i < booleans.Count; i++)
            if (booleans[i].ID.Equals(name)) {
                return;
            }
        booleans.Add(new KeyVal<string, bool>(name, value));
        Debug.Log("Added new bool flag: " + name);
    }

    /**
     * <summary>Creates a new integer flag to track, setting the default state to 0.</summary>
     * <param name="name">The name of the new flag.</param>
     */
    public static void AddIntFlag (string name) {
        for (int i = 0; i < integers.Count; i++)
            if (integers[i].ID.Equals(name))
                return;
        integers.Add(new KeyVal<string, int>(name, 0));
        Debug.Log("Added new int flag: " + name);
    }

    /**
     * <summary>Creates a new integer flag to track.</summary>
     * <param name="name">The name of the new flag.</param>
     * <param name="value">The value of the new flag.</param>
     */
    public static void AddIntFlag (string name, int value) {
        for (int i = 0; i < integers.Count; i++)
            if (integers[i].ID.Equals(name)) {
                return;
            }
        integers.Add(new KeyVal<string, int>(name, value));
        Debug.Log("Added new int flag: " + name);
    }

    /**
     * <summary>Updates an existing boolean flag. If flag does not exist, create a new one.</summary>
     * <param name="name">The name of the flag.</param>
     * <param name="value">The value to update the flag.</param>
     */
    public static void UpdateBoolFlag (string name, bool value) {
        for (int i = 0; i < booleans.Count; i++) {
            if (booleans[i].ID.Equals(name)) {
                booleans[i].Value = value;
                Debug.Log("Updated bool flag: " + name + " value = " + value);
                return;
            }
        }
        AddBoolFlag(name, value);
        Debug.Log("Updated new bool flag: " + name + " value = " + value);
    }

    /**
     * <summary>Updates an existing integer flag. If flag does not exist, create a new one.</summary>
     * <param name="name">The name of the flag.</param>
     * <param name="value">The value to update the flag.</param>
     */
    public static void UpdateIntFlag (string name, int value) {
        for (int i = 0; i < integers.Count; i++) {
            if (integers[i].ID.Equals(name)) {
                integers[i].Value = value;
                Debug.Log("Updated int flag: " + name + " value = " + value);
                return;
            }
        }
        AddIntFlag(name, value);
        Debug.Log("Updated new int flag: " + name + " value = " + value);
    }

    /**
     * <summary>Adds a number to an existing integer flag. If flag does not exist, create a new one.</summary>
     * <param name="name">The name of the flag.</param>
     * <param name="value">The value to add to the flag.</param>
     */
    public static void UpdateAddIntFlag (string name, int value) {
        for (int i = 0; i < integers.Count; i++) {
            if (integers[i].ID.Equals(name)) {
                integers[i].Value += value;
                Debug.Log("Updated int flag: " + name + " value = " + value);
                return;
            }
        }
        AddIntFlag(name, value);
        Debug.Log("Updated new int flag: " + name + " value = " + value);
    }

    public class KeyVal<Key, Val> {
        public Key ID;
        public Val Value;
        public KeyVal () { }
        public KeyVal (Key key, Val val) {
            ID = key;
            Value = val;
        }
    }
}
