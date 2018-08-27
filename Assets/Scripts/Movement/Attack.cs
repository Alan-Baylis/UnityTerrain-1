using System.Collections; using System.Collections.Generic; using UnityEngine;  public class Attack : MonoBehaviour {      //todo: make this private      private TerrainManager _terrainManager;     private CharacterManager _characterManager;     private InventoryHandler _inv;
    private ModalPanel _modalPanel;     private Cache _cache;
    private bool _monsterWarned;


    private float nextActionTime = 0.0f;

    // Use this for initialization
    void Start ()     {         _characterManager = CharacterManager.Instance();         _terrainManager = TerrainManager.Instance();         _inv = InventoryHandler.Instance();
        _modalPanel = ModalPanel.Instance();         _cache = Cache.Get();     } 	 	// Update is called once per frame 	void Update () 	{ 	    if (Input.GetMouseButtonDown(0))
            AttackTarget();         InLineOfSight(); 	}      void FixedUpdate()
    {
        float period = 10f;
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            print("Executed attack time =" + Time.time);
            _monsterWarned = false;
        }
    }      private void AttackTarget()     {         var monster = _terrainManager.GetMonster(Camera.main.ScreenToWorldPoint(Input.mousePosition),1);         if (monster != null)         {             var myPos = transform.position;             var direction = (monster.transform.position - myPos).normalized;             var attackRange = _characterManager.Character.AttackR == Character.AttackRange.Range ? 3 : 1;             var rayCast = Physics2D.Raycast(transform.position, direction, attackRange);             if (rayCast.collider == null)             {                 print("Monster "+ monster.MonsterType.Level + "Got hit: "+ monster.MonsterType.Health);
                var attAmount = _characterManager.CharacterSetting.AbilityAttack + _characterManager.CharacterSetting.MagicAttack + _characterManager.CharacterSetting.PoisonAttack;
                if (!_inv.UseEnergy((int) attAmount))
                {
                    _inv.PrintMessage("YEL: Not enough enegry");
                    return;
                }
                var dealAtt = RandomHelper.AttackRange(attAmount);                 if (dealAtt > attAmount)
                    print("CRITICAL ATTACK");                 monster.MonsterType.Health -= (int)dealAtt * 50; //Todo: remove this 50                 if (monster.MonsterType.Health <=0)                 {                     monster.Alive = false;                     var pos = monster.OrgLocation;                     _cache.Add(new CacheContent()                         {                             Location = pos,                             ObjectType = "DeadMonster"                         }                     );                     monster.gameObject.SetActive(false);                     _terrainManager.DeadMonster(monster.transform.position, monster.MonsterType.GetCharacter());                 }                 else
                {
                    var healthBar = monster.transform.GetComponentsInChildren<Transform>()[1];
                    healthBar.localScale = new Vector3((float) monster.MonsterType.Health / monster.MonsterType.MaxHealth / 3, healthBar.localScale.y , healthBar.localScale.z);
                }                 Debug.DrawRay(myPos, direction, Color.blue);             }         }     }      private void InLineOfSight()     {         var myPos = transform.position;
        //todo: activate the warning for low energy situatuation 
        //if (!_monsterWarned && !_characterManager.CharacterSetting.Fightmode)
        //{
        //    var monster5 = _terrainManager.GetMonster(myPos, 4);
        //    if (monster5 != null)
        //    {
        //        var attAmount = _characterManager.CharacterSetting.AbilityAttack + _characterManager.CharacterSetting.MagicAttack + _characterManager.CharacterSetting.PoisonAttack;
        //        print()

        //        if (monster5.MonsterType.Health > _characterManager.CharacterSetting.Energy)
        //        {
        //            _modalPanel.Choice("You don't have enough energy to fight this monster, head back", ModalPanel.ModalPanelType.YesCancel, () => { _monsterWarned = true; });
        //        }
        //    }
        //}         var monster3 = _terrainManager.GetMonster(myPos, 3);         //You are in the line of saw a monster          if (monster3!= null)
        {
            _characterManager.CharacterSetting.Fightmode = true;             monster3.SawTarget = true;
        }
        else
            _characterManager.CharacterSetting.Fightmode = false;     } } 