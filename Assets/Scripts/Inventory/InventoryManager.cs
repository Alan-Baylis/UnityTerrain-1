using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;

    private ItemDatabase _availableItems;

    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryManager>();
            }
            return instance;
        }
    }


    void Start()
    {
        //print("Inv.Count =" + Inv.Count + InvLocation.ToString());
        _availableItems = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
        //print("_availableItems.Count =" + _availableItems.Items.Count);
    }
    
    public bool MixAbleItems()
    {
        return false;
    }

    public bool CheckItemInInventory(int id, List<ItemContainer> invList)
    {
        for (int i = 0; i < invList.Count; i++)
        {
            if (invList[i].Id == id)
                return true;
        }
        return false;
    }

    public bool RemoveItemFromInventory(int id, List<ItemContainer> invList)
    {
        for (int i = 0; i < invList.Count; i++)
        {
            if (invList[i].Id == id)
            {
                if (invList[i].StackCnt > 1)
                    invList[i].setStackCnt(invList[i].StackCnt -1 );
                else
                    invList[i] = new ItemContainer();
                return true;
            }
        }
        return false;
    }


    //Draw the item sprite in the Rect 
    public void DrawSprite(Rect rect, ItemContainer item)
    {
        //Draw the item sprite in the slot 
        Sprite sprite = item.GetSprite();
        Rect spriteRec = sprite.rect;
        var spriteTexture = sprite.texture;
        spriteRec.xMin /= spriteTexture.width;
        spriteRec.xMax /= spriteTexture.width;
        spriteRec.yMin /= spriteTexture.height;
        spriteRec.yMax /= spriteTexture.height;
        GUI.DrawTextureWithTexCoords(rect, spriteTexture, spriteRec);
        if (item.StackCnt > 1)
            GUI.Label(rect, "<color=black> " + item.StackCnt + "</color>");
    }


    public void InitInventory(List<ItemContainer> invList, int count,int length)
    {
        //print("Inside InitInventory " + count +" "+ length);
        for (int i = 0; i < length; i++)
        {
            if (i < count)
            {
                ItemContainer ni = GetItemFromDatabase((int) Random.Range(0, _availableItems.Items.Count));
                invList.Add(new ItemContainer(ni.Id, ni.Name, ni.Description, ni.IconPath, ni.IconId, ni.Cost, ni.Weight, ni.MaxStackCnt, Random.Range(1, ni.MaxStackCnt), ni.Type, ni.Rarity, DateTime.Now.Add(new TimeSpan(24, 0, 0, 0)), ni.Values));
            }
            else
                invList.Add(new ItemContainer());
        }
    }

    
    public bool AddItemToInventory(int id, List<ItemContainer> invList)
    {
        for (int i = 0; i < invList.Count; i++)
        {
            if (invList[i].Name == null) //empty Slot
            {
                for (int j = 0; j < _availableItems.Items.Count; j++)
                {
                    if (_availableItems.Items[j].Id == id)
                    {
                        invList[i] = _availableItems.Items[j];
                        return true;
                    }
                }
                break;
            }
        }
        return false;
    }

    public ItemContainer GetItemFromDatabase(int id)
    {
        for (int i = 0; i < _availableItems.Items.Count; i++)
        {
            if (_availableItems.Items[i].Id == id)
                return _availableItems.Items[i];
        }
        return new ItemContainer();
    }


    public void SaveInventory(List<ItemContainer> invList,bool invSave)
    {
        //Todo: shouldbe limited to only active invs
        if (invSave)
            for (int i = 0; i < invList.Count; i++)
            {
                PlayerPrefs.SetInt("Inventory_" + i, invList[i].Id);
                PlayerPrefs.SetInt("InventoryCnt_" + i, invList[i].StackCnt);
            }
        else
            for (int i = 0; i < invList.Count; i++)
            {
                PlayerPrefs.SetInt("InvBank_" + i, invList[i].Id);
                PlayerPrefs.SetInt("InvBankCnt_" + i, invList[i].StackCnt);
            }
    }

    public void LoadInventory(List<ItemContainer> invList, bool invSave)
    {
        //Todo: shouldbe limited to only active invs
        if (invSave)
            for (int i = 0; i < invList.Count; i++)
            {
                invList[i] = InventoryManager.Instance.GetItemFromDatabase(PlayerPrefs.GetInt("Inventory_" + i, -1));
                invList[i].setStackCnt(PlayerPrefs.GetInt("InventoryCnt_" + i, 1)); 
            }
        else
            for (int i = 0; i < invList.Count; i++)
            {
                invList[i] = InventoryManager.Instance.GetItemFromDatabase(PlayerPrefs.GetInt("InvBank_" + i, -1));
                invList[i].setStackCnt(PlayerPrefs.GetInt("InvBankCnt_" + i, 1));
            }
    }

}
