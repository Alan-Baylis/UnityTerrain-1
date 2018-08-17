﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

    public int Id { get; set; }
    public string Name { get; set; }
    public bool Diggable { get; set; }
    public bool Walkable { get; set; }
    public bool Flyable { get; set; }
    public bool Swimable { get; set; }
    public bool HasElement { get; set; }
    public bool HasMonster { get; set; }
    public string DropItems { get; set; }
    public float DropChance { get; set; }
    public TerrainType Type { get; set; }
    public int Tiles { get; set; }
    public int AnimationControler { get; set; }
    public bool IsEnabled { get; set; }
    private List<Sprite> _tiles = new List<Sprite>();
    private List<RuntimeAnimatorController> _rac = new List<RuntimeAnimatorController>();

    public Sprite GetTile (float x,float y,int key)
    {
        if (_tiles.Count == 0)
            GetSprites();
        int iconId = RandomHelper.Range(x, y, key, Tiles);
        return _tiles[iconId];
    }

    public void GetSprites()
    {
        _tiles = Resources.LoadAll<Sprite>("Terrain/" + Name).ToList();
    }


    public RuntimeAnimatorController GetAnimation(float x, float y, int key)
    {
        if (_rac.Count == 0)
            GetAnimations();
        int iconId = RandomHelper.Range(x, y, key, AnimationControler);
        return _rac[iconId];
    }

    public void GetAnimations()
    {
        _rac = Resources.LoadAll<RuntimeAnimatorController>("Terrain/" + Name).ToList();
    }

    internal void Print()
    {
        Debug.Log(
            " Terrain:" + Id + 
            " Name:" + Name + 
            " Type:" + Type + 
            " Tiles:" + Tiles + 
            " HasElement:" + HasElement + 
            " IsEnabled:" + IsEnabled + 
            " AnimationControler:" + AnimationControler
            );
    }

}
