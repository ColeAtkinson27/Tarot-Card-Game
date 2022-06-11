using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusDisplayController : MonoBehaviour {
    [SerializeField] private Character character;
    [SerializeField] private RectTransform content;
    [SerializeField] private List<StatusDisplay> effects = new List<StatusDisplay>();

    public Character Character { get { return character; } set { character = value; } }

    public void UpdateEffects() {
        if (effects.Count > 0) {
            StatusDisplay buffer;
            while (effects.Count >= 1) {
                buffer = effects[0];
                effects.Remove(buffer);
                Destroy(buffer.gameObject);
            }
        }
        //ATK and DEF stats
        if (character.Status.ATKBonus > 0)
            CreateEffect(Enums.CardEffects.Bolster, character.Status.ATKBonus);
        else if (character.Status.ATKBonus < 0)
            CreateEffect(Enums.CardEffects.Cripple, character.Status.ATKBonus);
        if (character.Status.DEFBonus > 0)
            CreateEffect(Enums.CardEffects.Fortify, character.Status.DEFBonus);
        else if (character.Status.DEFBonus < 0)
            CreateEffect(Enums.CardEffects.Weaken, character.Status.DEFBonus);

        //Positive Effects
        if (character.Status.Armored > 0)
            CreateEffect(Enums.CardEffects.Armored, character.Status.Armored);
        if (character.Status.Shrouded > 0)
            CreateEffect(Enums.CardEffects.Shroud, character.Status.Shrouded);

        //Negative Effects
        if (character.Status.Stunned > 0)
            CreateEffect(Enums.CardEffects.Stun, character.Status.Stunned);

        //DoTs
        for (int i = 0; i < character.Status.Burns.Count; i++)
            CreateEffect(Enums.CardEffects.Burn, character.Status.Burns[i].Turns, character.Status.Burns[i].Damage);
        for (int i = 0; i < character.Status.Rejuvenations.Count; i++)
            CreateEffect(Enums.CardEffects.Rejuvenate, character.Status.Rejuvenations[i].Turns, character.Status.Rejuvenations[i].Damage);
    }

    public void CreateEffect(Enums.CardEffects effect, int count, int damage = 0) {
        StatusDisplay status = Instantiate(Resources.Load<StatusDisplay>("UserInterface/Status Display"));
        status.transform.SetParent(content);
        switch (effect) {
            case Enums.CardEffects.Armored:
                status.SetEffect(Enums.CardEffects.Armored, count);
                break;
            case Enums.CardEffects.Bolster:
                status.SetEffect(Enums.CardEffects.Bolster, count);
                break;
            case Enums.CardEffects.Burn:
                status.SetEffect(Enums.CardEffects.Burn, count, damage);
                break;
            case Enums.CardEffects.Rejuvenate:
                status.SetEffect(Enums.CardEffects.Rejuvenate, count, damage);
                break;
            case Enums.CardEffects.Shroud:
                status.SetEffect(Enums.CardEffects.Shroud, count);
                break;
            case Enums.CardEffects.Stun:
                status.SetEffect(Enums.CardEffects.Stun, count);
                break;
            case Enums.CardEffects.Weaken:
                status.SetEffect(Enums.CardEffects.Weaken, count);
                break;
        }
        effects.Add(status);
    }
}
