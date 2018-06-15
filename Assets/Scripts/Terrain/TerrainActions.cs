using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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
        //So the new object will be n top of terrain 
        pos.z = 0.01f;
        if (Input.GetKeyDown(KeyToPick))
        {
            var currentItem = Terrain_Manager.GetDropItem(pos.x, pos.y);
            if (currentItem != null)
            {
                print("###Inside Terrrain actions : Item id =" + currentItem.ItemTypeInUse.Id);
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
                    //Remember Consume elemnt to not draw them 
                    _cache.Add(new CacheContent()
                        {
                            Location = transform.position,
                            ObjectType = "VacantElement"
                        }
                    );
                    Terrain_Manager.DistroyEllement(currentElement);
                    //Last relaiable location  //TODO: after adding remember consumed elememnt this might not needed and also 
                    _cache.Add(new CacheContent()
                        {
                            Location = transform.position,
                            ObjectType = "Player"
                        }
                    );
                }

                return;
            }

            TerrainIns currentTerrain = Terrain_Manager.SelectTerrain(pos.x, pos.y);
            if (currentTerrain != null)
            {
                if (currentTerrain.IsDiggable)
                {
                    Terrain_Manager.CreateDigging(pos);
                    _cache.Add(new CacheContent()
                        {
                            Location = pos,
                            ObjectType = "Digging"
                        }
                    );
                }

                return;
            }
        }

        if (Input.GetKeyDown(KeyToDrop))
        {
            int itemId = 1;

            //print("###inside Terrainaction: " + pos);
            TerrainIns currentTerrain = Terrain_Manager.SelectTerrain(pos.x, pos.y);
            if (currentTerrain != null)
            {
                if (currentTerrain.Walkable)
                {
                    Terrain_Manager.CreateItem(pos, itemId);
                    _cache.Add(new CacheContent()
                        {
                            Location = pos,
                            Content = itemId.ToString(),
                            ObjectType = "Item"
                    }
                    );
                }
                return;
            }
        }
    }
}
