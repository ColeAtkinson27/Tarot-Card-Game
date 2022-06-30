using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCharacter : Character
{
    public int position;
    bool cFirst25 = true, cFirst50 = true, cFirst75 = true, cFirstPlay = true;

    public override void Start() {
        base.Start();

        //if (data.color != null && highlight != null)
        //    highlight.GetComponent<ParticleSystem>().startColor = data.color;
    }

    //overriden to update character display, and to return cards to then hand if the player plays multiple
    public override Card CardToPlay
    {
        get
        {
            return _cardToPlay;
        }
        set {
            var newCard = value;
            //If the player plays a card while another card has yet to be played, return the first to the hand
            if (_cardToPlay != null && newCard != null){
                GameManager.Instance.PlaceCardInHand(CardToPlay);
            }
            _cardToPlay = newCard;
            //Update the Character's action
            if(_cardToPlay != null){
                Action = Enums.Action.Card;
            } else {
                Action = Enums.Action.Attack;
            }
        }
    }

    //Pull current characters basic attack (can create new one and save to the data object for specific chars)
    public IEnumerator BasicAttack(Damage damage = null){
        yield return Targetable.GetTargetable(Enums.TargetType.Foes, $"{data.name} attacks: Select a foe!", 1);
        Character target = (Character)Targetable.currentTargets[0];
        damage = damage == null ? data.basicAttack : damage;
        InvokeAttackHandler(target, ref damage);

        //animator.SetTrigger("Attack");
        //AudioManager.audioMgr.PlayCharacterSFX(SFX, "Attack");
        //Calculate atk/def bonuses

        int modifier = 0;
        if (status.ATKBonus != 0) {
            if (status.ATKBonus > 0) {
                modifier++;
                status.ATKBonus--;
            } else {
                modifier--;
                status.ATKBonus++;
            }
        }
        if (target.Status.DEFBonus != 0) {
            if (target.Status.DEFBonus > 0) {
                modifier--;
                target.Status.DEFBonus--;
            } else {
                modifier++;
                target.Status.DEFBonus++;
            }
        }
        int damVal = damage.Value;
        if (modifier == -2) damVal /= 4;
        else if (modifier == -1) damVal /= 2;
        else if (modifier == 1) damVal = (int)(damVal * 1.5f);
        else if (modifier == 2) damVal *= 2;

        target.Health -= damVal;
    }

    //Executes the character's turn, where they either play their card or attack
    public override IEnumerator GetTurn(){
        Debug.Log($"<color=orange>{data.name}'s turn</color>");
        InvokeTurnStartHandler();
        if(Defeated) {
            Debug.Log($"<Color=darkred>{data.name} has been defeated and cannot continue the fight</color>");
        }
        else if(Action == Enums.Action.Card && CardToPlay != null) {
            Debug.Log($"<color=green>{data.name} playing card {CardToPlay.Name}</color>");

            yield return CombatUIManager.Instance.RevealCard(CardToPlay);
            //Execute the selected card from the dropzone.
            yield return CardToPlay.Activate();
        } else if(Action == Enums.Action.Attack || Action == Enums.Action.Silenced) {
            yield return BasicAttack();
        }
        yield return ResolveEffects();
        InvokeTurnEndHandler();
        yield return new WaitForSeconds(1f);
    }

    public override void EndResolvePhase() {
        GameManager.Instance.Discard(CardToPlay);
        Action = Enums.Action.Attack;
        base.EndResolvePhase();
    }


}
