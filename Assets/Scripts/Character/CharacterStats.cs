using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    private CharacterManager _characterManager;

    private CharacterSetting _settings;
    //public GUISkin Skin;
    //public Vector2 StatsLocation = Vector2.zero;
    //public KeyCode KeyToShowStats = KeyCode.S;
    //public bool ShowStatsButton = false;

    private BarHandler _health;
    private BarHandler _mana;
    private BarHandler _energy;
    private BarHandler _experience;
    private ContainerValueHandler _coin;
    private ContainerValueHandler _gem;
    private int _maxExperience;

    //private bool _showStats = false;
    //private bool _showTooltip = false;
    //private string _tooltip = "";

    // Use this for initialization
    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        _settings = _characterManager.CharacterSetting;
    }

    void Start ()
    {
        if (_health == null)
            _health = GameObject.FindGameObjectWithTag("Health").GetComponent<BarHandler>();
        if (_mana == null)
            _mana = GameObject.FindGameObjectWithTag("Mana").GetComponent<BarHandler>();
        if (_energy == null)
            _energy = GameObject.FindGameObjectWithTag("Energy").GetComponent<BarHandler>();
        if (_experience == null)
            _experience = GameObject.FindGameObjectWithTag("Experience").GetComponent<BarHandler>();
        if (_coin == null)
            _coin = GameObject.FindGameObjectWithTag("Coin").GetComponent<ContainerValueHandler>();
        if (_gem == null)
            _gem = GameObject.FindGameObjectWithTag("Gem").GetComponent<ContainerValueHandler>();



        _health.UpdateValues(_settings.Health, _settings.MaxHealth);
        _mana.UpdateValues(_settings.Mana, _settings.MaxMana);
        _energy.UpdateValues(_settings.Energy, _settings.MaxEnergy);
        _maxExperience = (int) Math.Pow( (_settings.Level+1) * 100, 2);
        _experience.UpdateValues(_settings.Experience, _maxExperience, _settings.Level);

        _coin.UpdateValue(_settings.Coin);
        _gem.UpdateValue(_settings.Gem);
    }
	
	// Update is called once per frame
	void Update () {
        if (_settings.Updated)
        {
            _health.UpdateValues(_settings.Health, _settings.MaxHealth);
            _mana.UpdateValues(_settings.Mana, _settings.MaxMana);
            _energy.UpdateValues(_settings.Energy, _settings.MaxEnergy);
            //todo: set exp 0 when it reach the max probabaly not in here ormaybe here 
            _experience.UpdateValues(_settings.Experience, _maxExperience, _settings.Level);

            _coin.UpdateValue(_settings.Coin);
            _gem.UpdateValue(_settings.Gem);

            _settings.Updated = false;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            _coin.UpdateValue(2000);
            _health.FillAmount += 10;
	        _mana.FillAmount += 5;
	        _energy.FillAmount += 15;
        }
	    if (Input.GetKeyDown(KeyCode.J))
	    {
	        _coin.UpdateValue(1000);
            _health.FillAmount -= 10;
	        _mana.FillAmount -= 5;
	        _energy.FillAmount -= 15;
        }
    }
}
