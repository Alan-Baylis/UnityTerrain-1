using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotData : MonoBehaviour,IDropHandler{

    public int SlotIndex;
    // Use this for initialization
    private InventoryHandler _inv;
    void Start()
    {
        _inv = GameObject.Find("Inventory").GetComponent<InventoryHandler>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        if (droppedItem.Item.Id == -1)
            return;
        ItemData[] existingItems = this.transform.GetComponentsInChildren<ItemData>();
        Debug.Log(droppedItem.SlotIndex+"-"+ SlotIndex);
        if (SlotIndex != droppedItem.SlotIndex )
        {
            Debug.Log(existingItems[0].Item.Id.ToString());
            existingItems[0].transform.SetParent(_inv.InvSlots[droppedItem.SlotIndex].transform);
            _inv.InvSlots[droppedItem.SlotIndex].name = existingItems[0].name;
            existingItems[0].transform.position = _inv.InvSlots[droppedItem.SlotIndex].transform.position;
            existingItems[0].SlotIndex = droppedItem.SlotIndex;
            droppedItem.SlotIndex = SlotIndex;
        }
    }
}
