using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    public Sprite TilableImage;
    public Sprite[] Sprites;
    public int HorizontalTiles = 25;
    public int VerticalTiles = 25;
    public int Key = 1;
    public Transform Player;
    public float MaxDistanceFromCenter = 7;


    private SpriteRenderer[,] _renderers;

    // Use this for initialization
    public Sprite SelectRandomSprite(int x,int y)
    {
        return Sprites[RandomHelper.Range(x,y,Key, Sprites.Length)];
    }



    void RedrawMap()
    {
        transform.position = new Vector3( (int)Player.position.x, (int)Player.position.y,Player.position.z);
        for (int x = 0; x < HorizontalTiles; x++)
        {
            for (int y = 0; y < VerticalTiles; y++)
            {
                var spriteRenderer = _renderers[x, y];
                spriteRenderer.sprite = SelectRandomSprite(
                        (int)transform.position.x + x,
                        (int)transform.position.y + y);
            }
        }
    }

    void Start () {
        var offset = new Vector3(0 - HorizontalTiles / 2, 0 - VerticalTiles/2, 0);
        _renderers = new SpriteRenderer[HorizontalTiles, VerticalTiles];

        for (int x = 0; x < HorizontalTiles; x++)
        {
            for (int y = 0; y < VerticalTiles; y++)
            {
                var tile = new GameObject();
                tile.transform.position = new Vector3(x, y, 0) + offset;
                _renderers[x,y]= tile.AddComponent<SpriteRenderer>();
                tile.name = "Terrain " + tile.transform.position;
                tile.transform.parent = transform;
            }
        }
        RedrawMap();

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
