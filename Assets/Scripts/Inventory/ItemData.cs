using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour,IPointerDownHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler {

	// Use this for initialization
    public ItemContainer Item;
    public int SlotIndex;


   
    private Vector2 _offset;
    private InventoryHandler _inv;
    private Tooltip _tooltip;
    void Start()
    {
        _inv = GameObject.Find("Inventory").GetComponent<InventoryHandler>();
        _tooltip = _inv.GetComponent<Tooltip>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        print("Item.Id"+ Item.Id);
        _offset = eventData.position - (Vector2) this.transform.position;
        this.transform.position = eventData.position - _offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        print("Item.Id2" + Item.Id);
        this.transform.SetParent(this.transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        this.transform.position = eventData.position - _offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        Transform originalParent = _inv.InvSlots[SlotIndex].transform;
        this.transform.SetParent(originalParent);
        originalParent.name = this.transform.name;
        this.transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        _tooltip.Activate(Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        _tooltip.Dectivate();
    }
}
