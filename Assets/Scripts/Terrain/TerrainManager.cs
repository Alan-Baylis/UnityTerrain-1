using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    public float EllementChance = 0.70f;
    public static int SceneIdForInsideBuilding = 1;
    public static int Key = 1;
    public Transform Player;
    public float MaxDistanceFromCenter = 7;
    public Vector2 MapOffset ;
    public Sprite Dig;


    private static TerrainManager _terrainManager;
    private InventoryHandler _inv;
    private ItemDatabase _itemDatabase;
    private TerrainDatabase _terrainDatabase;
    private CharacterDatabase _characterDatabase;

    private int _horizontalTiles = 25;

    private int _baseSortIndex = 0;
    private List<TerrainIns> _terrainTypes;
    private List<TerrainIns> _availableTerrainTypes = new List<TerrainIns>();

    private List<EllementIns> _ellementTypes;
    private List<ActiveEllementType> _ellements = new List<ActiveEllementType>();
    private List<EllementIns> _availableEllementTypes = new List<EllementIns>();

    private List<Character> _monsterTypes;
    private List<ActiveMonsterType> _monsters = new List<ActiveMonsterType>();
    private List<Character> _availableCharacterTypes = new List<Character>();

    
    private int _verticalTiles = 25;
    private SpriteRenderer[,] _renderers;
    private IEnumerable<Marker> _markers;
    private List<ActiveItemType> _activeItems = new List<ActiveItemType>();
    private List<GameObject> _digs = new List<GameObject>();

    private Cache _cache;

    [SerializeField]
    private GameObject MonsterObj;

    void Start()
    {
        _itemDatabase = ItemDatabase.Instance();
        _inv = InventoryHandler.Instance();
        _terrainDatabase = TerrainDatabase.Instance();
        _characterDatabase = CharacterDatabase.Instance();
        _cache = Cache.Get();
        _terrainManager = TerrainManager.Instance();
        _terrainTypes = _terrainDatabase._terrains;
        _ellementTypes = _terrainDatabase._ellements;
        _monsterTypes = _characterDatabase._characters;

        print("_terrainTypes count = " + _terrainTypes.Count);
        print("_ellementTypes count = " + _ellementTypes.Count);
        print("_monsterTypes count = " + _monsterTypes.Count);

        //todo: make the player private
        //Player = GameObject.FindGameObjectWithTag("PlayerCamera").transform;

        var offset = new Vector3(0 - _horizontalTiles / 2, 0 - _verticalTiles / 2, 0);
        _renderers = new SpriteRenderer[_horizontalTiles, _verticalTiles];

        for (int x = 0; x < _horizontalTiles; x++)
        {
            for (int y = 0; y < _verticalTiles; y++)
            {
                var tile = new GameObject();
                tile.transform.position = new Vector3(x, y, 0) + offset;
                var renderer = _renderers[x, y] = tile.AddComponent<SpriteRenderer>();
                renderer.sortingOrder = _baseSortIndex;
                tile.name = "Terrain " + tile.transform.position;
                tile.transform.parent = transform;
            }
        }
        SetAvailableMarketTerrains();
        var starter = GameObject.FindObjectOfType<TerrainStarter>();
        if (starter != null)
        {
            Player.position = starter.PreviousPosition;
            _inv.ShowInventory = starter.ShowInventory;
            Destroy(starter.gameObject);
        }
        RedrawMap();

        if (starter == null)
            SetPlayerlocation();
    }



    void Update()
    {
        if (MaxDistanceFromCenter < Vector3.Distance(Player.position, transform.position))
        {
            //Debug.Log("Redraw");
            RedrawMap();
        }
    }
    public Vector2 WorldToMapPosition(Vector3 worldPosition)
    {
        if (worldPosition.x < 0) worldPosition.x--;
        if (worldPosition.y < 0) worldPosition.y--;        
        return new Vector2((int)(worldPosition.x + MapOffset.x), (int)(worldPosition.y + MapOffset.y));
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
                if (terrain.Tiles > 0)
                    spriteRenderer.sprite = terrain.GetTile(
                        offset.x + x,
                        offset.y + y,
                        Key);
                var animator = spriteRenderer.gameObject.GetComponent<Animator>();
                if (terrain.AnimationControler > 0)
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
        CacheMonsters();
        _ellements.ForEach(x => Destroy(x.gameObject));
        _ellements.Clear();
        _monsters.ForEach(x => Destroy(x.gameObject));
        _monsters.Clear();
        _activeItems.ForEach(x => Destroy(x.gameObject));
        _activeItems.Clear();
        _digs.ForEach(x => Destroy(x.gameObject));
        _digs.Clear();
        foreach (var marker in _markers)
        {
            //Start up sets ups
            if (marker.HasEllement)
            {
                SetAvailableMarketEllements(marker.Terrain);
                LoadEllements(marker);
            }
            if (marker.HasMonster)
            {
                SetAvailableMarketMonsters(marker.Terrain);
                LoadMonsters(marker);
            }
        }
        //If it has been consumed recently delete them
        foreach (var element in _cache.Find("VacantElement", transform.position, _horizontalTiles / 2, true))
        {
            var currentElement = GetEllement(element.Location.x, element.Location.y);
            if (currentElement!=null)
                DistroyEllement(currentElement,false);
        }

        UnCacheMonsters(transform.position, _horizontalTiles / 2);
        LoadCaches();
    }


    private void LoadCaches()
    {
        _cache = Cache.Get();
        foreach (var item in _cache.Find("Digging", transform.position, _horizontalTiles / 2,true))
            CreateDigging(item.Location,false);
        foreach (var item in _cache.Find("Item", transform.position, _horizontalTiles / 2, true))
            CreateItem(item.Location, Int32.Parse(item.Content));
    }

    //Terrains
    private void SetAvailableMarketTerrains()
    {
        _availableTerrainTypes.Clear();
        for (int i = 0; i < _terrainTypes.Count; i++)
            if (_terrainTypes[i].IsEnabled)
                _availableTerrainTypes.Add(_terrainTypes[i]);
    }
    public TerrainIns SelectTerrain(float x, float y)
    {
        return Marker.Closest(_markers, new Vector2(x, y), Key).Terrain;
    }
    //Ellements
    private void SetAvailableMarketEllements(TerrainIns terrain)
    {
        _availableEllementTypes.Clear();
        if (terrain.HasElement)
            for (int i = 0; i < _ellementTypes.Count; i++)
                if (_ellementTypes[i].FavouriteTerrainTypes == terrain.Type && _ellementTypes[i].IsEnabled)
                    _availableEllementTypes.Add(_ellementTypes[i]);
    }
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
    void LoadEllements(Marker marker)
    {
        Vector2 rightCornerLocation = new Vector2(marker.Location.x - 8, marker.Location.y - 8);
        if (marker.HasEllement)
            SetEllements(marker.CharMap, marker.Location);
        else
            return;
        //Set up the map based on marker.CharMap

        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                //Skipp Empty places
                if (marker.CharMap[x, y] == 'E' || marker.CharMap[x, y] == 'M')
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
        var ellement = new GameObject();
        var active = ellement.AddComponent<ActiveEllementType>();
        _ellements.Add(active);
        ellement.transform.position = location;
        var renderer = ellement.AddComponent<SpriteRenderer>();
        var ellementInfo = _availableEllementTypes[RandomHelper.Range(ellement.transform.position, Key, _availableEllementTypes.Count)];
        renderer.sprite = ellementInfo.GetSprite();
        renderer.sortingOrder = _baseSortIndex+  5;
        active.EllementTypeInUse = ellementInfo;

        ellement.name = "Ellement " + ellement.transform.position;
        ellement.transform.parent = transform;
        return active;
    }
    public ActiveEllementType GetEllement(Vector2 mapPos)
    {
        foreach (var ellement in _ellements)
        {
            var bLoc = ellement.transform.position;
            //Mappos inbetween the ellement 
            //if (mapPos.x == bLoc.x && mapPos.y == bLoc.y)
                if (Math.Abs(bLoc.x - mapPos.x) < 1 && Math.Abs(bLoc.y - mapPos.y) < 1)
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
    internal bool DistroyEllement(ActiveEllementType element, bool useTool)
    {
        if (useTool)
            if (!_inv.ElementToolUse(element.EllementTypeInUse))
                return false;
        _ellements.Remove(element);
        Destroy(element.gameObject);
        return true;
    }
    //Monsters
    private void SetAvailableMarketMonsters(TerrainIns terrain)
    {
        _availableCharacterTypes.Clear();
        if (terrain.HasMonster)
            for (int i = 0; i < _monsterTypes.Count; i++)
                if (_monsterTypes[i].FavouriteTerrainTypes == terrain.Type && _monsterTypes[i].IsEnabled)
                    _availableCharacterTypes.Add(_monsterTypes[i]);
    }
    private void SetMonsters(char[,] charMap, Vector2 location)
    {
        int monstersMass = RandomHelper.Range(location.x, location.y, Key, 14 * 14) /30;
        for (int i = 0; i < monstersMass; i++)
        {
            //make x,y between 2-14 leave the boundres clear 
            int x = RandomHelper.Range(
                        location.x - i,
                        location.y,
                        Key,
                        12
                    ) + 2;
            int y = RandomHelper.Range(
                        location.x,
                        location.y - i,
                        Key,
                        12
                    ) + 2;
            if (charMap[x, y] != 'E')
                continue;
            charMap[x, y] = 'M';
        }
    }
    void LoadMonsters(Marker marker)
    {
        Vector2 rightCornerLocation = new Vector2(marker.Location.x - 8, marker.Location.y - 8);

        if (marker.HasMonster)
            SetMonsters(marker.CharMap, marker.Location);
        else 
            return;
        //Set up the map based on marker.CharMap

        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                //Skipp Empty places
                if (marker.CharMap[x, y] == 'E' || marker.CharMap[x, y] == 'B')
                    continue;
                var bLoc = new Vector3(
                    rightCornerLocation.x + x,
                    rightCornerLocation.y + y,
                    0.01f);
                //todo: distroy anything that is close by player
                //So player doesn't land on a ellement
                if (bLoc.x == 0 && bLoc.y == 0) continue;
                if (marker.CharMap[x, y] == 'M')
                    CreateMonsters(bLoc);
            }
        }
    }
    private ActiveMonsterType CreateMonsters(Vector3 location)
    {
        GameObject monster = Instantiate(MonsterObj);
        monster.transform.position = location;
        monster.transform.parent = transform;
        monster.name = "Monster " + monster.transform.position;

        var active = monster.GetComponent<ActiveMonsterType>();
        var spriteRenderer = monster.GetComponent<SpriteRenderer>();
        active.OrgLocation = location;
        active.Alive = true;
        _monsters.Add(active);

        var monsterCharacter = _availableCharacterTypes[RandomHelper.Range(monster.transform.position, Key, _availableEllementTypes.Count)];

        spriteRenderer.sprite = monsterCharacter.GetSprite();
        if (monsterCharacter.MoveT == Character.CharacterType.Fly)
            spriteRenderer.sortingOrder = _baseSortIndex +  6;
        else
            spriteRenderer.sortingOrder = _baseSortIndex +  3;
        if (monsterCharacter.IsAnimated)
        {
            var animator = monster.GetComponent<Animator>();
            animator.runtimeAnimatorController = monsterCharacter.GetAnimator();
            animator.speed = monsterCharacter.MoveT == Character.CharacterType.Fly ? 1 : 0;
        }
        active.MonsterType = _characterDatabase.GenerateMonster(monsterCharacter);

        return active;
    }
    internal ActiveMonsterType GetMonster(Vector2 mapPos, int radius)
    {
        return GetMonster(mapPos.x, mapPos.y, radius);
    }
    private ActiveMonsterType GetMonster(float x, float y, int radius)
    {
        foreach (var monster in _monsters)
        {
            var bLoc = monster.transform.position;
            //Mappos in between the monster 
            if (Math.Abs(bLoc.x - x) < radius && Math.Abs(bLoc.y - y) < radius)
                return monster;
        }
        return null;
    }
    internal bool DistroyMonster(ActiveMonsterType monster, bool drop)
    {
        if (drop)
            print("Drop Monster");
        print("Killing Monster " + monster.OrgLocation);
        _monsters.Remove(monster);
        Destroy(monster.gameObject);
        return true;
    }

    private void CacheMonsters()
    {
        foreach (var monster in _monsters)
        {
            var pos = monster.OrgLocation;
            if (!monster.Alive)
            {
                print("Added to DeadMonster cache"+ pos);
                _cache.Add(new CacheContent()
                    {
                        Location = pos,
                        ObjectType = "DeadMonster"
                    }
                );
            }else if (monster.Moved)
            {
                print("Added to MovedMonster cache" + pos + "New location ="+ monster.transform.position);
                _cache.Add(new CacheContent()
                    {
                        Location = pos ,
                        Content = monster.transform.position.ToString(),
                        ObjectType = "MovedMonster"
                    }
                );
            }
        }
    }

    private void UnCacheMonsters(Vector3 near, float radius)
    {
        //print("UnCacheMonsters(Vector3 near, float radius) " + near+ radius);
        foreach (var monster in _cache.Find("DeadMonster", near, radius, true))
        {
            print("out from DeadMonster cache" + monster.Location);
            var currentMonster = GetMonster(monster.Location.x, monster.Location.y,1);
            if (currentMonster != null)
                DistroyMonster(currentMonster, false);
        }
        foreach (var monster in _cache.Find("MovedMonster",  true))
        {
            new WaitForSeconds(5);
            print("1-out from MovedMonster cache" + monster.Location + "New location =" + monster.Content);
            var currentMonster = GetMonster(monster.Location.x, monster.Location.y, 1);
            if (currentMonster != null)
            {
                Vector3 newPos = StringToVector3(monster.Content);
                print("2-out from MovedMonster cache" + monster.Location + "New location =" + newPos);
                currentMonster.transform.position = currentMonster.NewLocation = newPos;
                print("Moving Monster " + currentMonster.OrgLocation + " To " + currentMonster.NewLocation);
            }
        }
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    //Digging
    public bool CreateDigging(Vector3 location,bool useTool)
    {
        if (useTool)
            if (!_inv.ElementToolUse())
                return false;
        GameObject dig = new GameObject();
        _digs.Add(dig);
        dig.transform.position = location;
        var renderer = dig.AddComponent<SpriteRenderer>();
        renderer.sprite = Dig;
        renderer.sortingOrder = _baseSortIndex + 1;
        dig.name = "Dig " + dig.transform.position;
        dig.transform.parent = transform;
        return true;

    }
    //Items
    public void CreateItem(Vector3 location, int itemId)
    {
        GameObject item = new GameObject();
        var active = item.AddComponent<ActiveItemType>();
        active.ItemTypeInUse = _itemDatabase.FindItem(itemId);
        location.z -= 0.001f;
        active.Location = location;
        _activeItems.Add(active);
        item.transform.position = location;
        item.transform.localScale += new Vector3(0.1f, 0, 0);
        var renderer = item.AddComponent<SpriteRenderer>();
        renderer.sprite = active.ItemTypeInUse.GetSprite();
        renderer.sortingOrder =  _baseSortIndex+  2;
        item.name = active.ItemTypeInUse.Name + item.transform.position;
        item.transform.parent = transform;
    }
    public ActiveItemType GetDropItem(float x, float y)
    {
        //foreach (var item in _activeItems)
        //    print(item.name+item.ItemTypeInUse.Name+ item.transform.position + " x= " + x + " y= " + y + " " + item.Location);
        foreach (var item in _activeItems)
        {
            var bLoc = item.transform.position;
            //Mappos inbetween the ellement 
            if (Math.Abs(bLoc.x - x) < 1 && Math.Abs(bLoc.y - y) < 1)
            {
                return item;
            }
        }
        return null;
    }
    internal void DistroyItem(ActiveItemType item)
    {
        _activeItems.Remove(item);
        Destroy(item.gameObject);
    }
    //Player
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

    public static TerrainManager Instance()
    {
        if (!_terrainManager)
        {
            _terrainManager = FindObjectOfType(typeof(TerrainManager)) as TerrainManager;
            if (!_terrainManager)
                Debug.LogError("There needs to be one active TerrainManager script on a GameObject in your scene.");
        }
        return _terrainManager;
    }

}
