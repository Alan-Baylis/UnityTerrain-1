using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TerrainConsume : MonoBehaviour {


    public TerrainManager Terrain_Manager;
    public KeyCode KeyToConsume = KeyCode.C;

    private Cache _cache;

	// Use this for initialization
	void Start () {
        _cache = Cache.Get();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyToConsume))
        {
            var pos = transform.position;
            //So the new object will be n top of terrain 
            pos.z = 0.01f;

            var currentElement = Terrain_Manager.GetEllement(pos.x, pos.y);
            if (currentElement != null)
            {
                if (currentElement.EllementTypeInUse.IsDistroyable)
                {
                    Terrain_Manager.DistroyEllement(pos);
                    _cache.Add(new CacheContent()
                        {
                            Location = transform.position,
                            ObjectType = "Player"
                        }
                    );
                }
                return;
            }

            TerrainIns currentTerrain = Terrain_Manager.SelectTerrain(pos.x, pos.y);
            if (currentTerrain != null)
            {
                if (currentTerrain.IsDiggable)
                {
                    Terrain_Manager.CreateDigging(pos);
                    _cache.Add(new CacheContent()
                    {
                        Location = pos,
                        ObjectType = "Digging"
                    }
                    );
                }

                return;
            }





        }
    }
}
