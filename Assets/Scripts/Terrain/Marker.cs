﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Marker  {

    public TerrainType Terrain { get; set; }
    public Vector2 Location { get; set; }
    public bool HasBuilding { get; set; }
    //public float BuildingMass { get; set; }
    public bool HasSpecial { get; set; }
    public char[,] CharMap { get; set; }

    protected Marker(TerrainType terrain, bool hasBuilding, bool hasSpecial, Vector2 location)
    {
        Terrain = terrain;
        HasBuilding = hasBuilding;
        HasSpecial = hasSpecial;
        Location = location;
        CharMap = new char[16, 16];
        for (int x = 0; x < 16; x++)
            for (int y = 0; y < 16; y++)
                CharMap[x, y] = 'E';
    }

    public static IEnumerable<Marker> GetMarkers(float x, float y,int key,TerrainType[] terrains,float buildingChance, float specialChance)
    {
        var markers = new Marker[9];
        List<TerrainType> activeTerrains = new List<TerrainType>();
        //Find active Terrain Types
        for(int i = 0; i < terrains.Length; i++)
            if (terrains[i].IsActive)
                activeTerrains.Add(terrains[i]);

        x = (int)x >> 4;
        y = (int)y >> 4;
        int markerIndex = 0;
        for (int iX = -1; iX < 2; iX++)
        {
            for (int iY = -1; iY < 2; iY++)
            {
                var terrain = activeTerrains[RandomHelper.Range(x + iX, y + iY, key, activeTerrains.Count)];
                //it will be a city if the terrain is walkable && a small chance to be city 
                bool hasBuilings = (terrain.Walkable) && (buildingChance > RandomHelper.Percent(x + iX , y + iY , key));
                bool hasSpecial = (terrain.Walkable) && (specialChance > RandomHelper.Percent(x + iX, y + iY, key));

                Vector2 location = new Vector2((int) (x + iX) << 4, (int) (y + iY) << 4);
                markers[markerIndex] = new Marker(terrain, hasBuilings, hasSpecial, location);
                //Debug.Log(markerIndex + "-" + markers[markerIndex].Terrain.Name +" " + markers[markerIndex].Location);
                markerIndex++;
            }
        }
        return markers;
    }

    public static Marker Closest(IEnumerable<Marker> markers,Vector2 location,int key)
    {
        Marker selected = null;
        float closest = float.MaxValue;
        foreach (var marker in markers)
        {
            float rand = RandomHelper.Percent(
                (int)(marker.Location.x + location.x),
                (int)(marker.Location.y + location.y),
                key) * 8;
            float distance = Vector2.Distance(marker.Location, location);
            distance -= rand;
            if (distance < closest)
            {
                closest = distance;
                selected = marker;
            }
        }
        return selected;
    }

}