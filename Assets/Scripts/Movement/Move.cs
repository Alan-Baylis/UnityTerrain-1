using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour {
    private TerrainManager _terrainManager;

    private Vector3 previousPosition = Vector3.zero;
    
    private CharacterManager _characterManager;
    private SpriteRenderer _renderer;
    private Cache _cache;



    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        //_cache = Cache.Get();
        _terrainManager = TerrainManager.Instance();

    }

    // Use this for initialization
    void Start ()
    {
        _renderer = transform.GetComponentsInChildren<SpriteRenderer>()[0];
        /*foreach (var item in _cache.Find("Player",true))
        {
            if (IsBlocked(item.Location))
                continue;
            else
                previousPosition = item.Location;
        }*/
    }

    private bool IsBlocked(Vector3 itemLocation, ActiveEllementType ellement)
    {
        Vector3 currentPos = itemLocation;
        Vector2 mapPos = _terrainManager.WorldToMapPosition(currentPos);
        var terrain = _terrainManager.SelectTerrain(mapPos.x, mapPos.y);
        if (
            //Terrain Types and charcter type 
            (!terrain.Walkable && _characterManager.Character.MoveT == Character.CharacterType.Walk) ||
            (!terrain.Flyable && _characterManager.Character.MoveT == Character.CharacterType.Fly) ||
            (!terrain.Swimable && _characterManager.Character.MoveT == Character.CharacterType.Swim) ||
            //ellement + character
            (ellement != null && !ellement.EllementTypeInUse.Enterable && _characterManager.Character.MoveT != Character.CharacterType.Fly)
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
        Vector2 mapPos = _terrainManager.WorldToMapPosition(currentPos);
        var ellement = _terrainManager.GetEllement(mapPos);
	    if (ellement != null)
	        if(ellement.transform.position.y> currentPos.y)
	            ellement.transform.GetComponent<SpriteRenderer>().sortingOrder = _renderer.sortingOrder -1;
            else
	            ellement.transform.GetComponent<SpriteRenderer>().sortingOrder = _renderer.sortingOrder + 1;
        //print("###Inside Move Update: "+ currentPos+ previousPosition+ mapPos);
        if (IsBlocked(currentPos, ellement))
        { 
            transform.position = currentPos = previousPosition;
        }
        if (ellement != null && ellement.EllementTypeInUse.Enterable)
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
