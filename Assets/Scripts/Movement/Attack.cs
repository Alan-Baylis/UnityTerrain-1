using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    //todo: make this private 
    private TerrainManager _terrainManager;
    public ActiveMonsterType Monster;
    // Use this for initialization
    void Start ()
    {
        _terrainManager = TerrainManager.Instance();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    InLineOfSight();
        AttachTarget();
	}

    private void AttachTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Monster = _terrainManager.GetMonster(Camera.main.ScreenToWorldPoint(Input.mousePosition),1);
        }
        if (Monster != null)
        {
            var myPos = transform.position;
            var direction = (Monster.transform.position - myPos).normalized;
            var rayCast = Physics2D.Raycast(transform.position, direction, 5);
            if (rayCast.collider == null)
            {
                Debug.DrawRay(transform.position, direction, Color.blue);
            }
        }
    }

    private void InLineOfSight()
    {
        var myPos = transform.position;
        var monster = _terrainManager.GetMonster(myPos,3);
        //You are in the line of saw a monster 
        if (monster!= null)
            monster.SawTarget = true;
    }
}
