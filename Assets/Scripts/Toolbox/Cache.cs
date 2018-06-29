using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;

public class Cache : MonoBehaviour {

    private List<CacheContent> _content = new List<CacheContent>();

    public static Cache Get()
    {
        var cache = GameObject.FindObjectOfType<Cache>();
        if (cache==null)
        {
            GameObject go = new GameObject();
            go.name = "Cache";
            GameObject.DontDestroyOnLoad(go);
            cache = go.AddComponent<Cache>();
        }
        return cache;
    }

    public string Serialize(object target)
    {
        var serializer = new XmlSerializer(target.GetType());
        using (var writer = new StringWriter())
        {
            serializer.Serialize(writer, target);
            return writer.ToString();
        }
    }

    public T Deserialize<T>(string content)
    {
        var serializer = new XmlSerializer(typeof(T));
        using (var stream = new StringReader(content))
        {
            return (T)serializer.Deserialize(stream);
        }
    }
    public string SerializeCache()
    {
        List<string> cacheArray = new List<string>();
        foreach (var item in _content)
        {
            cacheArray.Add(Serialize(item));
        }
        return Serialize(cacheArray.ToArray());
    }
    public void DeserializeCache(string content)
    {
        string[] serList = Deserialize<string[]>(content);
        _content.Clear();
        foreach (var item in serList)
        {
            _content.Add(Deserialize<CacheContent>(item));
        }
    }

    public void Add(CacheContent item)
    {
        _content.Add(item);
    }

    public void Print()
    {
        print("Cache count: "+_content.Count);
        foreach (var c in _content)
            print("Cache count: " + c.ObjectType + c.Location + c.Content);
    }

    public IEnumerable<CacheContent> Find(
                        string objectType, 
                        Vector3 near, // Location
                        float radius, //in what range we need to look for the object 
                        bool destroy //distroy the rest if true
                            )
    {
        List<CacheContent> toDelete = new List<CacheContent>();
        foreach (var c in _content)
        {
            if (c.ObjectType != objectType)
                continue;
            if (Vector3.Distance(near, c.Location) < radius)
                //lets the loop continue and return all the objects in that radius
                yield return c;
            else if (destroy)
            {
                toDelete.Add(c);
                //print("###Inside Find to delete" + c.ObjectType + c.Location+ Vector3.Distance(near, c.Location)+ near);
            }
        }
        if (destroy)
            foreach (var c in toDelete)
                _content.Remove(c);
    }


    public IEnumerable<CacheContent> Find(
        string objectType,
        bool destroy //distroy the rest if true
        )
    {
        List<CacheContent> toDelete = new List<CacheContent>();
        foreach (var c in _content)
        {
            if (c.ObjectType != objectType)
                continue;
            yield return c;
            toDelete.Add(c);
        }
        if (destroy)
            foreach (var c in toDelete)
                _content.Remove(c);
    }

    public bool Find(string objectType, Vector3 location)
    {
        foreach (var c in _content)
        {
            if (c.ObjectType != objectType)
                continue;
            if (location == c.Location)
                return true;
        }
        return false;
    }
    //target has to be seralizable
 

    public void SyncItems(string objectType, List<ActiveItemType> items)
    {
        List<CacheContent> toDelete = new List<CacheContent>();
        bool objectExists = false;
        foreach (var c in _content)
        {
            if (c.ObjectType != objectType)
                continue;
            objectExists = false;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ItemTypeInUse.Id == Int32.Parse(c.Content)
                    && items[i].Location == c.Location)
                {
                    objectExists = true;
                    break;
                }
            }
            if (!objectExists)
                toDelete.Add(c);
        }
        foreach (var c in toDelete)
            _content.Remove(c);
    }


}
