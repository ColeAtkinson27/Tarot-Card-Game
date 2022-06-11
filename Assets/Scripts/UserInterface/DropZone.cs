using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Draggable.DragHandler onDrop;
    public bool hovering = false;

    public void OnPointerEnter(PointerEventData data){
        hovering = true;
    }
    public void OnPointerExit(PointerEventData data){
        hovering = false;
    }

    public void Update(){
        if(hovering && Draggable.dragTarget != null && Input.GetMouseButtonUp(0)){
            Debug.Log($"<color=purple>DropZone.cs</color>: {Draggable.dragTarget.name} dropped in {name}");
            Draggable.dragTarget.Drop(this);
            
        }
    }
}
