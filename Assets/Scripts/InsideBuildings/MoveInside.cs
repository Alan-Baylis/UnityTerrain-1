using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveInside : MonoBehaviour
{


    public BuildingInterior Building;

    private Vector3 previousPosition = Vector3.zero;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = transform.position;
        var targetTile = new Rect(currentPos, Vector2.one);

        if (Building.IsExiting(targetTile))
        {
            //Preparing to sreturn to terrain
            GameObject go = new GameObject();
            //Make go undestroyable
            GameObject.DontDestroyOnLoad(go);
            var starter = go.AddComponent<TerrainStarter>();
            starter.PreviousPosition = Building.PreviousPosition;
            go.name = "Terrain Starter";
            //switch the scene
            SceneManager.LoadScene(BuildingInterior.SceneIdForTerrainView);
        }

        if (Building.IsBlocked(targetTile))
            transform.position = currentPos = previousPosition;
        previousPosition = currentPos;
    }
}
