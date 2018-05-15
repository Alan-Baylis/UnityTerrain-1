using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = System.Random;


public class InventoryItem : Item
{
    public int StackCnt { get; set; }


    public InventoryItem(int id, string name, string desc, int cost, ItemType type)
        : base( id, name,  desc,  cost,  type)
    {
        // DerivedClass parameter types have to match base class types
        // Do additional work here otherwise you can leave it empty
        StackCnt = UnityEngine.Random.Range(1,4);
    }

    public InventoryItem()
        : base()
    {
    }


    // Copy Constructor

    public InventoryItem(Item item)
    {
        // copy base class properties.
        foreach (PropertyInfo prop in item.GetType().GetProperties())
        {
            PropertyInfo prop2 = item.GetType().GetProperty(prop.Name);
            prop2.SetValue(this, prop.GetValue(item, null), null);
            StackCnt = 1;
        }
    }
}
