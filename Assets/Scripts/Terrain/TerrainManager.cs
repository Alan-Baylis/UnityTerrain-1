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
    public TerrainIns[] TerrainTypes;
    public EllementType[] EllementTypes;
    public Sprite Dig;


    private ItemDatabase _itemDatabase;

    private int _horizontalTiles = 25;
    public List<EllementType> _availableEllementTypes = new List<EllementType>();
    public List<TerrainIns> _availableTerrainTypes = new List<TerrainIns>();
    private int _verticalTiles = 25;
    private SpriteRenderer[,] _renderers;
    private IEnumerable<Marker> _markers;
    private List<ActiveEllementType> _ellements = new List<ActiveEllementType>();
    private List<ActiveItemType> _items = new List<ActiveItemType>();
    private List<GameObject> _digs = new List<GameObject>();

    private Cache _cache;
    
    public Vector2 WorldToMapPosition(Vector3 worldPosition)
    {
        if (worldPosition.x < 0) worldPosition.x--;
        if (worldPosition.y < 0) worldPosition.y--;        
        return new Vector2((int)(worldPosition.x + MapOffset.x), (int)(worldPosition.y + MapOffset.y));
    }

    
    public TerrainIns SelectTerrain(float x, float y)
    {
        return Marker.Closest(_markers, new Vector2(x, y),Key).Terrain;
    }




    void LoadEllements(Marker marker)
    {
        Vector2 rightCornerLocation = new Vector2(marker.Location.x - 8, marker.Location.y - 8);

        if (marker.HasEllement)
            SetEllements(marker.CharMap, marker.Location);
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

    void RedrawMap()
    {
        transform.position = new Vector3( (int)Player.position.x, (int)Player.position.y,Player.position.z);
        _markers = Marker.GetMarkers(transform.position.x, transform.position.y, Key, _availableTerrainTypes, EllementChance);
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
                        animator.runtimeAnimatorController = terrain.GetAnimation(
                                                                            offset.x + x,
                                                                            offset.y + y,
                                                                            Key);
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
        _ellements.ForEach(x => Destroy(x.gameObject));
        _ellements.Clear();
        _cache.SyncItems("Item", _items);
        _items.ForEach(x => Destroy(x.gameObject));
        _items.Clear();
        _digs.ForEach(x => Destroy(x.gameObject));
        _digs.Clear();
        foreach (var marker in _markers)
        {
            SetAvailableMarketEllements(marker.Terrain.Type);
            LoadEllements(marker);
        }
        //If it has been consumed recently delete them

        //print("###Inside RedrawMap Player pos " +  transform.position+ _horizontalTiles / 2);
        foreach (var element in _cache.Find("VacantElement", transform.position, _horizontalTiles / 2, true))
        {
            var currentElement = GetEllement(element.Location.x, element.Location.y);
            DistroyEllement(currentElement);
            //print("###Inside RedrawMap currentElement Remove"+ currentElement.name+ transform.position);
        }
        LoadCaches();
    }

    private void LoadCaches()
    {
        _cache = Cache.Get();
        //print("###Inside LoadCaches: " + transform.position);
        foreach (var item in _cache.Find("Digging", transform.position, _horizontalTiles / 2,true))
        {
            CreateDigging(item.Location);
        }
        foreach (var item in _cache.Find("Item", transform.position, _horizontalTiles / 2, true))
        {
            //print("###Inside LoadCaches: item: " + item.ObjectType + item.Location+ item.Content );
            CreateItem(item.Location, Int32.Parse(item.Content));
        }
    }


    void Start() {

        _itemDatabase = ItemDatabase.Instance();
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
        var starter = GameObject.FindObjectOfType<TerrainStarter>();
        if (starter != null)
        {
            Player.position = starter.PreviousPosition;
            Destroy(starter.gameObject);
        }
        RedrawMap();

        if (starter == null)
            SetPlayerlocation();
    }
    
    // Update is called once per frame
    void Update () {
        if (MaxDistanceFromCenter<Vector3.Distance(Player.position,transform.position))
        {
            //Debug.Log("Redraw");
            RedrawMap();
        }
	}

    public ActiveEllementType CreateEllements(Vector3 location)
    {
        var ellement = new GameObject();
        var active = ellement.AddComponent<ActiveEllementType>();
        _ellements.Add(active);
        ellement.transform.position = location;
        var renderer = ellement.AddComponent<SpriteRenderer>();
        var ellementInfo = _availableEllementTypes[RandomHelper.Range(ellement.transform.position, Key, _availableEllementTypes.Count)];
        renderer.sprite = ellementInfo.Tile;
        active.EllementTypeInUse = ellementInfo;

        ellement.name = "Ellement " + ellement.transform.position;
        ellement.transform.parent = transform;
        return active;

    }
    public void CreateDigging(Vector3 location)
    {
        GameObject dig = new GameObject();
        _digs.Add(dig);
        dig.transform.position = location;
        var renderer = dig.AddComponent<SpriteRenderer>();
        renderer.sprite = Dig;
        dig.name = "Dig " + dig.transform.position;
        dig.transform.parent = transform;
    }

    public void CreateItem(Vector3 location, int itemId)
    {
        GameObject Item = new GameObject();
        var active = Item.AddComponent<ActiveItemType>();
        active.ItemTypeInUse = _itemDatabase.FindItem(itemId);
        active.Location = location;
        _items.Add(active);
        Item.transform.position = location;
        var renderer = Item.AddComponent<SpriteRenderer>();
        renderer.sprite = active.ItemTypeInUse.GetSprite();
        Item.name = "Item " + Item.transform.position;
        Item.transform.parent = transform;
    }
    
    public ActiveEllementType GetEllement(Vector2 mapPos)
    {
        foreach (var ellement in _ellements)
        {
            var bLoc = ellement.transform.position;
            //Mappos inbetween the ellement 
            if (mapPos.x == bLoc.x && mapPos.y == bLoc.y)
                return ellement;
        }
        return null;
    }

    public ActiveEllementType GetEllement(float x, float y)
    {
        foreach (var ellement in _ellements)
        {
            var bLoc = ellement.transform.position;
            //Mappos inbetween the ellement 
            if (Math.Abs(bLoc.x - x) < 1 && Math.Abs(bLoc.y - y) < 1)
                return ellement;

        }
        return null;
    }
    
    public ActiveItemType GetDropItem(float x, float y)
    {
        foreach (var item in _items)
        {
            var bLoc = item.transform.position;
            //Mappos inbetween the ellement 
            if (Math.Abs(bLoc.x - x) < 1 && Math.Abs(bLoc.y - y) < 1)
                return item;
        }
        return null;
    }

    internal void DistroyItem(ActiveItemType item)
    {
        _items.Remove(item);
        Destroy(item.gameObject);
    }

    internal void DistroyEllement(ActiveEllementType element)
    {
        _ellements.Remove(element);
        Destroy(element.gameObject);
    }


    //Start up sets ups
    void SetEllements(char[,] charMap, Vector2 location)
    {
        int ellementMass = RandomHelper.Range(location.x, location.y, Key, 14 * 14) / 5;
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
    private void SetAvailableMarketTerrains()
    {
        _availableTerrainTypes.Clear();
        for (int i = 0; i < TerrainTypes.Length; i++)
            if (TerrainTypes[i].IsActive)
                _availableTerrainTypes.Add(TerrainTypes[i]);
    }
    private void SetAvailableMarketEllements(TerrainIns.TerrainType type)
    {
        _availableEllementTypes.Clear();
        for (int i = 0; i < EllementTypes.Length; i++)
            if (EllementTypes[i].FavouriteTerrainTypes == type && EllementTypes[i].IsActive)
                _availableEllementTypes.Add(EllementTypes[i]);
    }

    private void SetPlayerlocation()
    {
        Marker marker = Marker.Closest(_markers, new Vector2(Player.position.x, Player.position.y), Key);

        //print("###Inside SetPlayerlocation: " + Player.position + marker.Terrain.Name);
        //int i = 0;
        if (!marker.Terrain.Walkable)
            foreach (var newMarker in _markers)
            {
                //print("###Inside SetPlayerlocation(for"+ i++ + "): " + newMarker.Terrain.Name);
                if (newMarker.Terrain.Walkable)
                {
                    marker = newMarker;
                    break;
                }
            }
        for (int r = 0; r < 8; r++) //Radiate from the center to find closest empty open space in the middle 
            for (int x = 8 - r; x < 8 + r; x++)
                for (int y = 8 - r; y < 8 + r; y++)
                    if (marker.CharMap[x, y] == 'E')
                    {
                        Vector2 rightCornerLocation = new Vector2(marker.Location.x - 8, marker.Location.y - 8);
                        Player.position = new Vector3(
                            rightCornerLocation.x + x,
                            rightCornerLocation.y + y,
                            0);
                        //print("###Inside SetPlayerlocation: " + Player.position + marker.Terrain.Name);
                        return;
                    }
    }


    

}
