using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour {

    public TerrainManager Terrain_Manager;

    private Vector3 previousPosition = Vector3.zero;
    
    private CharacterManager _characterManager;

    private Cache _cache;



    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        _cache = Cache.Get();
    }

    // Use this for initialization
    void Start () {
        /*foreach (var item in _cache.Find("Player",true))
        {
            if (IsBlocked(item.Location))
                continue;
            else
                previousPosition = item.Location;
        }*/
    }

    private bool IsBlocked(Vector3 itemLocation)
    {
        Vector3 currentPos = itemLocation;
        Vector2 mapPos = Terrain_Manager.WorldToMapPosition(currentPos);
        var terrain = Terrain_Manager.SelectTerrain(mapPos.x, mapPos.y);
        var ellement = Terrain_Manager.GetEllement(mapPos);
        if (
            //Terrain Types and charcter type 
            (!terrain.Walkable && _characterManager.Character.MoveType == Character.CharacterType.Walk) ||
            (!terrain.Flyable && _characterManager.Character.MoveType == Character.CharacterType.Fly) ||
            (!terrain.Swimable && _characterManager.Character.MoveType == Character.CharacterType.Swim) ||
            //ellement + character
            (ellement != null && !ellement.EllementTypeInUse.IsEnterable && _characterManager.Character.MoveType != Character.CharacterType.Fly)
            )
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
	void Update ()
	{
	    Vector3 currentPos = transform.position;
	    Vector2 mapPos = Terrain_Manager.WorldToMapPosition(currentPos);
	    var ellement = Terrain_Manager.GetEllement(mapPos);
        //print("###Inside Move Update: "+ currentPos+ previousPosition+ mapPos);
        if (IsBlocked(currentPos))
        { 
            transform.position = currentPos = previousPosition;
        }
        if (ellement != null && ellement.EllementTypeInUse.IsEnterable)
        {
            //todo: make canvas DontDestroyOnLoad
            //https://answers.unity.com/questions/1245665/keep-some-ui-alive-between-scenes.html
            //https://answers.unity.com/questions/1396450/how-to-not-lose-canvas-reference-when-loading-new.html

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
