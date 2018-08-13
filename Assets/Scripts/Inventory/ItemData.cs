﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemData : MonoBehaviour,IPointerDownHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{

	// Use this for initialization
    public ItemContainer Item;
    public int SlotIndex;


   
    private Vector2 _offset;
    private InventoryHandler _inv;
    private Tooltip _tooltip;

    public Sprite EmptySprite;

    void Start()
    {
        _inv = InventoryHandler.Instance();
        _tooltip = Tooltip.Instance();
    }

    void Update()
    {
        if (Item.Id == -1)
            return;
        if (Item.StackCnt == 0)
        {
            //Logic of adding empty Item to Slot 
            Item = new ItemContainer();
            this.transform.name = "Empty";
            this.GetComponent<Image>().sprite = EmptySprite;
            //Update Text
            this.transform.GetChild(0).GetComponent<Text>().text = "";
            _inv.InvSlots[SlotIndex].transform.name = this.transform.name;
            _inv.UpdateInventory(true);
        }
        else
        {
            Text stackCntText = this.transform.GetChild(0).GetComponent<Text>();
            if (Item.StackCnt.ToString() != stackCntText.text)
                stackCntText.text = Item.StackCnt > 1 ? Item.StackCnt.ToString() : "";
        }
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

    public void OnPointerDown(PointerEventData eventData)
    {
        //Todo: you can move the sprite by cliclking cpouple time fix it 
        if (Item.Id == -1)
            return;
        _offset = eventData.position - (Vector2) this.transform.position;
        this.transform.position = eventData.position - _offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
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
        Transform originalParent = _inv.InvSlots[SlotIndex].transform;
        this.transform.SetParent(originalParent);
        originalParent.name = this.transform.name;
        this.transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void LoadItem(ItemContainer item)
    {
        //print("#######Inside loadItem Itemdata: "+ item.Id);
        if (item.Id == -1)
        {
            this.Item = new ItemContainer();
            GetComponent<Image>().sprite = EmptySprite;
            Text stackCntText = this.transform.GetChild(0).GetComponent<Text>();
            stackCntText.text = "";
            _inv.UpdateInventory(true);
        }
        else
        {
            Item = new ItemContainer(item);
            Item.Print();
            this.transform.name = Item.Name;
            GetComponent<Image>().sprite = Item.GetSprite();
            this.transform.GetChild(0).GetComponent<Text>().text = Item.StackCnt > 1 ? Item.StackCnt.ToString() : "";
            print(Item.Name);
            _inv.InvSlots[SlotIndex].transform.name = Item.Name;
        }
    }

}
