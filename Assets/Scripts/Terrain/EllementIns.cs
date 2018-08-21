using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class EllementIns {

    public enum EllementType
    {
        Hole,
        Building,
        Tree,
        Bush,
        Rock
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string MultipleName { get; set; }
    public int MultipleId { get; set; }
    public bool Walkable { get; set; }
    public bool Flyable { get; set; }
    public bool Swimable { get; set; }
    public bool Enterable { get; set; }
    public bool Distroyable { get; set; }
    public string DropItems { get; set; }
    public float DropChance { get; set; }
    public EllementType Type { get; set; }
    public TerrainIns.TerrainType FavouriteTerrainTypes { get; set; }
    public bool IsEnabled { get; set; }

    private Sprite _tile ;
    public Sprite GetSprite()
    {
        if (_tile == null)
            BuildSprite();
        return _tile;
    }

    public void BuildSprite()
    {
        if (MultipleId!=-1)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Ellements/" + MultipleName);
            // Get specific sprite
            _tile = sprites[MultipleId];

        }
        else
            _tile = Resources.Load<Sprite>("Ellements/"+ Name);
    }


    internal void Print()
    {
        Debug.Log(
            " Ellement:" + Id + 
            " Name:" + Name + 
            " Type:" + Type + 
            " Distroyable:" + Distroyable + 
            " IsEnabled:" + IsEnabled + 
            " FavouriteTerrainTypes:" + FavouriteTerrainTypes
            );
    }

}
