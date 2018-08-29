using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingInterior : MonoBehaviour {

    public Vector3 PreviousPosition = Vector3.zero;
    public int Key = 0;
    //public Sprite Floor;
    public Sprite[] FloorTiles;
    public Sprite Wall;
    public Sprite DoorShadow;
    public Transform Player;
    public static int SceneIdForTerrainView = 0;

    public Vector2 _mapPosition = Vector2.zero;
    private int _maxWidth;
    private int _maxHeight;
    private Sprite _floor;
    private int _randomIndex = 0;

    private List<Rect> _walls = new List<Rect>();
    private Rect _exit;



    public bool IsExiting(Rect area)
    {
        return _exit.Overlaps(area);
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
        return RandomHelper.Range(_mapPosition, Key + _randomIndex++, max);
    }

    private Rect RandomRoom()
    {
        return new Rect(Range(_maxWidth / 2),                       //X
                        Range(_maxHeight / 2),                      //Y
                        Range(_maxWidth  / 4) + _maxWidth  / 4,        //Width
                        Range(_maxHeight / 4) + _maxHeight / 4);    //height
    }

    // Use this for initialization
    void Start () {
        var starter = GameObject.FindObjectOfType<SceneStarter>();
        if (starter == null)
            starter = new GameObject().AddComponent<SceneStarter>();
        _mapPosition = starter.MapPosition;
        Key = starter.Key;
        PreviousPosition = starter.PreviousPosition;
        //for some reason when it is not float it only return 0
        _floor = FloorTiles[RandomHelper.Range(new Vector2(_mapPosition.x + 0.01f, _mapPosition.y + 0.01f) , Key, FloorTiles.Length)];
        //print("Building Interior _floor: "  +"  " + _floor.name);
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
        int roomCount = Range(6) + 2; //1-5

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
                    var tileRenderer = tile.AddComponent<SpriteRenderer>();
                    tileRenderer.sprite = _floor;
                    tile.transform.parent = transform;
                    tile.name = "Floor " + tile.transform.position;
                }
            }
            prevRoom = newRoom;
        }

        List<Vector3> sortedWalls = walls.OrderByDescending(v => v.y).ToList();
        float wallOrder = -0.001f;
        foreach (var wallPos in sortedWalls)
        {

            //Debug.Log(wallPos);
            //if it is already tiled then it should not be a wall
            if (applied.Contains(wallPos))
                continue;
            applied.Add(wallPos);
            var tile = new GameObject();

            //Helps the 3D look of the wall lookbetter 
            tile.transform.position = new Vector3(wallPos.x, wallPos.y, wallPos.z + wallOrder);
            wallOrder -= 0.001f;
            //tile.transform.position = wallPos;

            var wallRenderer = tile.AddComponent<SpriteRenderer>();
            wallRenderer.sprite = Wall;
            tile.transform.parent = transform;
            tile.name = "Wall " + tile.transform.position;

            if (wallPos + Vector3.up == lowestFloor)
            {
                tile.name = "DoorFloor " + tile.transform.position;
                wallRenderer.sprite = _floor;
                _exit = new Rect(wallPos, Vector3.one);

                //Add the doorway shadow onject
                var shadow = new GameObject();
                shadow.transform.position = new Vector3(wallPos.x, wallPos.y, wallPos.z + wallOrder);
                wallOrder -= 0.001f;
                var doorRenderer = shadow.AddComponent<SpriteRenderer>();
                doorRenderer.sprite = DoorShadow;
                shadow.transform.parent = transform;
                shadow.name = "DoorShadow " + shadow.transform.position;
            }
            else
                //Add to be blocked
                _walls.Add(new Rect(wallPos, Vector2.one));
        }
        lowestFloor.z = Player.position.z;
        Player.position = lowestFloor;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
