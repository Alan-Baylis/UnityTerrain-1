using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class TerrainType {
    public string Name;
    public bool NotWalkable;
    public Sprite[] Tiles;
    public bool IsAnimated;
    public RuntimeAnimatorController AnimationControler;

    public Sprite GetTile (float x,float y,int key)
    {
        return Tiles[RandomHelper.Range(x, y, key, Tiles.Length)];
    }
}
