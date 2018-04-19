using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkBlockedTiles : MonoBehaviour {

    public TerrainManager Terrain_Manager;

    private float radius = 0.25f;
    private float tileRange = 4;

    void OnDrawGizmos()
    {
        if (Application.isEditor)
            for (float x = transform.position.x - tileRange ; x <= transform.position.x + tileRange; x+= radius)
            {
                for (float y = transform.position.x - tileRange; y <= transform.position.x + tileRange; y += radius)
                {
                    Vector3 worldPos= new Vector3(x, y, 0);
                    Vector2 mapPos = Terrain_Manager.WorldToMapPosition(worldPos);
                    bool isBlocked = false;
                    Terrain_Manager.SelectRandomSprite(mapPos.x, mapPos.y, out isBlocked);
                    if (isBlocked)
                    {
                        UnityEditor.Handles.DrawWireDisc(worldPos, Vector3.back, radius);
                    }
                }
        }
    }


}
