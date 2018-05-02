using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInterior : MonoBehaviour {

    public Vector2 MapPosition = Vector2.zero;
    public int Key = 0;
    public Sprite Floor;
    public Transform Player;

    private int _maxWidth;
    private int _maxHeight;
    private int _randomIndex = 0;


    private int Range(int max)
    {
        return RandomHelper.Range(MapPosition, Key + _randomIndex++, max);
    }

    private Rect RandomRoom()
    {
        return new Rect(Range(_maxWidth / 2),                       //X
                        Range(_maxHeight / 2),                      //Y
                        Range(_maxWidth / 4)+ _maxWidth / 4,        //Width
                        Range(_maxHeight / 4) + _maxHeight / 4);    //height
    }

    // Use this for initialization
    void Start () {
        var info = GameObject.FindObjectOfType<InsideBuildingStarter>();
        if (info == null)
            info = new GameObject().AddComponent<InsideBuildingStarter>();
        MapPosition = info.MapPosition;
        Key = info.Key;
        Destroy(info.gameObject);
        GenerateInterior();
	}

    private void GenerateInterior()
    {
        List<Vector3> applied = new List<Vector3>();
        //Whole area of the inside buildings 
        _maxWidth = _maxHeight = Range(12) + 8; //Range 8-20
        int roomCount = Range(4) + 2; //1-5

        var prevRoom = RandomRoom();

        for (int roomIndex = 0; roomIndex < roomCount; roomIndex++)
        {
            var newRoom = RandomRoom();
            if (!prevRoom.Overlaps(newRoom))
            {
                roomCount++;
                continue;
            }

            for (int x = 0; x < newRoom.width; x++)
            {
                for (int y = 0; y < newRoom.height; y++)
                {
                    var tilePos = new Vector3(newRoom.x + x, newRoom.y + y, 0);
                    if (applied.Contains(tilePos))
                        continue;
                    applied.Add(tilePos);
                    var tile = new GameObject();
                    tile.transform.position = tilePos;
                    var renderer = tile.AddComponent<SpriteRenderer>();
                    renderer.sprite = Floor;
                    tile.transform.parent = transform;
                    tile.name = "Room " + tile.transform.position;
                }
            }
            prevRoom = newRoom;
        }
        var position = applied[0];
        position.z = Player.position.z;
        Player.position = position;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
