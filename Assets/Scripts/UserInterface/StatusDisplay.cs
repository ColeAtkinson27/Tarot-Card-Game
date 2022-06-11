using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour {

    [SerializeField] private Image effectImage;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI countText;
    private Enums.CardEffects effectType;
    public Enums.CardEffects Effect {  get { return effectType; } }

    public void SetEffect(Enums.CardEffects effect, int count) {
        switch (effect) {
            case Enums.CardEffects.Armored:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Armored");
                effectType = Enums.CardEffects.Armored;
                break;
            case Enums.CardEffects.Bolster:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Bolstered");
                effectType = Enums.CardEffects.Bolster;
                break;
            case Enums.CardEffects.Burn:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Burn");
                effectType = Enums.CardEffects.Burn;
                break;
            case Enums.CardEffects.Rejuvenate:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Rejuvenating");
                effectType = Enums.CardEffects.Rejuvenate;
                break;
            case Enums.CardEffects.Shroud:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Shrouded");
                effectType = Enums.CardEffects.Shroud;
                break;
            case Enums.CardEffects.Stun:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Stunned");
                effectType = Enums.CardEffects.Stun;
                break;
            case Enums.CardEffects.Weaken:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Weakened");
                effectType = Enums.CardEffects.Weaken;
                break;
        }
        countText.text = count.ToString();
    }
    public void SetEffect(Enums.CardEffects effect, int count, int damage) {
        SetEffect(effect, count);
        damageText.text = Mathf.Abs(damage).ToString();
    }

    public void UpdateEffect(int count) {
        if (count <= 0)
            Destroy(gameObject);
        countText.text = count.ToString();
    }
}
