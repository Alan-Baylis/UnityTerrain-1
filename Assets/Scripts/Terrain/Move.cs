using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour {

    public TerrainManager Terrain_Manager;

    private Vector3 previousPosition = Vector3.zero;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 currentPos = transform.position;
        Vector2 mapPos = Terrain_Manager.WorldToMapPosition(currentPos);

        var terrain = Terrain_Manager.SelectTerrain(mapPos.x, mapPos.y);
        var building = Terrain_Manager.GetBuilding(mapPos);
	    var specialBuilding = Terrain_Manager.GetSpecialBuilding(mapPos);
        if (!terrain.Walkable || 
            (building!=null && !building.BuildingTypeInUse.IsEnterable) || 
            (specialBuilding != null && !specialBuilding.SpecialBuildingTypeInUse.IsEnterable) )
        {
            transform.position = currentPos = previousPosition;
        }
        if (building != null && building.BuildingTypeInUse.IsEnterable)
        {
            //Debug.Log("Walked in");
            //Preparing to switch the scene
            GameObject go = new GameObject();
            //Make go undestroyable
            GameObject.DontDestroyOnLoad(go);
            var starter=go.AddComponent<InsideBuildingStarter>();
            starter.Key = TerrainManager.Key;
            starter.MapPosition = mapPos;
            starter.PreviousPosition = previousPosition;
            go.name = "Inside Building Starter";

            //switch the scene
            SceneManager.LoadScene(TerrainManager.SceneIdForInsideBuilding);
        }
	    if (specialBuilding != null && specialBuilding.SpecialBuildingTypeInUse.IsEnterable)
	    {
	        Debug.Log("Walked in SB");
	    }
        previousPosition = currentPos;
    }
}
