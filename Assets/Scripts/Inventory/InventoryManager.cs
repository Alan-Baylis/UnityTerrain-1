using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        print("_availableItems.Count =" + _availableItems.Items.Count);
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
                    invList[i].StackCnt--;
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


    public void InitInventory(List<ItemContainer> invList)
    {
        for (int i = 0; i < _availableItems.Items.Count; i++)
        {
            print(i+" item "+ _availableItems.Items[i].Name);
            invList[i] = _availableItems.Items[i];
            if (i == 2)
                break;
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

    public ItemContainer GetItemFromDatabase(int id,  List<ItemContainer> databaseList)
    {
        for (int i = 0; i < databaseList.Count; i++)
        {
            if (databaseList[i].Id == id)
                return databaseList[i];
        }
        return new ItemContainer();
    }


    public void SaveInventory(List<ItemContainer> invList,bool invSave)
    {
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
        if (invSave)
            for (int i = 0; i < invList.Count; i++)
            {
                invList[i] = InventoryManager.Instance.GetItemFromDatabase(PlayerPrefs.GetInt("Inventory_" + i, -1), _availableItems.Items);
                invList[i].StackCnt = PlayerPrefs.GetInt("InventoryCnt_" + i, 1);
            }
        else
            for (int i = 0; i < invList.Count; i++)
            {
                invList[i] = InventoryManager.Instance.GetItemFromDatabase(PlayerPrefs.GetInt("InvBank_" + i, -1), _availableItems.Items);
                invList[i].StackCnt = PlayerPrefs.GetInt("InvBankCnt_" + i, 1);
            }
    }

}
