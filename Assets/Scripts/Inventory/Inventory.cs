using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

    public int SlotsCount = 21;

    public int PlayerSlots = 7;
    public int SlotsPadding = 5;
    public int SlotSize = 50;
    public GUISkin Skin;
    public Vector2 InvLocation = Vector2.zero;
    public KeyCode KeyToShowInventory = KeyCode.I;
    public bool ShowInvButton = false;



    private List<ItemContainer> _inv = new List<ItemContainer>();

    private int _slotsX = 0;
    private int _slotsY = 0;
    private bool _showInventory = false;
    private bool _showTooltip = false;
    private string _tooltip = "";
    private bool _dragging = false;
    private ItemContainer _draggedItem;
    private int _draggedIndex;
    //private static GameObject _player;



    // Use this for initialization
    void Start()
    {
        //Slot x,y number calculations 
        _slotsX = 5;
        _slotsY = SlotsCount / _slotsX + 1;

        //Inv location
        InvLocation = new Vector2(10, 10);

        //_player = GameObject.Find(Player);

        //Added to InitInventory 
        InventoryManager.Instance.InitInventory(_inv, 5, _slotsX * _slotsY);
        //Print the inventory for debug 
        //foreach (var VARIABLE in _inv) VARIABLE.Print();
    }

    internal bool AddItemToInventory(ItemContainer item)
    {
        for (int i = 0; i < _inv.Count; i++)
        {
            //print("###Inside AddItemToInventory : "+ _inv.Count + "index"+ i + " id "+_inv[i].Id + _inv[i].Name); 
            if (i > PlayerSlots)
            {
                //Rect tooltipBox = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200, dynamicHeight);
                //GUI.Box(tooltipBox, _tooltip);
                print("Inventory is full");
                break;
            }
            if (_inv[i].Id == -1) //empty Slot
            {
                _inv[i] = new ItemContainer(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost,
                    item.Weight, item.MaxStackCnt, item.StackCnt, item.Type, item.Rarity,
                    DateTime.Now.Add(new TimeSpan(24, 0, 0, 0)), item.Values);
                //print("###inside AddItemToInventory " +i); 
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        //if (Input.GetButtonDown("Inventory"))
        if (Input.GetKeyDown(KeyToShowInventory))
        {
            _showInventory = !_showInventory;
            if (_dragging)
                PutItemBack();
        }

    }

    void OnGUI()
    {
        //for (int i = 0; i < Inv.Count; i++)
        //{
        //    GUI.Label(new Rect( 10,10*i,200,50),Inv[i].Name);
        //}

        _showTooltip = false;
        GUI.skin = Skin;

        // You can only call GUI ellements only in the OnGUI
        if (_showInventory)
        {
            if (ShowInvButton)
            {
                //Save Inventory to cache Button
                if (GUI.Button(new Rect(300, 50, 100, 40), "Save"))
                    InventoryManager.Instance.SaveInventory(_inv, true);
                //Load Inventory from cache Button
                if (GUI.Button(new Rect(300, 100, 100, 40), "Load"))
                    InventoryManager.Instance.LoadInventory(_inv, true);
            }

            //print(Slots.Count);
            DrawInventory();
            if (_showTooltip && !_dragging)
            {
                float dynamicHeight = Skin.box.CalcHeight(new GUIContent(_tooltip), 200);
                Rect tooltipBox = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200, dynamicHeight);
                GUI.Box(tooltipBox, _tooltip, Skin.GetStyle("box"));
            }
        }
        if (_dragging)
        {
            //10% bigger than normal
            Rect dragBox = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, width: SlotSize * 11 / 10, height: SlotSize * 11 / 10);
            InventoryManager.Instance.DrawSprite(dragBox, _draggedItem);
        }
        if (Event.current.type == EventType.MouseUp && _dragging)
        {
            PutItemBack();
        }
    }


    void DrawInventory()
    {
        Event currentEvent = Event.current;

        Rect invRect = new Rect(InvLocation.x - SlotsPadding, InvLocation.y - SlotsPadding, (SlotSize + SlotsPadding) * _slotsX + SlotsPadding, (SlotSize + SlotsPadding) * _slotsY + SlotsPadding);

        //print(invRect.ToString());

        GUI.Box(invRect, "", Skin.GetStyle("slotNormal"));

        for (int y = 0; y < _slotsY; y++)
        {
            for (int x = 0; x < _slotsX; x++)
            {
                int invIndex = x + y * _slotsX;
                if (invIndex > SlotsCount)
                    break;
                Rect slotRect = new Rect(x * (SlotSize + SlotsPadding) + InvLocation.x, y * (SlotSize + SlotsPadding) + InvLocation.y, SlotSize, SlotSize);
                GUI.Box(
                        slotRect, 
                        "",
                        invIndex > PlayerSlots ? 
                                  Skin.GetStyle("slotBroken") 
                                : Skin.GetStyle("slotDamaged")
                    );
                if (_inv[invIndex].Id != -1)
                {
                    //Draw the item sprite in the slot 
                    InventoryManager.Instance.DrawSprite(slotRect, _inv[invIndex]);
                    //Draw mouse hover
                    if (slotRect.Contains(currentEvent.mousePosition))
                    {
                        _showTooltip = true;
                        _tooltip = _inv[invIndex].GetTooltip();
                        //todo: Garbage copllections review all the logics 1/2
                        if (currentEvent.button == 0 && currentEvent.type == EventType.MouseDrag && !_dragging) //Right click and drag
                        {
                            _dragging = true;
                            _draggedItem = _inv[invIndex];
                            _draggedIndex = invIndex;
                            _inv[invIndex] = new ItemContainer();
                        }
                        if (currentEvent.type == EventType.MouseUp && _dragging)
                        {
                            _dragging = false;
                            //Drag&Drop #1/3: on filled Item
                            //Same items Stack them together 
                            if ( _inv[invIndex].Id == _draggedItem.Id)
                            {   
                                if (_inv[invIndex].StackCnt + _draggedItem.StackCnt > _inv[invIndex].MaxStackCnt)
                                {
                                    _draggedItem.setStackCnt(_draggedItem.StackCnt- (_inv[invIndex].MaxStackCnt - _inv[invIndex].StackCnt));
                                    _inv[invIndex].setStackCnt(_inv[invIndex].MaxStackCnt);
                                }
                                else
                                {
                                    _inv[invIndex].setStackCnt(_inv[invIndex].StackCnt + _draggedItem.StackCnt);
                                    _draggedItem.setStackCnt(0);
                                }
                                PutItemBack();
                            }
                            else //not Same items Mix or swap them
                            {
                                Recipe newRecipe = InventoryManager.Instance.CheckRecipes( _inv[invIndex].Id,_draggedItem.Id);
                                if (newRecipe!=null)
                                {
                                    //todo: Garbage copllections review all the logics 2/2
                                    if (newRecipe.FirstItemCnt <= _inv[invIndex].StackCnt)
                                        if (newRecipe.FirstItemCnt <= _draggedItem.StackCnt)
                                        {   //Mixing items Logic
                                            _inv[invIndex].setStackCnt(_inv[invIndex].StackCnt - newRecipe.FirstItemCnt);
                                            _draggedItem.setStackCnt(_draggedItem.StackCnt - newRecipe.SecondItemCnt);
                                            ItemContainer item = InventoryManager.Instance.GetItemFromDatabase(newRecipe.FinalItemId);
                                            if (_inv[invIndex].StackCnt == 0 )
                                            {
                                                
                                                _inv[invIndex] = new ItemContainer(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost,
                                                    item.Weight, item.MaxStackCnt, Math.Min(newRecipe.FinalItemCnt, item.MaxStackCnt), item.Type, item.Rarity,
                                                    DateTime.Now.Add(new TimeSpan(24, 0, 0, 0)), item.Values);
                                                print("add new item to _inv[invIndex] ");
                                                PutItemBack();
                                            }
                                            else if (_draggedItem.StackCnt == 0)
                                            {
                                                _draggedItem = new ItemContainer(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost,
                                                    item.Weight, item.MaxStackCnt, Math.Min(newRecipe.FinalItemCnt, item.MaxStackCnt), item.Type, item.Rarity,
                                                    DateTime.Now.Add(new TimeSpan(24, 0, 0, 0)), item.Values);
                                                print("add new item to _draggedItem ");
                                            }
                                            else
                                            {
                                                //We need to put back the _draggedItem so we don't put the new item in that place
                                                PutItemBack();
                                                ItemContainer newItem = new ItemContainer(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost,
                                                    item.Weight, item.MaxStackCnt, Math.Min(newRecipe.FinalItemCnt, item.MaxStackCnt), item.Type, item.Rarity,
                                                    DateTime.Now.Add(new TimeSpan(24, 0, 0, 0)), item.Values);
                                                if (!AddItemToInventory(newItem))
                                                {   //Reverce back the changes
                                                    print("Not enough space in your inventory"); //todo: remove this because AddItemToInventory already have that print 
                                                    _inv[invIndex].setStackCnt(_inv[invIndex].StackCnt + newRecipe.FirstItemCnt);
                                                    _draggedItem.setStackCnt(_draggedItem.StackCnt + newRecipe.SecondItemCnt);
                                                }
                                            }
                                        }
                                        else //Not enough materials 
                                            print("Not enough " + _draggedItem.Name + " in the inventory, You need " + (newRecipe.FirstItemCnt - _draggedItem.StackCnt) + " more");
                                    else //Not enough materials 
                                        print("Not enough "+ _inv[invIndex].Name + " in the inventory, You need "+ (newRecipe.FirstItemCnt-_inv[invIndex].StackCnt) +" more");
                                    PutItemBack();
                                }
                                else //Swaping items Logic
                                {
                                    _inv[_draggedIndex] = _inv[invIndex];
                                    _inv[invIndex] = _draggedItem;
                                }
                            }
                        }
                        //Right clicked 
                        if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 1)
                        {
                            if (_inv[invIndex].Type == Item.ItemType.Consumable)
                            {
                                //Use Consumable
                                //UseConsumable(Inv[invIndex], invIndex, true);
                            }
                        }
                    }
                }
                else if (slotRect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseUp && _dragging)
                {
                    //Drag&Drop #2/3: on empty slot
                    if (invIndex > PlayerSlots)
                        _inv[_draggedIndex] = _draggedItem;
                    else
                    {
                        _inv[_draggedIndex] = _inv[invIndex];
                        _inv[invIndex] = _draggedItem;
                    }
                    _dragging = false;
                }
            }
        }
    }


    private void PutItemBack()
    {
        //Drag&Drop #3/3: on clear inventory
        if (_draggedItem.StackCnt == 0 )
            _inv[_draggedIndex] = new ItemContainer();
        else
            _inv[_draggedIndex] = _draggedItem;
        _dragging = false;
    }

    //private void UseConsumable(ItemContainer item, int invIndex, bool deleteItem)
    //{
    //    if (InventoryManager.Instance.CheckItemInInventory(item.Id,Inv))
    //    {
    //        switch (item.Id)
    //        {
    //            case 0:
    //                //do something;
    //                print("Consumable " + invIndex + " " + item.Name);
    //                break;
    //            case 2:
    //                //do something;
    //                print("Consumable " + invIndex + " " + item.Name);
    //                break;
    //            default:
    //                print(invIndex + " Not a Consumable ");
    //                break;
    //        }
    //        if (deleteItem)
    //            if (!InventoryManager.Instance.RemoveItemFromInventory(item.Id, Inv))
    //                Debug.Log("Remove Item From Inventory Failed");
    //    }
    //    else
    //    {
    //        print("Not in inventory");
    //    }
    //}







}
