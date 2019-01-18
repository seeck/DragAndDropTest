using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {

    
    [HideInInspector]
    public Transform parentToReturnTo;
    [HideInInspector]
    public Transform placeHolderParent;
    [HideInInspector]
    public int newSiblingIndex;
    [HideInInspector]
    public Transform originalParent;
    [HideInInspector]
    public int originalSiblingIndex;
    
    private GameObject placeHolder;

    //Added event to detect sharing of song with contact.
    public Action<Draggable, Draggable> OnDraggableDropped;

    public void OnBeginDrag(PointerEventData eventData) {
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        placeHolder = new GameObject();
        placeHolder.transform.SetParent(transform.parent);
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeHolder.transform.SetSiblingIndex(transform.GetSiblingIndex() );

        parentToReturnTo = transform.parent;
        placeHolderParent = parentToReturnTo;
        transform.SetParent(transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;

        if (placeHolder.transform.parent != placeHolderParent)
        {
            placeHolder.transform.SetParent(placeHolderParent);
        }
        
        int newSiblingIndex = placeHolderParent.childCount;

		//reorder items in the same list
        for (int i = 0; i < placeHolderParent.childCount; i++)
        {
            if (transform.position.y > placeHolderParent.GetChild(i).position.y)
            {
                newSiblingIndex = i;

                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;                
                break;
            }
        }

        placeHolder.transform.SetSiblingIndex(newSiblingIndex);
        this.newSiblingIndex = newSiblingIndex;
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentToReturnTo);
        transform.SetSiblingIndex(newSiblingIndex);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeHolder);
    }

    //Added to check for song dropped on contact.
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag!=null)
        {
            Draggable d1 = eventData.pointerDrag.GetComponent<Draggable>();
            OnDraggableDropped.SafeInvoke(d1, this);
        }

    }
}
