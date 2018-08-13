using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class TerrainDatabase : MonoBehaviour {

    private static TerrainDatabase _terrainDatabase;


    public List<EllementIns> _ellements = new List<EllementIns>();
    public List<TerrainIns> _terrains = new List<TerrainIns>();


    void Awake()
    {
        _terrainDatabase = TerrainDatabase.Instance();
        LoadTerrains();
        LoadEllements();
    }


    private void LoadTerrains()
    {
        //Empty the Recipes DB
        _terrains.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Terrain.xml");
        //Read the Recipes from Recipe.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<TerrainIns>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _terrains = (List<TerrainIns>)serializer.Deserialize(fs);
        fs.Close();
    }

    private void LoadEllements()
    {
        //Empty the Recipes DB
        _ellements.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Ellement.xml");
        //Read the Recipes from Recipe.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<EllementIns>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _ellements = (List<EllementIns>)serializer.Deserialize(fs);
        fs.Close();
    }


    public static TerrainDatabase Instance()
    {
        if (!_terrainDatabase)
        {
            _terrainDatabase = FindObjectOfType(typeof(TerrainDatabase)) as TerrainDatabase;
            if (!_terrainDatabase)
                Debug.LogError("There needs to be one active TerrainDatabase script on a GameObject in your scene.");
        }
        return _terrainDatabase;
    }
}
