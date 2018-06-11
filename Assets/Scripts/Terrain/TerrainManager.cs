using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    public float EllementChance = 0.70f;
    public static int SceneIdForInsideBuilding = 1;
    public static int Key = 1;
    public Transform Player;
    public float MaxDistanceFromCenter = 7;
    public Vector2 MapOffset ;
    public TerrainType[] TerrainTypes;
    public EllementType[] EllementTypes;

    private int _horizontalTiles = 25;
    public List<EllementType> _availableEllementTypes = new List<EllementType>();
    public List<TerrainType> _availableTerrainTypes = new List<TerrainType>();
    private int _verticalTiles = 25;
    private SpriteRenderer[,] _renderers;
    private IEnumerable<Marker> _markers;
    private List<ActiveEllementType> _Ellements = new List<ActiveEllementType>();
    private Cache _cache;
    
    public ActiveEllementType GetEllement(Vector2 mapPos)
    {
        //max will be 9  or at most 100 
        foreach (var ellement in _Ellements)
        {
            var bLoc = ellement.transform.position;
            //Mappos inbetween the ellement 
            if (mapPos.x == bLoc.x && mapPos.y == bLoc.y)
                return ellement;

            //if (mapPos.x >= bLoc.x - 1
            //    && mapPos.x < bLoc.x
            //    && mapPos.y >= bLoc.y - 1
            //    && mapPos.y < bLoc.y )
            //    return true;
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


    void LoadEllements(char[,] charMap, Vector2 location)
    {
        int ellementMass = RandomHelper.Range(location.x, location.y, Key, 14* 14) /5; 
        for (int i = 0; i < ellementMass; i++)
        {
            //make x,y between 2-14 leave the boundres clear 
            int x = RandomHelper.Range(
                        location.x + i,
                        location.y,
                        Key,
                        12
                    ) + 2;
            int y = RandomHelper.Range(
                        location.x,
                        location.y + i,
                        Key,
                        12
                    ) + 2;
            charMap[x, y] = 'B';
        }
    }


    void LoadAddOns(Marker marker)
    {
        Vector2 rightCornerLocation = new Vector2(marker.Location.x - 8, marker.Location.y - 8);

        if (marker.HasEllement) 
            LoadEllements(marker.CharMap, marker.Location);
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
                //So player doesn't land on a ellement
                if (bLoc.x == 0 && bLoc.y == 0) continue;
                if (marker.CharMap[x, y] == 'B')
                    CreateEllements(bLoc);
            }
        }
    }
    public ActiveEllementType CreateEllements(Vector3 location)
    {
        var Ellement = new GameObject();
        var active = Ellement.AddComponent<ActiveEllementType>();
        _Ellements.Add(active);
        Ellement.transform.position = location;
        var renderer = Ellement.AddComponent<SpriteRenderer>();
        var ellementInfo = _availableEllementTypes[RandomHelper.Range(Ellement.transform.position, Key, _availableEllementTypes.Count)];
        renderer.sprite = ellementInfo.Tile;
        active.EllementTypeInUse = ellementInfo;

        Ellement.name = "Ellement " + Ellement.transform.position;
        Ellement.transform.parent = transform;
        return active;

    }

    void RedrawMap()
    {
        transform.position = new Vector3( (int)Player.position.x, (int)Player.position.y,Player.position.z);
        _markers = Marker.GetMarkers(transform.position.x, transform.position.y, Key, _availableTerrainTypes, EllementChance);

        print("Inside Terrain manager AvailableMarketTerrains : " + _availableTerrainTypes.Count);
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
        _Ellements.ForEach(x => Destroy(x.gameObject));
        _Ellements.Clear();
        foreach (var marker in _markers)
        {
            SetAvailableMarketEllements(marker.Terrain.Id);
            LoadAddOns(marker);
        }
    }

    private void SetAvailableMarketTerrains()
    {
        _availableTerrainTypes.Clear();
        for (int i = 0; i < TerrainTypes.Length; i++)
            if ( TerrainTypes[i].IsActive)
                _availableTerrainTypes.Add(TerrainTypes[i]);
    }


    private void SetAvailableMarketEllements(int terrainIndex)
    {
        _availableEllementTypes.Clear();
        for (int i = 0; i < EllementTypes.Length; i++)
            if (EllementTypes[i].FavouriteTerrain.Contains(terrainIndex) && EllementTypes[i].IsActive)
                _availableEllementTypes.Add(EllementTypes[i]);
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


        SetAvailableMarketTerrains();

        RedrawMap();
        var starter = GameObject.FindObjectOfType<TerrainStarter>();
        if (starter != null)
        {
            Player.position = starter.PreviousPosition;
            Destroy(starter.gameObject);
        }
        else
            SetPlayerlocation();
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
                            0);
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

}
