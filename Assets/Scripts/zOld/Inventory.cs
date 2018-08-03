using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryOld : MonoBehaviour {


    //public GUISkin Skin;

    //private static CharacterDatabase _characterDb;
    //private static TerrainActions _terrainActions;
    //private static GUIManager _GUIManager;
    //private List<ItemContainer> _inv = new List<ItemContainer>();
    
    //public KeyCode _keyToShowInventory = KeyCode.I;
    //private Rect _invRect;
    ////Inv location
    //private Vector2 _invLocation = new Vector2(300, 300);
    //private int _slotsPadding = 5;
    //private int _slotSize = 50;
    //private int _slotsX = 0;
    //private int _slotsY = 0;
    //private bool _showInvButton = false;
    //private bool _showInventory = false;
    //private bool _updateInventory = false;
    //private bool _showTooltip = false;
    //private string _tooltip = "";
    //private bool _dragging = false;
    //private ItemContainer _draggedItem;
    //private int _draggedIndex;
    
    //private int MaxSlotsCount = 21;
    //private int _playerSlots = 0;



    //// Use this for initialization
    //void Start()
    //{
    //    //Slot x,y number calculations 
    //    _slotsX = 5;
    //    _slotsY = MaxSlotsCount / _slotsX + 1;


    //    _invRect = BuildInventoryBox();


    //    //print(invRect.ToString());


    //    _terrainActions = GameObject.FindGameObjectWithTag("Player").GetComponent<TerrainActions>();
    //    _GUIManager = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<GUIManager>();

    //    //Added to InitInventory 
    //    InventoryManager.Instance.InitInventory(_inv, 5, _slotsX * _slotsY);
    //    //Print the inventory for debug 
    //    //foreach (var VARIABLE in _inv) VARIABLE.Print();

    //    _characterDb = GameObject.FindGameObjectWithTag("Character Database").GetComponent<CharacterDatabase>();
    //    CharacterSetting settings = _characterDb.PlayerSetting;
    //    //print("###insite Start inventory "+ settings.CarryCnt);
    //    _playerSlots = settings.CarryCnt;

    //    InventoryManager.Instance.InitInventory(_inv, _characterDb.PlayerInventory);
    //    //TODO: delete : _inv = _characterDb.PlayerInventory; 


    //}

    //private Rect BuildInventoryBox()
    //{
    //    float invWeight = (_slotSize + _slotsPadding) * _slotsX + _slotsPadding;
    //    float invHeight = (_slotSize + _slotsPadding) * _slotsY + _slotsPadding;
    //    _invLocation = new Vector2((Screen.width - invWeight) / 2, (Screen.height - invHeight) / 2);
    //    return new Rect(_invLocation.x - _slotsPadding, _invLocation.y - _slotsPadding, invWeight, invHeight);
    //}
    
    //internal bool AddItemToInventory(ItemContainer item)
    //{
    //    for (int i = 0; i < _inv.Count; i++)
    //    {
    //        if (i > _playerSlots)
    //        {
    //            _GUIManager.AddMessage("RED: Inventory is full");
    //            break;
    //        }
    //        if (_inv[i].Id == -1) //empty Slot
    //        {
    //            _inv[i] = new ItemContainer(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost,
    //                item.Weight, item.MaxStackCnt, item.StackCnt, item.Type, item.Rarity,
    //                item.DurationDays, item.ExpirationTime, item.Values);
    //            _updateInventory = true;
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //void Update()
    //{
    //    //if (Input.GetButtonDown("Inventory"))
    //    if (Input.GetKeyDown(_keyToShowInventory))
    //    {
    //        _showInventory = !_showInventory;
    //        if (_dragging)
    //            PutItemBack();
    //    }

    //    if (_updateInventory)
    //    {
    //        _characterDb.SaveCharacterInventory(_inv); 
    //        _updateInventory = false;
    //    }

    //}

    //void OnGUI()
    //{
    //    //for (int i = 0; i < Inv.Count; i++)
    //    //{
    //    //    GUI.Label(new Rect( 10,10*i,200,50),Inv[i].Name);
    //    //}

    //    _showTooltip = false;
    //    GUI.skin = Skin;

    //    // You can only call GUI ellements only in the OnGUI
    //    if (_showInventory)
    //    {
    //        if (_showInvButton)
    //        {
    //            //Save Inventory to cache Button
    //            if (GUI.Button(new Rect(300, 50, 100, 40), "Save"))
    //                InventoryManager.Instance.SaveInventory(_inv, true);
    //            //Load Inventory from cache Button
    //            if (GUI.Button(new Rect(300, 100, 100, 40), "Load"))
    //                InventoryManager.Instance.LoadInventory(_inv, true);
    //        }

    //        //print(Slots.Count);
    //        DrawInventory();
    //        if (_showTooltip && !_dragging)
    //        {
    //            float dynamicHeight = Skin.box.CalcHeight(new GUIContent(_tooltip), 200);
    //            Rect tooltipBox = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200, dynamicHeight);
    //            GUI.Box(tooltipBox, _tooltip, Skin.GetStyle("box"));
    //        }
    //    }
    //    if (_dragging)
    //    {
    //        //10% bigger than normal
    //        Rect dragBox = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, width: _slotSize * 11 / 10, height: _slotSize * 11 / 10);
    //        InventoryManager.Instance.DrawSprite(dragBox, _draggedItem);
    //    }
    //    if (Event.current.type == EventType.MouseUp && _dragging)
    //    {
    //        PutItemBack();
    //    }
    //}


    //void DrawInventory()
    //{
    //    Event currentEvent = Event.current;

    //    GUI.Box(_invRect, "", Skin.GetStyle("slotNormal"));

    //    for (int y = 0; y < _slotsY; y++)
    //    {
    //        for (int x = 0; x < _slotsX; x++)
    //        {
    //            int invIndex = x + y * _slotsX;
    //            if (invIndex > MaxSlotsCount)
    //                break;
    //            Rect slotRect = new Rect(x * (_slotSize + _slotsPadding) + _invLocation.x, y * (_slotSize + _slotsPadding) + _invLocation.y, _slotSize, _slotSize);
    //            GUI.Box(
    //                    slotRect, 
    //                    "",
    //                    invIndex > _playerSlots ? 
    //                              Skin.GetStyle("slotBroken") 
    //                            : Skin.GetStyle("slotDamaged")
    //                );
    //            if (_inv[invIndex].Id != -1)
    //            {
    //                //Draw the item sprite in the slot 
    //                InventoryManager.Instance.DrawSprite(slotRect, _inv[invIndex]);
    //                //Draw mouse hover
    //                if (slotRect.Contains(currentEvent.mousePosition))
    //                {
    //                    _showTooltip = true;
    //                    _tooltip = _inv[invIndex].GetTooltip();
    //                    //todo: Garbage copllections review all the logics 1/2
    //                    //#################################### START DRAGGING
    //                    if (currentEvent.button == 0 && currentEvent.type == EventType.MouseDrag && !_dragging) //Right click and drag
    //                    {
    //                        _dragging = true;
    //                        _draggedItem = _inv[invIndex];
    //                        _draggedIndex = invIndex;
    //                        _inv[invIndex] = new ItemContainer();
    //                    }
    //                    //#################################### End DRAGGING
    //                    if (currentEvent.type == EventType.MouseUp && _dragging)
    //                    {
    //                        _dragging = false;
    //                        //Drag&Drop #1/3: on filled Item
    //                        //Same items Stack them together 
    //                        //#################################### Stacking
    //                        if ( _inv[invIndex].Id == _draggedItem.Id)
    //                        {   
    //                            if (_inv[invIndex].StackCnt + _draggedItem.StackCnt > _inv[invIndex].MaxStackCnt)
    //                            {
    //                                _draggedItem.setStackCnt(_draggedItem.StackCnt- (_inv[invIndex].MaxStackCnt - _inv[invIndex].StackCnt));
    //                                _inv[invIndex].setStackCnt(_inv[invIndex].MaxStackCnt);
    //                            }
    //                            else
    //                            {
    //                                _inv[invIndex].setStackCnt(_inv[invIndex].StackCnt + _draggedItem.StackCnt);
    //                                _draggedItem.setStackCnt(0);
    //                            }
    //                            PutItemBack();
    //                            _updateInventory = true;
    //                        }
    //                        else //not Same items Mix or swap them
    //                        {
    //                            Recipe newRecipe = InventoryManager.Instance.CheckRecipes( _inv[invIndex].Id,_draggedItem.Id);
    //                            //#################################### Mixing
    //                            if (newRecipe!=null)
    //                            {
    //                                //todo: Garbage copllections review all the logics 2/2
    //                                if (newRecipe.FirstItemCnt <= _inv[invIndex].StackCnt)
    //                                    if (newRecipe.SecondItemCnt <= _draggedItem.StackCnt)
    //                                    {   //Mixing items Logic
    //                                        //todo: use DurationMinutes and Energy in recepie to make it 
    //                                        _inv[invIndex].setStackCnt(_inv[invIndex].StackCnt - newRecipe.FirstItemCnt);
    //                                        _draggedItem.setStackCnt(_draggedItem.StackCnt - newRecipe.SecondItemCnt);
    //                                        ItemContainer item = InventoryManager.Instance.GetItemFromDatabase(newRecipe.FinalItemId);
    //                                        if (_inv[invIndex].StackCnt == 0 )
    //                                        {
                                                
    //                                            _inv[invIndex] = new ItemContainer(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost,
    //                                                item.Weight, item.MaxStackCnt, Math.Min(newRecipe.FinalItemCnt, item.MaxStackCnt), item.Type, item.Rarity,
    //                                                item.DurationDays, item.ExpirationTime, item.Values);
    //                                            PutItemBack();
    //                                        }
    //                                        else if (_draggedItem.StackCnt == 0)
    //                                        {
    //                                            _draggedItem = new ItemContainer(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost,
    //                                                item.Weight, item.MaxStackCnt, Math.Min(newRecipe.FinalItemCnt, item.MaxStackCnt), item.Type, item.Rarity,
    //                                                item.DurationDays, item.ExpirationTime, item.Values);
    //                                        }
    //                                        else
    //                                        {
    //                                            //We need to put back the _draggedItem so we don't put the new item in that place
    //                                            PutItemBack();
    //                                            ItemContainer newItem = new ItemContainer(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost,
    //                                                item.Weight, item.MaxStackCnt, Math.Min(newRecipe.FinalItemCnt, item.MaxStackCnt), item.Type, item.Rarity,
    //                                                item.DurationDays, item.ExpirationTime, item.Values);
    //                                            if (!AddItemToInventory(newItem))
    //                                            {   //Reverce back the changes
    //                                                _inv[invIndex].setStackCnt(_inv[invIndex].StackCnt + newRecipe.FirstItemCnt);
    //                                                _draggedItem.setStackCnt(_draggedItem.StackCnt + newRecipe.SecondItemCnt);
    //                                            }
    //                                        }
    //                                        _updateInventory = true;
    //                                    }
    //                                    else //Not enough materials 
    //                                        _GUIManager.AddMessage("YEL: Not enough " + _draggedItem.Name + " in the inventory, You need " + (newRecipe.FirstItemCnt - _draggedItem.StackCnt) + " more");
    //                                else //Not enough materials 
    //                                    _GUIManager.AddMessage("YEL: Not enough " + _inv[invIndex].Name + " in the inventory, You need "+ (newRecipe.FirstItemCnt-_inv[invIndex].StackCnt) +" more");
    //                                PutItemBack();
    //                            }
    //                            //#################################### Swaping
    //                            else //Swaping items Logic
    //                            {
    //                                _inv[_draggedIndex] = _inv[invIndex];
    //                                _inv[invIndex] = _draggedItem;
    //                                _updateInventory = true;
    //                            }
    //                        }
    //                    }
    //                    //#################################### Consuming
    //                    if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 1) //Right clicked 
    //                    {
    //                        if (_inv[invIndex].Type == Item.ItemType.Consumable || _inv[invIndex].Type == Item.ItemType.Equipment)
    //                        {
    //                            //Use Consumable
    //                            if (UseItem(_inv[invIndex], invIndex))
    //                                Debug.Log("Consumed successfully");
    //                            _updateInventory = true;
    //                        }
    //                    }
    //                    //#################################### Discarding
    //                    if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 2)
    //                    {
    //                        DiscardItem(_inv[invIndex].Id);
    //                        _inv[invIndex] = new ItemContainer();
    //                        _updateInventory = true;
    //                    }
    //                }
    //            }
    //            else if (slotRect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseUp && _dragging)
    //            {
    //                //Drag&Drop #2/3: on empty slot
    //                if (invIndex > _playerSlots)
    //                    _inv[_draggedIndex] = _draggedItem;
    //                else
    //                {
    //                    _inv[_draggedIndex] = _inv[invIndex];
    //                    _inv[invIndex] = _draggedItem;
    //                    _updateInventory = true;
    //                }
    //                _dragging = false;
    //            }
    //        }
    //    }
    //}

    //private void DiscardItem(int id)
    //{
    //    _terrainActions.DropItem(id);
    //}

    //private void PutItemBack()
    //{
    //    //Drag&Drop #3/3: on clear inventory
    //    //TODO NOT change it to <= 
    //    if (_draggedItem.StackCnt == 0 ) 
    //        _inv[_draggedIndex] = new ItemContainer();
    //    else
    //        _inv[_draggedIndex] = _draggedItem;
    //    _dragging = false;
    //}

    //private bool UseItem(ItemContainer item, int invIndex)
    //{
    //    switch (item.Type)
    //    {
    //        case Item.ItemType.Consumable:
    //            //do something;
    //            print("Consumable " + invIndex + " " + item.Name);
    //            return true;
    //        case Item.ItemType.Equipment:
    //            if (item.MaxStackCnt == item.StackCnt)
    //            {
    //                ItemContainer oldItem = InventoryManager.Instance.GetItemFromDatabase(_characterDb.AddEquipment((int)item.PlaceHolder, item.Id));
    //                //Empty or the existing item in the equipment will be replace in invenotory slot
    //                oldItem.setStackCnt(oldItem.MaxStackCnt);
    //                _inv[invIndex] = oldItem;
    //                _GUIManager.AddMessage("GRE: " + item.Name + " Equipt ");
    //                return true;
    //            }
    //            else
    //                _GUIManager.AddMessage("YEL: "+ (item.MaxStackCnt-item.StackCnt) + " more " + item.Name +" is needed!");
    //            return false;
    //        default:
    //            _GUIManager.AddMessage("YEL: " + invIndex + " Not a Consumable ");
    //            return false;
    //    }
    //}
}
