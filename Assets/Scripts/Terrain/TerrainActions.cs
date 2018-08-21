using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainActions : MonoBehaviour {
    private TerrainManager _terrainManager;

    private InventoryHandler _inv;
    public KeyCode KeyToConsume = KeyCode.C;
    public KeyCode KeyToDrop = KeyCode.D;
    public KeyCode KeyToPick = KeyCode.P;

    

    private Cache _cache;

        // Use this for initialization
    void Start () {
        _cache = Cache.Get();
        _inv = InventoryHandler.Instance();
        _terrainManager = TerrainManager.Instance();
    }
	
	// Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        pos.z += 0.01f;
        if (Input.GetKeyDown(KeyToPick))
        {
            ActiveItemType currentItem = _terrainManager.GetDropItem(pos.x, pos.y);
            if (currentItem != null)
                if (_inv.AddItemToInventory(currentItem.ItemTypeInUse.Id))
                    _terrainManager.DistroyItem(currentItem);
        }

        if (Input.GetKeyDown(KeyToConsume))
        {
            var currentElement = _terrainManager.GetEllement(pos.x, pos.y);
            if (currentElement != null)
            {
                if (currentElement.EllementTypeInUse.Distroyable)
                {
                    if (_terrainManager.DistroyEllement(currentElement, true))
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
                        DropItem(elementPos, currentElement.EllementTypeInUse.DropChance, currentElement.EllementTypeInUse.DropItems);
                    }
                }
                return;
            }
            TerrainIns currentTerrain = _terrainManager.SelectTerrain(pos.x, pos.y);
            if (currentTerrain != null)
            {
                if (currentTerrain.Diggable)
                {
                    pos.y -= 0.2f;
                    if (_terrainManager.CreateDigging(pos,true))
                    {
                        _cache.Add(new CacheContent()
                            {
                                Location = pos,
                                ObjectType = "Digging"
                            }
                        );
                        DropItem(pos, currentTerrain.DropChance, currentTerrain.DropItems);
                    }

                }
                return;
            }
        }
        if (Input.GetKeyDown(KeyToDrop))
        {
            //print("###inside Terrainaction: " + pos);
            TerrainIns currentTerrain = _terrainManager.SelectTerrain(pos.x, pos.y);
            if (currentTerrain != null)
                if (currentTerrain.Walkable)
                    DropItem(pos, 1, "0,1,2,3,4");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
            print(collision.gameObject.name);
    }


    private void DropItem(Vector3 pos, float chance, string dropItems)
    {
        print("chance = "+ chance +" Yourchance= " + RandomHelper.Percent(pos, 1) + pos);
        if (chance >= 1f || chance > RandomHelper.Percent(pos, 1))
        {
            List<int> items = dropItems.Split(',').Select(int.Parse).ToList();
            if (items.Count > 0)
            {
                //todo: item based on rarity of item 
                int itemId = items[RandomHelper.Range(pos, 1, items.Count)];
                _terrainManager.CreateItem(pos, itemId);
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
