using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotDelete : MonoBehaviour, IDropHandler
{
    
    private static ModalPanel _modalPanel;

    void Start()
    {
        _modalPanel = ModalPanel.Instance();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        ItemData draggedItem = eventData.pointerDrag.GetComponent<ItemData>();

        if ( draggedItem == null)
            return;
        _modalPanel.Choice("Are you sure you want to delete this Item", ModalPanel.ModalPanelType.YesCancel, () => { draggedItem.Item.setStackCnt(0); });
    }
}
