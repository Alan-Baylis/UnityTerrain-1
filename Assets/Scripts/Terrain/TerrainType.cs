﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class TerrainType
{
    public string Name;
    public int Id;
    public bool IsActive;
    public bool Walkable;
    public bool Flyable;
    public bool Swimable;
    public Sprite[] Tiles;
    public bool IsAnimated;
    public RuntimeAnimatorController AnimationControler;

    public Sprite GetTile (float x,float y,int key)
    {
        return Tiles[RandomHelper.Range(x, y, key, Tiles.Length)];
    }
}
