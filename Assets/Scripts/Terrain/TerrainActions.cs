using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainActions : MonoBehaviour {
    private TerrainManager _terrainManager;
    private GUIManager _GUIManager;

    private InventoryHandler _inv;
    public KeyCode KeyToConsume = KeyCode.C;
    public KeyCode KeyToDrop = KeyCode.D;
    //public KeyCode KeyToPick = KeyCode.P;

    

    private Cache _cache;

        // Use this for initialization
    void Start () {
        _cache = Cache.Get();
        _inv = InventoryHandler.Instance();
        _terrainManager = TerrainManager.Instance();
        _GUIManager = GUIManager.Instance();
    }
	
	// Update is called once per frame
    void Update()
    {
        if (_inv.InventoryPanelStat())
            return;

        var pos = transform.position;
        pos.z += 0.01f;

        if (Input.GetMouseButtonDown(0))
        {
            var touchLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(touchLocation, pos);
            if (distance > 1)
            {
                _GUIManager.AddMessage("YEL: Too far from this location");
            }
            else
            {
                //Pick Up Item
                var currentItem = _terrainManager.GetDropItem(touchLocation);
                if (currentItem != null)
                {
                    if (_inv.AddItemToInventory(currentItem.ItemTypeInUse.Id))
                        _terrainManager.DistroyItem(currentItem);
                    return;
                }
                //Consume Ellemnt
                else
                {
                    var currentElement = _terrainManager.GetEllement(touchLocation);
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
                                _terrainManager.DropItem(elementPos, currentElement.EllementTypeInUse.DropChance, currentElement.EllementTypeInUse.DropItems);
                            }
                        }
                        return;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var touchLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(touchLocation, pos);
            if (distance > 1)
            {
                _GUIManager.AddMessage("YEL: Too far from this location");
            }
            else
            {
                TerrainIns currentTerrain = _terrainManager.SelectTerrain(pos.x, pos.y);
                if (currentTerrain != null)
                {
                    if (currentTerrain.Diggable)
                    {
                        pos.y -= 0.2f;
                        if (_terrainManager.CreateDigging(pos, true))
                        {
                            _cache.Add(new CacheContent()
                            {
                                Location = pos,
                                ObjectType = "Digging"
                            }
                            );
                            _terrainManager.DropItem(pos, currentTerrain.DropChance, currentTerrain.DropItems);
                        }
                    }
                    return;
                }
            }
        }
        if (Input.GetKeyDown(KeyToDrop))
        {
            //print("###inside Terrainaction: " + pos);
            TerrainIns currentTerrain = _terrainManager.SelectTerrain(pos.x, pos.y);
            if (currentTerrain != null)
                if (currentTerrain.Walkable)
                    _terrainManager.DropItem(pos, 1, "0,1,2,3,4");
        }


        //if (Input.GetKeyDown(KeyToPick))
        //{
        //    ActiveItemType currentItem = _terrainManager.GetDropItem(pos);
        //    if (currentItem != null)
        //        if (_inv.AddItemToInventory(currentItem.ItemTypeInUse.Id))
        //            _terrainManager.DistroyItem(currentItem);
        //}
        //if (Input.GetKeyDown(KeyToConsume))
        //{
        //    var currentElement = _terrainManager.GetEllement(pos);
        //    if (currentElement != null)
        //    {
        //        if (currentElement.EllementTypeInUse.Distroyable)
        //        {
        //            if (_terrainManager.DistroyEllement(currentElement, true))
        //            {
        //                Vector3 elementPos = currentElement.transform.position;
        //                //Remember Consume elemnt to not draw them 
        //                //print("###Inside KeyToConsume currentElement ADD to cache" + currentElement.name);
        //                _cache.Add(new CacheContent()
        //                    {
        //                        Location = elementPos,
        //                        ObjectType = "VacantElement"
        //                    }
        //                );
        //                _terrainManager.DropItem(elementPos, currentElement.EllementTypeInUse.DropChance, currentElement.EllementTypeInUse.DropItems);
        //            }
        //        }
        //        return;
        //    }
        //    TerrainIns currentTerrain = _terrainManager.SelectTerrain(pos.x, pos.y);
        //    if (currentTerrain != null)
        //    {
        //        if (currentTerrain.Diggable)
        //        {
        //            pos.y -= 0.2f;
        //            if (_terrainManager.CreateDigging(pos,true))
        //            {
        //                _cache.Add(new CacheContent()
        //                    {
        //                        Location = pos,
        //                        ObjectType = "Digging"
        //                    }
        //                );
        //                _terrainManager.DropItem(pos, currentTerrain.DropChance, currentTerrain.DropItems);
        //            }
        //        }
        //        return;
        //    }
        //}

    } 
}
