using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour {

    [SerializeField] private Character character;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private StatusDisplayController statusDisplay;
    public Character Character {
        get { return character; }
        set {
            character = value;
            nameLabel.text = character.data.Name;
            //Subscribe to character events to continually update
            character.onStatChange += (string statName, ref int oldValue, ref int newValue) => {
                if (statName == "health") ChangeHealth(newValue);
            };

            healthSlider.maxValue = Character.data.MaxHP;
            ChangeHealth(Character.Health);
        }
    }
    public StatusDisplayController StatusDisplay { get { return statusDisplay; } }

    public void ChangeHealth(int currentHealth) {
        healthSlider.value = currentHealth;
    }
}
