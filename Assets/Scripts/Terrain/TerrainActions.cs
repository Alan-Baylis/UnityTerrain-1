﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainActions : MonoBehaviour {

    public TerrainManager Terrain_Manager;
    public Inventory Inventory_Manager;
    public KeyCode KeyToConsume = KeyCode.C;
    public KeyCode KeyToDrop = KeyCode.D;
    public KeyCode KeyToPick = KeyCode.P;

    

    private Cache _cache;

	// Use this for initialization
	void Start () {
        _cache = Cache.Get();
	}
	
	// Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        pos.z += 0.01f;
        if (Input.GetKeyDown(KeyToPick))
        {
            var currentItem = Terrain_Manager.GetDropItem(pos.x, pos.y);
            if (currentItem != null)
            {
                //print("###Inside Terrrain actions : Item id =" + currentItem.ItemTypeInUse.Id);
                if (Inventory_Manager.AddItemToInventory(currentItem.ItemTypeInUse))
                    Terrain_Manager.DistroyItem(currentItem);
            }
        }

        if (Input.GetKeyDown(KeyToConsume))
        {

            var currentElement = Terrain_Manager.GetEllement(pos.x, pos.y);
            if (currentElement != null)
            {
                if (currentElement.EllementTypeInUse.IsDistroyable)
                {
                    Vector3 elementPos = currentElement.transform.position;
                    //Remember Consume elemnt to not draw them 
                    //print("###Inside KeyToConsume currentElement ADD to cache" + currentElement.name);
                    _cache.Add(new CacheContent()
                        {
                            Location = elementPos,
                            ObjectType = "VacantElement"
                        }
                    );
                    Terrain_Manager.DistroyEllement(currentElement);
                    //Last relaiable location
                    //TODO: Removed on 6/28 => after adding remember consumed elememnt this might not needed and also 
                    /*_cache.Add(new CacheContent()
                        {
                            Location = transform.position,
                            ObjectType = "Player"
                        }
                    );
                    */
                    DropItem(elementPos, currentElement.EllementTypeInUse.DropChance, currentElement.EllementTypeInUse.DropItems);
                }
                return;
            }

            TerrainIns currentTerrain = Terrain_Manager.SelectTerrain(pos.x, pos.y);
            if (currentTerrain != null)
            {
                if (currentTerrain.IsDiggable)
                {
                    pos.y -= 0.2f;
                    Terrain_Manager.CreateDigging(pos);
                    _cache.Add(new CacheContent()
                        {
                            Location = pos,
                            ObjectType = "Digging"
                        }
                    );
                    DropItem(pos, currentTerrain.DropChance, currentTerrain.DropItems);
                }
                return;
            }
        }

        if (Input.GetKeyDown(KeyToDrop))
        {
            //print("###inside Terrainaction: " + pos);
            TerrainIns currentTerrain = Terrain_Manager.SelectTerrain(pos.x, pos.y);
            if (currentTerrain != null)
                if (currentTerrain.Walkable)
                    DropItem(pos, 1, "0,1,2,3,4");
        }
    }


    public void DropItem(int id)
    {
        var pos = transform.position;
        pos.z += 0.01f;
        DropItem(pos, 1, id.ToString());
    }

    private void DropItem(Vector3 pos, float chance, string dropItems)
    {
        if (chance == 1 || chance > RandomHelper.Percent(pos, 1))
        {
            List<int> items = dropItems.Split(',').Select(int.Parse).ToList();
            //print("###Inside KeyToConsume : (" + items.Count + ")" + dropItems + "=>" + RandomHelper.Range(pos, 1, items.Count));
            if (items.Count > 0)
            {
                //todo: item based on rarity of item 
                int itemId = items[RandomHelper.Range(pos, 1, items.Count)];
                Terrain_Manager.CreateItem(pos, itemId);
                _cache.Add(new CacheContent()
                    {
                        Location = pos,
                        Content = itemId.ToString(),
                        ObjectType = "Item"
                    }
                );
            }
        }
    }
}
