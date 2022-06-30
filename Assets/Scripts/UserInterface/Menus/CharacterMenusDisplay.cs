using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterMenusDisplay : MonoBehaviour {

    [SerializeField] private Image profile;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI affinityText;

    private CharacterData character;

    public void SetCharacter(CharacterData data) {
        character = data;

        profile.sprite = data.Avatar;
        nameText.text = data.Name;
        healthText.text = data.MaxHP + " hp";
        affinityText.text = data.PrimaryAffinity + "/" + data.SecondaryAffinity;
    }
}
