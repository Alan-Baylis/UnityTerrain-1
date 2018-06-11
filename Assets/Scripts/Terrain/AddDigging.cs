using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDigging : MonoBehaviour {


    public TerrainManager Terrain_Manager;
    public KeyCode KeyToDig = KeyCode.B;

    private Cache _cache;

	// Use this for initialization
	void Start () {
        _cache = Cache.Get();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyToDig))
        {
            var pos = transform.position;
            pos.x = (int)pos.x ;
            pos.y = (int)pos.y +2;
            //TODO
            print("New digidy");
            //Adddigging(pos);

            //_cache.Add(new CacheContent ()
            //    {
            //        Location = pos,
            //        ObjectType = "Building"
            //    }
            //);
        }
    }
}
