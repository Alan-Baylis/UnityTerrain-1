using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CacheContent  {

    public float X = 0;
    public float Y = 0;
    public float Z = 0;

    public Vector3 Location
    {
        get
        {
            return new Vector3(X,Y,Z);
        }
        set
        {
            X = value.x;
            Y = value.y;
            Z = value.z;
        }
    }

    public string ObjectType { get; set; }

    public string Content { get; set; }

}
