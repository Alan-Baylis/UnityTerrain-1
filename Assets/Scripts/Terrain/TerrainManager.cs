using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    public float BuildingChance = 0.70f;
    public float SpecialChance = 0.01f;
    public static int SceneIdForInsideBuilding = 1;
    public static int Key = 1;
    public Transform Player;
    public float MaxDistanceFromCenter = 7;
    public Vector2 MapOffset ;
    public TerrainType[] TerrainTypes;
    public BuildingType[] BuildingTypes;
    public SpecialBuildingType[] SpecialBuildingTypes;

    private int _horizontalTiles = 25;
    private int _verticalTiles = 25;
    private SpriteRenderer[,] _renderers;
    private IEnumerable<Marker> _markers;
    private List<ActiveBuildingType> _buildings = new List<ActiveBuildingType>();
    private List<ActiveSpecialBuildingType> _specialBuildings = new List<ActiveSpecialBuildingType>();
    private Cache _cache;
    
    public ActiveBuildingType GetBuilding(Vector2 mapPos)
    {
        //max will be 9  or at most 100 
        foreach (var building in _buildings)
        {
            var bLoc = building.transform.position;
            //Mappos inbetween the building 
            if (mapPos.x == bLoc.x && mapPos.y == bLoc.y)
                return building;

            //if (mapPos.x >= bLoc.x - 1
            //    && mapPos.x < bLoc.x
            //    && mapPos.y >= bLoc.y - 1
            //    && mapPos.y < bLoc.y )
            //    return true;
        }
        return null;
    }

    public ActiveSpecialBuildingType GetSpecialBuilding(Vector2 mapPos)
    {
        //max will be 9  or at most 100 
        foreach (var building in _specialBuildings)
        {
            var bLoc = building.transform.position;
            if (mapPos.x == bLoc.x && mapPos.y == bLoc.y)
                return building;
        }
        return null;
    }




    public Vector2 WorldToMapPosition(Vector3 worldPosition)
    {
        if (worldPosition.x < 0) worldPosition.x--;
        if (worldPosition.y < 0) worldPosition.y--;        
        return new Vector2((int)(worldPosition.x + MapOffset.x), (int)(worldPosition.y + MapOffset.y));
    }

    
    public TerrainType SelectTerrain(float x, float y)
    {
        return Marker.Closest(_markers, new Vector2(x, y),Key).Terrain;
    }


    void LoadBuildings(char[,] charMap, Vector2 location)
    {
        int buildingMass = RandomHelper.Range(location.x, location.y, Key, 14* 14) /5; 
        for (int i = 0; i < buildingMass; i++)
        {
            //make x,y between 1-15 leave the boundres clear 
            int x = RandomHelper.Range(
                        location.x + i,
                        location.y,
                        Key,
                        14
                    ) + 1;
            int y = RandomHelper.Range(
                        location.x,
                        location.y + i,
                        Key,
                        14
                    ) + 1;
            charMap[x, y] = 'B';
        }
    }
    void LoadSpecialBuildings(char[,] charMap,Vector2 location)
    {
        int x = RandomHelper.Range(
                    location.x,
                    location.y,
                    Key,
                    14
                ) + 1;
        int y = RandomHelper.Range(
                    location.x,
                    location.y,
                    Key,
                    14
                ) + 1;
        charMap[x, y] = 'S';
    }

    void LoadAddOns(Marker marker)
    {
        Vector2 rightCornerLocation = new Vector2(marker.Location.x - 8, marker.Location.y - 8);

        if (marker.HasBuilding) 
            LoadBuildings(marker.CharMap, marker.Location);
        if (marker.HasSpecial)
            LoadSpecialBuildings(marker.CharMap, marker.Location);
        //Set up the map based on marker.CharMap

        for (int x = 0; x < 16 ; x++)
        {
            for (int y = 0; y < 16 ; y++)
            {
                //Skipp Empty places
                if (marker.CharMap[x, y] == 'E')
                    continue;
                var bLoc = new Vector3(
                    rightCornerLocation.x + x,
                    rightCornerLocation.y + y,
                    0.01f);
                //So player doesn't land on a building
                if (bLoc.x == 0 && bLoc.y == 0) continue;
                if (marker.CharMap[x, y] == 'B')
                    CreateBuilding(bLoc);
                if (marker.CharMap[x, y] == 'S')
                    //Todo: clear surrondings ??
                    CreateSpecial(bLoc);
            }
        }
    }

    public ActiveSpecialBuildingType CreateSpecial(Vector3 location)
    {
        var specialBuilding = new GameObject();
        var active = specialBuilding.AddComponent<ActiveSpecialBuildingType>();
        _specialBuildings.Add(active);
        specialBuilding.transform.position = location;
        var renderer = specialBuilding.AddComponent<SpriteRenderer>();
        var specialBuildingInfo = SpecialBuildingTypes[RandomHelper.Range(specialBuilding.transform.position, Key, SpecialBuildingTypes.Length)];
        renderer.sprite = specialBuildingInfo.Tile;
        active.SpecialBuildingTypeInUse = specialBuildingInfo;

        specialBuilding.name = "BUILDING " + specialBuilding.transform.position;
        specialBuilding.transform.parent = transform;
        return active;
    }
    public ActiveBuildingType CreateBuilding(Vector3 location)
    {
        var building = new GameObject();
        var active = building.AddComponent<ActiveBuildingType>();
        _buildings.Add(active);
        building.transform.position = location;
        var renderer = building.AddComponent<SpriteRenderer>();
        var buildingInfo = BuildingTypes[RandomHelper.Range(building.transform.position, Key, BuildingTypes.Length)];
        renderer.sprite = buildingInfo.Tile;
        active.BuildingTypeInUse = buildingInfo;

        building.name = "Building " + building.transform.position;
        building.transform.parent = transform;
        return active;

    }

    void RedrawMap()
    {
        transform.position = new Vector3( (int)Player.position.x, (int)Player.position.y,Player.position.z);

        _markers = Marker.GetMarkers(transform.position.x, transform.position.y, Key,TerrainTypes,BuildingChance, SpecialChance);

        var offset = new Vector3(
                transform.position.x - _horizontalTiles / 2, 
                transform.position.y - _verticalTiles / 2, 
                0);
        for (int x = 0; x < _horizontalTiles; x++)
        {
            for (int y = 0; y < _verticalTiles; y++)
            {
                var spriteRenderer = _renderers[x, y];
                var terrain = SelectTerrain(
                        offset.x + x,
                        offset.y + y);
                spriteRenderer.sprite = terrain.GetTile(
                        offset.x + x,
                        offset.y + y,
                        Key);
                var animator = spriteRenderer.gameObject.GetComponent<Animator>();
                if (terrain.IsAnimated)
                {
                    if(animator == null )
                    {
                        animator = spriteRenderer.gameObject.AddComponent<Animator>();
                        animator.runtimeAnimatorController = terrain.AnimationControler;
                    }
                }
                else
                {
                    if (animator != null)
                        GameObject.Destroy(animator);
                }
            }
        }
        //Destruction happen at the end of the frame not immediately 
        _buildings.ForEach(x => Destroy(x.gameObject));
        _buildings.Clear();
        _specialBuildings.ForEach(x => Destroy(x.gameObject));
        _specialBuildings.Clear();

        foreach (var marker in _markers)
        {
            LoadAddOns(marker);
        }
        foreach (var item in _cache.Find("Building", transform.position, _horizontalTiles / 2))
            CreateBuilding(item.Location);
        foreach (var item in _cache.Find("SpecialBuilding", transform.position, _horizontalTiles / 2))
            CreateSpecial(item.Location);
    }

    void Start() {

        _cache = Cache.Get();

        //todo: make the player private
        //Player = GameObject.FindGameObjectWithTag("PlayerCamera").transform;


        int sortIndex = 0;
        var offset = new Vector3(0 - _horizontalTiles / 2, 0 - _verticalTiles / 2, 0);
        _renderers = new SpriteRenderer[_horizontalTiles, _verticalTiles];

        for (int x = 0; x < _horizontalTiles; x++)
        {
            for (int y = 0; y < _verticalTiles; y++)
            {
                var tile = new GameObject();
                tile.transform.position = new Vector3(x, y, 0) + offset;
                var renderer = _renderers[x,y]= tile.AddComponent<SpriteRenderer>();
                renderer.sortingOrder = sortIndex--;
                tile.name = "Terrain " + tile.transform.position;
                tile.transform.parent = transform;
            }
        }
        RedrawMap();
        var starter = GameObject.FindObjectOfType<TerrainStarter>();
        if (starter != null)
        {
            Player.position = starter.PreviousPosition;
            Destroy(starter.gameObject);
        }
        else
            SetPlayerlocation();
        print("Trainmanager Starter Player Position:"+Player.position.ToString());
    }

    private void SetPlayerlocation()
    {
        Marker marker = _markers.ElementAtOrDefault(4);
        if (!marker.Terrain.Walkable )
            foreach (var newMarker in _markers)
                if (newMarker.Terrain.Walkable)
                    marker = newMarker;
        for (int r =0; r < 8; r++) //Radiate from the center to find closest empty open space in the middle 
            for (int x = 8 - r; x < 8 + r; x++)
                for (int y = 8 - r; y < 8 + r; y++)
                    if (marker.CharMap[x, y] == 'E')
                    {
                        Vector2 rightCornerLocation = new Vector2(marker.Location.x - 8, marker.Location.y - 8);
                        Player.position = new Vector3(
                            rightCornerLocation.x + x,
                            rightCornerLocation.y + y,
                            0.04f);
                        return;
                    }

    }

    // Update is called once per frame
    void Update () {
        if (MaxDistanceFromCenter<Vector3.Distance(Player.position,transform.position))
        {
            //Debug.Log("Redraw");
            RedrawMap();
        }
	}





    void LoadCity(Marker marker)
    {
        if (!marker.HasBuilding)
            return;

        int cityMass = 10;// (int)marker.BuildingMass;

        bool[,] addAt = new bool[cityMass * 2, cityMass * 2];
        for (int iArea = 0; iArea < cityMass; iArea++)
        {
            int x1 = RandomHelper.Range(
                marker.Location.x + iArea,
                marker.Location.y,
                Key,
                cityMass
                );
            int y1 = RandomHelper.Range(
                marker.Location.x,
                marker.Location.y + iArea,
                Key,
                cityMass
                );
            int x2 = RandomHelper.Range(
                marker.Location.x + iArea,
                marker.Location.y - iArea,
                Key,
                cityMass
                ) + cityMass;
            int y2 = RandomHelper.Range(
                marker.Location.x - iArea,
                marker.Location.y + iArea,
                Key,
                cityMass
                ) + cityMass;
            for (int x = x1; x < x2; x++)
            {
                addAt[x, y1] = true;
                addAt[x, y2] = true;
            }
            for (int y = y1; y < y2; y++)
            {
                addAt[x1, y] = true;
                addAt[x2, y] = true;
            }
            if (RandomHelper.TrueFalse(marker.Location, Key + iArea))
            {
                int removeX = RandomHelper.Range(marker.Location, iArea - Key, x2 - x1) + x1;
                for (int y = 0; y < cityMass * 2; y++)
                    addAt[removeX, y] = false;
            }
            else
            {

                int removeY = RandomHelper.Range(marker.Location, iArea + Key, y2 - y1) + y1;
                for (int x = 0; x < cityMass * 2; x++)
                    addAt[x, removeY] = false;
            }
        }
        for (int x = 0; x < cityMass * 2; x++)
        {
            for (int y = 0; y < cityMass * 2; y++)
            {
                //Skipp some of the cityies
                if (!addAt[x, y])
                    continue;
                var bLoc = new Vector3(
                                    marker.Location.x + cityMass - x,
                                    marker.Location.y + cityMass - y,
                                    0.01f);
                CreateBuilding(bLoc);


            }
        }
    }


}
