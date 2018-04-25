using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker  {

    public TerrainType Terrain { get; set; }
    public Vector2 Location { get; set; }
    public bool IsCity { get; set; }
    public float CityMass { get; set; }

    public static IEnumerable<Marker> GetMarkers(float x, float y,int key,TerrainType[] terrains,float cityChance)
    {
        var markers = new Marker[9];
        x = (int)x >> 4;
        y = (int)y >> 4;
        int markerIndex = 0;
        for (int iX = -1; iX < 2; iX++)
        {
            for (int iY = -1; iY < 2; iY++)
            {
                var terrain = terrains[RandomHelper.Range(x + iX, y + iY, key, terrains.Length)];
                //it will be a city if the terrain is walkable && a smal chance to be city 
                bool isCity = (!terrain.NotWalkable) && (cityChance > RandomHelper.Percent(x + iX , y + iY , key));

                //*5 to make it a bigger mass and + 2 to make sure it doesn't come back as 0  Range will be between (2-7) so it is smaller than biom 
                float mass = RandomHelper.Percent(x + iX, y + iY, key) * 5 +2;
                markers[markerIndex++] = new Marker()
                {
                    Terrain = terrain,
                    IsCity = isCity,
                    CityMass = mass,
                    Location = new Vector2((int)(x + iX) << 4, (int)(y + iY) << 4)
                };
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
