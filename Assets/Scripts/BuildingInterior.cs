using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInterior : MonoBehaviour {

    public Vector3 PreviousPosition = Vector3.zero;
    public Vector2 MapPosition = Vector2.zero;
    public int Key = 0;
    public Sprite Floor;
    public Sprite Wall;
    public Sprite Door;
    public Transform Player;
    public static int SceneIdForTerrainView = 0;

    private int _maxWidth;
    private int _maxHeight;
    private int _randomIndex = 0;

    private List<Rect> _walls = new List<Rect>();
    private Rect _exit;



    public bool IsExiting(Rect area)
    {
        return _exit.Overlaps(area);
    }


    public bool IsBlocked(Vector3 pos)
    {
        //add an offset 
        pos += Vector3.one / 2;
        foreach (var wall in _walls)
            if (wall.Contains(pos))
                return true;
        return false;
    }


    public bool IsBlocked(Rect area)
    {
        foreach (var wall in _walls)
            if (wall.Overlaps(area))
                return true;
        return false;
    }



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
        var starter = GameObject.FindObjectOfType<InsideBuildingStarter>();
        if (starter == null)
            starter = new GameObject().AddComponent<InsideBuildingStarter>();
        MapPosition = starter.MapPosition;
        Key = starter.Key;
        PreviousPosition = starter.PreviousPosition;
        Destroy(starter.gameObject);
        GenerateInterior();
	}

    private void GenerateInterior()
    {
        List<Vector3> applied = new List<Vector3>();
        List<Vector3> walls = new List<Vector3>();

        //Identify the lowest place to implant the door 
        Vector3 lowestFloor = new Vector3(0, int.MaxValue, 0); 
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
                    if (tilePos.y < lowestFloor.y)
                        lowestFloor = tilePos;
                    //Add potensial Walls in all 8 surunding  places 
                    walls.AddRange(new Vector3[] {
                        tilePos + Vector3.up,
                        tilePos + Vector3.down,
                        tilePos + Vector3.left,
                        tilePos + Vector3.right,
                        tilePos + Vector3.up + Vector3.left,
                        tilePos + Vector3.down + Vector3.left,
                        tilePos + Vector3.up + Vector3.right,
                        tilePos + Vector3.down + Vector3.right
                        });

                    var tile = new GameObject();
                    tile.transform.position = tilePos;
                    var renderer = tile.AddComponent<SpriteRenderer>();
                    renderer.sprite = Floor;
                    tile.transform.parent = transform;
                    tile.name = "Floor " + tile.transform.position;
                }
            }
            prevRoom = newRoom;
        }

        foreach (var wallPos in walls)
        {

            Debug.Log(wallPos);
            //if it is already tiled then it should not be a wall
            if (applied.Contains(wallPos))
                continue;
            applied.Add(wallPos);
            var tile = new GameObject();
            tile.transform.position = wallPos;
            var renderer = tile.AddComponent<SpriteRenderer>();
            renderer.sprite = Wall;
            tile.transform.parent = transform;
            tile.name = "Wall " + tile.transform.position;

            if (wallPos + Vector3.up == lowestFloor)
            {
                renderer.sprite = Door;
                _exit = new Rect(wallPos, Vector3.one);
            }
            else
                //Add to be blocked
                _walls.Add(new Rect(wallPos, Vector2.one));
        }
        var position = applied[0];
        position.z = Player.position.z;
        Player.position = position;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
