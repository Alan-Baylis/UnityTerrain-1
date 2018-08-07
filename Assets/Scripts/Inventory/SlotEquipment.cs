using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotEquipment : MonoBehaviour, IDropHandler
{
    public Equipment.PlaceType EquType;
    // Use this for initialization
    private InventoryHandler _inv;

    void Start()
    {
        _inv = InventoryHandler.Instance();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData draggedItem = eventData.pointerDrag.GetComponent<ItemData>();
        
        if (draggedItem == null)
            return;
        if (draggedItem.Item.Id == -1)
            return;

        //Wearing Equipment
        if (EquType != Equipment.PlaceType.None)
        {
            if (draggedItem.Item.Type == Item.ItemType.Equipment)
                if (draggedItem.Item.PlaceHolder == EquType)
                    if (draggedItem.Item.StackCnt == draggedItem.Item.MaxStackCnt )
                    {

                        ItemEquipment existingEquipment = this.transform.GetComponentInChildren<ItemEquipment>();
                        ItemContainer itemEquipment = existingEquipment.Item;
                        //Todo: remove the effect of item
                        existingEquipment.LoadItem(draggedItem.Item);
                        draggedItem.LoadItem(itemEquipment);
                        _inv.UpdateInventory(true);
                    }
                    else
                        _inv.PrintMessage("YEL: you need "+ (draggedItem.Item.MaxStackCnt- draggedItem.Item.StackCnt) + " of this item to equipe");
                else
                    _inv.PrintMessage("YEL: you cannot equip this item here");
            else if (draggedItem.Item.Type == Item.ItemType.Weapon)
                if (EquType == Equipment.PlaceType.LeftHand || EquType == Equipment.PlaceType.RightHand)
                    if (draggedItem.Item.CarryType == Weapon.Hands.OneHand)
                    {
                        //Todo: Add logic of hands carry 
                        ItemEquipment existingEquipment = this.transform.GetComponentInChildren<ItemEquipment>();
                        ItemContainer itemEquipment = existingEquipment.Item;
                        //Todo: remove the effect of item
                        existingEquipment.LoadItem(draggedItem.Item);
                        draggedItem.LoadItem(itemEquipment);
                        _inv.UpdateInventory(true);
                    }
                    else 
                        _inv.PrintMessage("YEL: It is not possible to carry this weapon yet");
                else
                    _inv.PrintMessage("YEL: you cannot equip this item here");
            else if (draggedItem.Item.Type == Item.ItemType.Tool)
                if (EquType == Equipment.PlaceType.LeftHand || EquType == Equipment.PlaceType.RightHand )
                    {
                        //Todo: Add logic of hands carry 
                        ItemEquipment existingEquipment = this.transform.GetComponentInChildren<ItemEquipment>();
                        ItemContainer itemEquipment = existingEquipment.Item;
                        //Todo: remove the effect of item
                        existingEquipment.LoadItem(draggedItem.Item);
                        draggedItem.LoadItem(itemEquipment);
                        _inv.UpdateInventory(true);
                    }
                else
                    _inv.PrintMessage("YEL: you cannot equip this item here");
            else
                _inv.PrintMessage("YEL: This item can not be equiped");
        }


    }
}