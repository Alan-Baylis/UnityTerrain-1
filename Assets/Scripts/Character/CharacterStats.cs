using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    //public GUISkin Skin;
    //public Vector2 StatsLocation = Vector2.zero;
    //public KeyCode KeyToShowStats = KeyCode.S;
    //public bool ShowStatsButton = false;

    private BarHandler _health;
    private BarHandler _mana;

    //private bool _showStats = false;
    //private bool _showTooltip = false;
    //private string _tooltip = "";

    // Use this for initialization
    void Start ()
    {
        if (_health == null)
            _health = GameObject.FindGameObjectWithTag("Health").GetComponent<BarHandler>();
        if (_mana == null)
            _mana = GameObject.FindGameObjectWithTag("Mana").GetComponent<BarHandler>();

        _health.FillAmount = 100;
        _health.MaxValue = 100;
        _mana.FillAmount = 100;
        _mana.MaxValue = 100;
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.H))
	    {
	        _health.FillAmount += 10;
	        _mana.FillAmount += 5;
        }
	    if (Input.GetKeyDown(KeyCode.J))
	    {
	        _health.FillAmount -= 10;
	        _mana.FillAmount -= 5;
        }
    }
}
