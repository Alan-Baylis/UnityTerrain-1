using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour {

    public TerrainManager Terrain_Manager;

    private Vector3 previousPosition = Vector3.zero;

    private Character _playerCharacter;

    // Use this for initialization
    void Start () {

        _playerCharacter = CharacterManager.Instance.GetCharacterFromDatabase(0);
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 currentPos = transform.position;
        Vector2 mapPos = Terrain_Manager.WorldToMapPosition(currentPos);

        var terrain = Terrain_Manager.SelectTerrain(mapPos.x, mapPos.y);
        var ellement = Terrain_Manager.GetEllement(mapPos);
        
        if (
            //Terrain Types and charcter type 
            (!terrain.Walkable && _playerCharacter.MoveType == Character.CharacterType.Walk) || 
            (!terrain.Flyable && _playerCharacter.MoveType == Character.CharacterType.Fly) ||
            (!terrain.Swimable && _playerCharacter.MoveType == Character.CharacterType.Swim) ||
            //ellement + character
            (ellement != null && !ellement.EllementTypeInUse.IsEnterable  && _playerCharacter.MoveType != Character.CharacterType.Fly)  )
        {
            transform.position = currentPos = previousPosition;
        }
        if (ellement != null && ellement.EllementTypeInUse.IsEnterable)
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
        previousPosition = currentPos;
    }
}
