using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class EllementType {
    public string Name;
    public bool IsActive;
    public bool IsEnterable;
    public bool IsDistroyable;
    public bool Walkable;
    public bool Flyable;
    public bool Swimable;
    public string DropItems;
    public float DropChance;
    public TerrainIns.TerrainType FavouriteTerrainTypes;
    public Sprite Tile;
}
