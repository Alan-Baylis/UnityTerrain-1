using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TerrainIns
{

    public enum TerrainType
    {
        Land,
        Desert,
        Rock,
        Water,
        Snow,
        Death,
        Lava
    }


    public string Name;
    //public int Id;
    public bool IsActive;
    public bool IsDiggable;
    public bool Walkable;
    public bool Flyable;
    public bool Swimable;
    public bool HasElement;
    public string DropItems;
    public float DropChance;
    public TerrainType Type;
    public Sprite[] Tiles;
    public bool IsAnimated;
    public RuntimeAnimatorController[] AnimationControler;

    public Sprite GetTile (float x,float y,int key)
    {
        return Tiles[RandomHelper.Range(x, y, key, Tiles.Length)];
    }


    public RuntimeAnimatorController GetAnimation(float x, float y, int key)
    {
        return AnimationControler[RandomHelper.Range(x, y, key, AnimationControler.Length)];
    }
}
