using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkBlockedTiles : MonoBehaviour {

    public TerrainManager Terrain_Manager;

    private float radius = 0.25f;
    private float tileRange = 4;



    public bool NotWalkable()
    {
        return true;
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
            for (float x = transform.position.x - tileRange ; x <= transform.position.x + tileRange; x+= radius)
            {
                for (float y = transform.position.y - tileRange; y <= transform.position.y + tileRange; y += radius)
                {
                    Vector3 worldPos= new Vector3(x, y, 0);
                    var mapPos = Terrain_Manager.WorldToMapPosition(worldPos);
                    var terrain = Terrain_Manager.SelectTerrain(mapPos.x, mapPos.y);

                    //UnityEditor.Handles.color = Color.red;
                    //UnityEditor.Handles.DrawWireDisc(worldPos, Vector3.back, radius);
                    var building = Terrain_Manager.GetBuilding(mapPos);
                    if (!terrain.Walkable || (building != null && !building.BuildingTypeInUse.IsEnterable))
                    {
                        UnityEditor.Handles.color = Color.white;
                        UnityEditor.Handles.DrawWireDisc(worldPos, Vector3.back, radius);
                    }
                }
        }
    }


}
