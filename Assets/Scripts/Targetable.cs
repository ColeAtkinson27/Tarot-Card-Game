using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Character))]
public class Targetable : MonoBehaviour, IPointerClickHandler
{
    private static bool targetting = false;
    private static Enums.TargetType targetType;
    public static List<ITargetable> currentTargets = new List<ITargetable>();
    public static Character targetSource;

    [Tooltip("List of target types that apply to this gameobject")]
    public List<Enums.TargetType> targetTypes;
    public ITargetable target; //Assume characters are the only targetable entities

    public void Start() {
        target = GetComponent<Character>();
    }

    public void OnPointerClick(PointerEventData data) { //Camera needs to have the PhysicsRaycast Component
        if(targetting && targetTypes.Contains(targetType) && ((Character)target).Defeated == false) {
            currentTargets.Add(((Character)target));
            Debug.Log($"<color=Cyan>Target assigned:</color> {name} has been targeted");
            highlightTargets();
        }
    }

    public static void highlightTargets() {
        
        foreach (Character c in GameManager.Instance.Party()) {
            c.GetComponent<Targetable>().highlightTarget(c);
        }

        foreach (Character c in GameManager.Instance.Foes()) {
            c.GetComponent<Targetable>().highlightTarget(c);
        }
    }

    public void highlightTarget(Character character) {
        if (targetting && targetTypes.Contains(targetType) && !currentTargets.Contains(character))
            character.toggleHighlight();
        else
            character.toggleHighlight(false);
    }

    public static IEnumerator GetTargetable(Enums.TargetType type, string msg, int count = 1) {
        //send msg to some Text object in the screen to inform the player what they are targetting
        CombatUIManager.Instance.SetMessage(msg);

        currentTargets = new List<ITargetable>();
        targetting = true;
        targetType = type;
        highlightTargets();

        //loop while target is being found based on 'targetting'. The onpointerclick function is utilized while this keeps the function from ending \
        // Checks each frame if the number of targets is returned.
        while (currentTargets.Count < count) {
            yield return new WaitForEndOfFrame();
        }
        targetting = false;
        highlightTargets();
        CombatUIManager.Instance.HideMessage();
    }
}

