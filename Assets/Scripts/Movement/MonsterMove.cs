using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterMove : MonoBehaviour {
    private TerrainManager _terrainManager;
    private CharacterManager _characterManager;

    private float Speed = 2;
    private int _baseSortIndex = 0;
    
    private Cache _cache;

    private ActiveMonsterType _active;
    private Character _character;

    private SpriteRenderer _renderer;
    private Animator _animator;
    private Sprite _up;
    private Sprite _down;
    private Sprite _right;
    private Sprite _left;

    void Awake()
    {
        _cache = Cache.Get();
        _characterManager = CharacterManager.Instance();
        _terrainManager = TerrainManager.Instance();
    }

    // Use this for initialization
    void Start ()
    {
        _active = transform.GetComponent<ActiveMonsterType>();
        _character = _active.MonsterType.GetCharacter();

        _renderer = transform.GetComponent<SpriteRenderer>();
        _animator = transform.GetComponent<Animator>();

        if (_character.IsAnimated)
            _animator.runtimeAnimatorController = GetMoveAnimation();
        else
        {
            SetMoveSprites();
            _renderer.sprite = _down;
        }
    }

    // Update is called once per frame
    void Update ()
    {   
        if (_active.SawTarget)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            Vector3 targetPos = player.position;
            Vector3 monsterPos = transform.position;

            float dist = Vector3.Distance(targetPos, monsterPos);
            if (dist>1)
            {
                var movement = (targetPos - monsterPos).normalized;
                if (_character.IsAnimated)
                    HandleAnimation(movement);
                else
                    HandleSprite(movement);
                DoMove(movement);
                _active.Moved = true;
            }
            else
            {
                AttackPlayer(_active.MonsterType);
            }

        }
    }

    private void AttackPlayer(MonsterIns monsterType)
    {
        switch (_character.AttackT)
        {
            case Character.AttackType.Strength:
                _characterManager.AddCharacterSetting("Health", -monsterType.AbilityAttack);
                break;
            case Character.AttackType.Magic:
                _characterManager.AddCharacterSetting("Health", -monsterType.MagicAttack);
                break;
            case Character.AttackType.Poison:
                _characterManager.AddCharacterSetting("Health", -monsterType.PoisonAttack);
                break;
        }

    }

    private void DoMove(Vector3 movement)
    {
        var currentSpeed = Speed;
        //Old moving system
        transform.Translate(movement * currentSpeed * Time.deltaTime);
        _renderer.sortingOrder = _baseSortIndex + 7;
    }

    private bool CanMove()
    {
        var currentPos = transform.position;
        var mapPos = _terrainManager.WorldToMapPosition(currentPos);
        var terrain = _terrainManager.SelectTerrain(mapPos.x, mapPos.y);
        if (
            //Terrain Types and charcter type 
            (!terrain.Walkable && _character.MoveT == Character.CharacterType.Walk) ||
            (!terrain.Flyable && _character.MoveT == Character.CharacterType.Fly) ||
            (!terrain.Swimable && _character.MoveT == Character.CharacterType.Swim) 
        )
            return true;
        return false;
    }
    private void HandleAnimation(Vector3 movement)
    {
        if (_character.MoveT != Character.CharacterType.Fly && movement == Vector3.zero)
            _animator.speed = 0;
        else
            _animator.speed = 1;
        if (movement == Vector3.zero)
            return;
        if (movement.x > 0.1f && Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            movement = Vector3.right;
        else if (movement.x < -0.1f && Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            movement = Vector3.left;
        else if (movement.y > 0.1f && Mathf.Abs(movement.y) >= Mathf.Abs(movement.x))
            movement = Vector3.up;
        else if (movement.y < -0.1f && Mathf.Abs(movement.y) >= Mathf.Abs(movement.x))
            movement = Vector3.down;
        _animator.SetFloat("x", movement.x);
        _animator.SetFloat("y", movement.y);
    }

    private void HandleSprite(Vector3 movement)
    {
        if (movement.x > 0.1f && Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            _renderer.sprite = _right;
        if (movement.x < -0.1f && Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            _renderer.sprite = _left;
        if (movement.y > 0.1f && Mathf.Abs(movement.y) >= Mathf.Abs(movement.x))
            _renderer.sprite = _up;
        if (movement.y < -0.1f && Mathf.Abs(movement.y) >= Mathf.Abs(movement.x))
            _renderer.sprite = _down;
    }
    private void SetMoveSprites()
    {
        var sprites = _character.GetSprites();
        _right = sprites[0];
        _left = sprites[1];
        _up = sprites[2];
        _down = sprites[3];
    }

    private RuntimeAnimatorController GetMoveAnimation()
    {
        // Load Animation Controllers
        return _character.GetAnimator();
    }
}
