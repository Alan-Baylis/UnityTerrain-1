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



    public List<ItemContainer> Inv = new List<ItemContainer>();


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
        //for (int i = 0; i < _slotsX * _slotsY; i++) Inv.Add(new ItemContainer());  
        InventoryManager.Instance.InitInventory(Inv,5, _slotsX * _slotsY);
    }


    void Update()
    {
        //if (Input.GetButtonDown("Inventory"))
        if (Input.GetKeyDown(KeyCode.I))
        {
            _showInventory = !_showInventory;
            if (_dragging)
            {
                PutItemBack();

            }

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
            if (GUI.Button(new Rect(300, 50, 100, 40), "Save"))
            {
                print("SaveInventory");
                InventoryManager.Instance.SaveInventory(Inv, true);
            }
            if (GUI.Button(new Rect(300, 100, 100, 40), "Load"))
            {
                print("LoadInventory");
                InventoryManager.Instance.LoadInventory(Inv, true);
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
                if (Inv[invIndex].Name != null)
                {
                    //Draw the item sprite in the slot 
                    InventoryManager.Instance.DrawSprite(slotRect, Inv[invIndex]);
                    //Draw mouse hover
                    if (slotRect.Contains(currentEvent.mousePosition))
                    {
                        _showTooltip = true;
                        _tooltip = Inv[invIndex].GetTooltip();
                        if (currentEvent.button == 0 && currentEvent.type == EventType.MouseDrag && !_dragging) //Right click and drag
                        {
                            _dragging = true;
                            _draggedItem = Inv[invIndex];
                            _draggedIndex = invIndex;
                            Inv[invIndex] = new ItemContainer();
                        }
                        if (currentEvent.type == EventType.MouseUp && _dragging)
                        {
                            _dragging = false;
                            //Drag&Drop #1/3: on filled Item
                            //Same items Stack them together 
                            if (_draggedItem.Id == Inv[invIndex].Id)
                            {
                                //Todo: if higher than MaxStackCnt don't let them do it 
                                Inv[invIndex].StackCnt += _draggedItem.StackCnt;
                                if (Inv[invIndex].StackCnt > Inv[invIndex].MaxStackCnt)
                                {
                                    _draggedItem.StackCnt = Inv[invIndex].StackCnt - Inv[invIndex].MaxStackCnt;
                                    Inv[invIndex].StackCnt = Inv[invIndex].MaxStackCnt;
                                    PutItemBack();
                                }
                            }
                            //not Same items swap them
                            else
                            {
                                if (InventoryManager.Instance.MixAbleItems())
                                {
                                    print("MixAbleItems");
                                }
                                else
                                {
                                    Inv[_draggedIndex] = Inv[invIndex];
                                    Inv[invIndex] = _draggedItem;
                                }
                            }
                        }
                        //Right clicked 
                        if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 1)
                        {
                            if (Inv[invIndex].Type == Item.ItemType.Consumable)
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
                        Inv[_draggedIndex] = _draggedItem;
                    else
                    {
                        Inv[_draggedIndex] = Inv[invIndex];
                        Inv[invIndex] = _draggedItem;
                    }
                    _dragging = false;
                }
            }
        }
    }


    private void PutItemBack()
    {
        //Drag&Drop #3/3: on clear inventory
        Inv[_draggedIndex] = _draggedItem;
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
