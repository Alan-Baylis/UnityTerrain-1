using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

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

	public void Add(CacheContent item)
    {
        _content.Add(item);
    }


    public IEnumerable<CacheContent> Find(
                        string objectType, 
                        Vector3 near, // Location
                        float radius) //in what range we need to look for the object 
    {
        foreach (var c in _content)
        {
            if (c.ObjectType != objectType)
                continue;
            if (Vector3.Distance(near, c.Location) < radius)
                //lets the loop continue and return all the objects in that radius
                yield return c;
        }
    }

    //target has to be seralizable
    public string Serialize(object target)
    {
        var serializer = new XmlSerializer(target.GetType());
        using (var writer = new StringWriter())
        {
            serializer.Serialize(writer, target);
            return writer.ToString();
        }
    }

    public T Deserialize<T> (string content)
    {
        var serializer = new XmlSerializer(typeof(T));
        using (var stream = new StringReader(content))
        {
            return (T) serializer.Deserialize(stream);
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


}
