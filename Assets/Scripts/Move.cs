using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour {

    public float Speed = 1;
    public float MaxSpeed = 3;
    public KeyCode EnableFastSpeedWithKey = KeyCode.LeftShift;
    public Transform TurnWithMovement;
    public TerrainManager Terrain_Manager;

    private Vector3 previousPosition = Vector3.zero;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var currentSpeed = Speed;
        if (Input.GetKey(EnableFastSpeedWithKey))
        {
            currentSpeed = MaxSpeed;
        }
		var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0);
        transform.Translate(movement * currentSpeed * Time.deltaTime);
        if ( Mathf.Abs(movement.x) > 0.1f || Mathf.Abs(movement.y) > 0.1f)
        {
            TurnWithMovement.rotation = Quaternion.LookRotation(Vector3.back, movement.normalized);
        }

        Vector3 currentPos = transform.position;
        Vector2 mapPos = Terrain_Manager.WorldToMapPosition(currentPos);

        var terrain = Terrain_Manager.SelectTerrain(mapPos.x, mapPos.y);
        var building = Terrain_Manager.GetBuilding(mapPos);
        if (terrain.NotWalkable || (building!=null && !building.BuildingTypeInUse.IsEnterable) )
        {
            transform.position = currentPos = previousPosition;
        }
        if (building != null && building.BuildingTypeInUse.IsEnterable)
        {
            //Debug.Log("Walked in");
            //Preparing to switch the scene
            GameObject go = new GameObject();
            var starter=go.AddComponent<InsideBuildingStarter>();
            starter.Key = TerrainManager.Key;
            starter.MapPosition = mapPos;
            //Make go undestroyable
            GameObject.DontDestroyOnLoad(go);
            go.name = "Inside Building Starter";

            //switch the scene
            SceneManager.LoadScene(TerrainManager.SceneIdForInsideBuilding);
        }
        previousPosition = currentPos;
    }
}
