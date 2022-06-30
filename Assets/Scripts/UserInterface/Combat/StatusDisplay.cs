using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour {

    [SerializeField] private Image effectImage;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Enums.CardEffects effectType;
    public Enums.CardEffects Effect {  get { return effectType; } }

    public void SetEffect(Enums.CardEffects effect, int count) {
        switch (effect) {
            case Enums.CardEffects.Advance:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Advance");
                effectType = Enums.CardEffects.Advance;
                break;
            case Enums.CardEffects.Armored:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Armored");
                effectType = Enums.CardEffects.Armored;
                break;
            case Enums.CardEffects.Bleed:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Bleed");
                effectType = Enums.CardEffects.Bleed;
                break;
            case Enums.CardEffects.Blind:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Blinded");
                effectType = Enums.CardEffects.Blind;
                break;
            case Enums.CardEffects.Bolster:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Bolstered");
                effectType = Enums.CardEffects.Bolster;
                break;
            case Enums.CardEffects.Burn:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Burn");
                effectType = Enums.CardEffects.Burn;
                break;
            case Enums.CardEffects.Cripple:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Crippled");
                effectType = Enums.CardEffects.Cripple;
                break;
            case Enums.CardEffects.Curse:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Cursed");
                effectType = Enums.CardEffects.Curse;
                break;
            case Enums.CardEffects.Fortify:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Fortified");
                effectType = Enums.CardEffects.Fortify;
                break;
            case Enums.CardEffects.Haste:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Hasted");
                effectType = Enums.CardEffects.Haste;
                break;
            case Enums.CardEffects.Poison:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Poisoned");
                effectType = Enums.CardEffects.Poison;
                break;
            case Enums.CardEffects.Rejuvenate:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Rejuvenating");
                effectType = Enums.CardEffects.Rejuvenate;
                break;
            case Enums.CardEffects.Shroud:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Shrouded");
                effectType = Enums.CardEffects.Shroud;
                break;
            case Enums.CardEffects.Silence:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Silenced");
                effectType = Enums.CardEffects.Silence;
                break;
            case Enums.CardEffects.Sleep:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Sleeping");
                effectType = Enums.CardEffects.Sleep;
                break;
            case Enums.CardEffects.Slow:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Slowed");
                effectType = Enums.CardEffects.Slow;
                break;
            case Enums.CardEffects.Stun:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Stunned");
                effectType = Enums.CardEffects.Stun;
                break;
            case Enums.CardEffects.Taunt:
                effectImage.sprite = Resources.Load<Sprite>("UserInterface/Status Icons/Taunt");
                effectType = Enums.CardEffects.Taunt;
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
        countText.text = Mathf.Abs(count).ToString();
    }
}
