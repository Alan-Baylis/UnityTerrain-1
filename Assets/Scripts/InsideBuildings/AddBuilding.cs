using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBuilding : MonoBehaviour {


    public TerrainManager Terrain_Manager;
    public KeyCode KeyToCreateBuilding = KeyCode.B;
    public KeyCode KeyToCreateSpecialBuilding = KeyCode.N;

    private Cache _cache;

	// Use this for initialization
	void Start () {
        _cache = Cache.Get();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyToCreateBuilding))
        {
            var pos = transform.position;
            pos.x = (int)pos.x ;
            pos.y = (int)pos.y +2;
            Terrain_Manager.CreateBuilding(pos);

            _cache.Add(new CacheContent ()
                {
                    Location = pos,
                    ObjectType = "Building"
                }
            );
        }
	    if (Input.GetKeyDown(KeyToCreateSpecialBuilding))
	    {
	        var pos = transform.position;
	        pos.x = (int)pos.x;
	        pos.y = (int)pos.y + 2;
	        Terrain_Manager.CreateSpecial(pos);

	        _cache.Add(new CacheContent()
	            {
	                Location = pos,
	                ObjectType = "SpecialBuilding"
	            }
	        );
	    }
    }
}
