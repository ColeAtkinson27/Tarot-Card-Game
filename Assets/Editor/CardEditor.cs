using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor {
    Card card;
    SerializedObject GetTarget;
    CardEffect effect;
    static bool displayBools;

    #region CARD DATA
    SerializedProperty CardName;
    SerializedProperty CardDescription;
    SerializedProperty CardCorruptionPassDescription;
    SerializedProperty CardCorruptionFailDescription;
    SerializedProperty CardFlavor;
    SerializedProperty CardAffinity;
    SerializedProperty CardAllyTargets;
    SerializedProperty CardEnemyTargets;
    SerializedProperty CardExile;
    SerializedProperty CardBossCard;
    SerializedProperty CardAnim;
    SerializedProperty CardIcons;
    SerializedProperty CardBackIcons;
    SerializedProperty CardEffects;
    SerializedProperty CardCorPass;
    SerializedProperty CardCorFail;
    SerializedProperty CardEffectBuffer;
    #endregion

    private void OnEnable () {
        card = (Card)target;
        GetTarget = new SerializedObject(card);

        #region CARD DATA
        CardName = GetTarget.FindProperty("cardName");
        CardDescription = GetTarget.FindProperty("cardDescription");
        CardCorruptionPassDescription = GetTarget.FindProperty("cardCorruptionPassDescription");
        CardCorruptionFailDescription = GetTarget.FindProperty("cardCorruptionFailDescription");
        CardFlavor = GetTarget.FindProperty("cardFlavor");
        CardAffinity = GetTarget.FindProperty("cardAffinity");
        CardAllyTargets = GetTarget.FindProperty("cardAllyTargets");
        CardEnemyTargets = GetTarget.FindProperty("cardEnemyTargets");
        CardExile = GetTarget.FindProperty("exiled");
        CardBossCard = GetTarget.FindProperty("bossCard");
        CardAnim = GetTarget.FindProperty("cardAnimation");
        CardIcons = GetTarget.FindProperty("cardIcons");
        CardBackIcons = GetTarget.FindProperty("cardBackIcons");
        CardEffects = GetTarget.FindProperty("cardEffects");
        CardCorPass = GetTarget.FindProperty("cardCorPass");
        CardCorFail = GetTarget.FindProperty("cardCorFail");
        #endregion
    }

    public override void OnInspectorGUI () {
        GetTarget.Update();
        
        using (new EditorGUILayout.VerticalScope("HelpBox")) {
            EditorGUILayout.LabelField("Card Information", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(CardName);
            EditorGUILayout.PropertyField(CardDescription);
            EditorGUILayout.PropertyField(CardCorruptionPassDescription);
            EditorGUILayout.PropertyField(CardCorruptionFailDescription);
            EditorGUILayout.PropertyField(CardFlavor);
            EditorGUILayout.LabelField("Card Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(CardAffinity);
            EditorGUILayout.PropertyField(CardAllyTargets);
            EditorGUILayout.PropertyField(CardEnemyTargets);
            displayBools = EditorGUILayout.Foldout(displayBools, "Extra Card Options");
            if (displayBools) {
                EditorGUILayout.PropertyField(CardExile);
                EditorGUILayout.PropertyField(CardBossCard);
            }
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Card Art", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(CardIcons);
            EditorGUILayout.PropertyField(CardBackIcons);
            EditorGUILayout.PropertyField(CardAnim);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Card Effects", EditorStyles.boldLabel);
            InsertCardEffectFields(CardEffects, 0);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Corruption Effects", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Pass");
            InsertCardEffectFields(CardCorPass, 1);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Fail");
            InsertCardEffectFields(CardCorFail, 2);
        }

        GetTarget.ApplyModifiedProperties();
    }

    private void InsertCardEffectFields(SerializedProperty effectsList, int listNo) {
        //Go through each effect in the list
        for (int i = 0; i < effectsList.arraySize; i++) {
            effect = card.GetEffect(i, listNo);
            //Add the effect dropdown select, and remove button
            GUILayout.BeginHorizontal();
            effect.EffectType = (Enums.CardEffects)EditorGUILayout.EnumPopup("Card Effect", effect.EffectType);
            if (i != 0) {
                if (GUILayout.Button("Up", EditorStyles.miniButtonLeft, GUILayout.Width(30f))) {
                    //SerializedProperty buffer = effectsList.GetArrayElementAtIndex(i - 1);
                    effectsList.MoveArrayElement(i, i - 1);
                    continue;
                }
            }
            if (i != effectsList.arraySize - 1) {
                if (GUILayout.Button("Down", EditorStyles.miniButtonLeft, GUILayout.Width(40f))) {
                    effectsList.MoveArrayElement(i, i + 1);
                    continue;
                }
            }
            if (GUILayout.Button("Remove", EditorStyles.miniButtonLeft, GUILayout.Width(60f))) {
                effectsList.DeleteArrayElementAtIndex(i);
                continue;
            }
            GUILayout.EndHorizontal();

            #region CARD DATA
            CardEffectBuffer = effectsList.GetArrayElementAtIndex(i);
            #endregion

            #region EFFECT TYPES
            SerializedProperty value1 = CardEffectBuffer.FindPropertyRelative("value1");
            SerializedProperty value2 = CardEffectBuffer.FindPropertyRelative("value2");
            SerializedProperty value3 = CardEffectBuffer.FindPropertyRelative("value3");
            SerializedProperty stringValue = CardEffectBuffer.FindPropertyRelative("stringValue");
            SerializedProperty targetType = CardEffectBuffer.FindPropertyRelative("targetType");
            SerializedProperty numTargets = CardEffectBuffer.FindPropertyRelative("numTargets");
            #endregion

            //Draw input fields based on chosen effect
            switch (effect.EffectType) {
                case Enums.CardEffects.Advance:
                    break;
                case Enums.CardEffects.Armored:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Shields"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Bleed:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Damage"));
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Blind:
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Bolster:
                    if (listNo == 0) AddStandardFields(targetType);
                    EditorGUILayout.PropertyField(value1, new GUIContent("Applications"));
                    break;
                case Enums.CardEffects.Burn:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Damage"));
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Cleanse:
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Cold:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Num of Die"));
                    EditorGUILayout.PropertyField(value2, new GUIContent("Num of Sides"));
                    EditorGUILayout.PropertyField(value3, new GUIContent("Bonus Damage"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Corrupt:
                    EditorGUILayout.PropertyField(value3, new GUIContent("Value"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Cripple:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Applications"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Curse:
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Discard:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Cards"));
                    break;
                case Enums.CardEffects.Draw:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Cards"));
                    break;
                case Enums.CardEffects.Fire:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Num of Die"));
                    EditorGUILayout.PropertyField(value2, new GUIContent("Num of Sides"));
                    EditorGUILayout.PropertyField(value3, new GUIContent("Bonus Damage"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Fortify:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Applications"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Haste:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Applications"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Heal:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Num of Die"));
                    EditorGUILayout.PropertyField(value2, new GUIContent("Num of Sides"));
                    EditorGUILayout.PropertyField(value3, new GUIContent("Bonus Healing"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Insert:
                    break;
                case Enums.CardEffects.Lightning:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Num of Die"));
                    EditorGUILayout.PropertyField(value2, new GUIContent("Num of Sides"));
                    EditorGUILayout.PropertyField(value3, new GUIContent("Bonus Damage"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Mark:
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Nature:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Num of Die"));
                    EditorGUILayout.PropertyField(value2, new GUIContent("Num of Sides"));
                    EditorGUILayout.PropertyField(value3, new GUIContent("Bonus Damage"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Nullify:
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Physical:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Num of Die"));
                    EditorGUILayout.PropertyField(value2, new GUIContent("Num of Sides"));
                    EditorGUILayout.PropertyField(value3, new GUIContent("Bonus Damage"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Poison:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Damage"));
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Protect:
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Rejuvenate:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Value"));
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Reshuffle:
                    break;
                case Enums.CardEffects.Revive:
                    EditorGUILayout.PropertyField(value3, new GUIContent("Health %"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Shroud:
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Silence:
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Sleep:
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Slow:
                    break;
                case Enums.CardEffects.Soothe:
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Stun:
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Summon:
                    break;
                case Enums.CardEffects.Taunt:
                    EditorGUILayout.PropertyField(value2, new GUIContent("Turns"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
                case Enums.CardEffects.Weaken:
                    EditorGUILayout.PropertyField(value1, new GUIContent("Applications"));
                    if (listNo == 0) AddStandardFields(targetType);
                    break;
            }

            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add Effect")) {
            card.AddCardEffectMaker(listNo);
        }
    }

    private void AddStandardFields(SerializedProperty targetType) {
        EditorGUILayout.PropertyField(targetType);
    }
}
