using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MoveManager : MonoBehaviour {
    
    public float Speed = 3;
    public KeyCode EnableFastSpeedWithKey = KeyCode.LeftShift;
    //public Transform TurnWithMovement;
    public bool DoAnimation = true;


    private int _baseSortIndex = 0;
    private CharacterManager _characterManager;
    private Character _playerCharacter;
    private CharacterSetting _playerCharacterSetting;

    private Sprite _up;
    private Sprite _down;
    private Sprite _right;
    private Sprite _left;
    
    private RuntimeAnimatorController _playerAnimeCtrl;


    //private GameObject _player;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private Vector3 _movement;

    private Rigidbody2D _myRigidbody2D;


    // Use this for initialization
    private void Start ()
    {
        _characterManager = CharacterManager.Instance();
        _playerCharacter = _characterManager.Character;
        _playerCharacterSetting = _characterManager.CharacterSetting;

        _renderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        _myRigidbody2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();


        _animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        if (_animator == null)
            _animator = GameObject.FindGameObjectWithTag("Player").AddComponent<Animator>();

        SetMoveSprites();
        SetMoveAnimation();

        if (DoAnimation)
            _animator.runtimeAnimatorController = _playerAnimeCtrl;
        else
            _renderer.sprite = _down;
    }

    // Update is called once per frame
    void Update () {

        //New moving System by touch/Mouse
        //Vector3 touchpos;
        //Touch touch = Input.GetTouch(0);
        ////touchpos = GetComponentInChildren<Camera>().ScreenToWorldPoint(touch.position);
        //if (Input.GetMouseButtonDown(0))
        //{
        //    touchpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector3 playerpos = transform.position;
        //    _movement = (touchpos - playerpos).normalized;
        //    print(touchpos + " - " + playerpos + " = " + (touchpos - playerpos) + " normalized = " + _movement);
        //}

        //Old moving System by key board 
        //Get the value of the movemoen x -1(Left) .. +1(Right) & y -1(Down) .. +1(UP)
        _movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        //Change Sprite or Animation according to the direction of moving
        if (DoAnimation)
            HandleAnimation(_movement);
        else
            HandleSprite(_movement);
    }

    private void FixedUpdate()
    {
        if (!_playerCharacterSetting.Fightmode)
            DoMove(_movement);
    }
    private void DoMove(Vector3 movement)
    {
        var currentSpeed = _playerCharacter.BasicSpeed;
        if (Input.GetKey(EnableFastSpeedWithKey))
        {
            currentSpeed += 2;
        }
        //Old moving system
        transform.Translate(movement * currentSpeed * Time.deltaTime);
        if (_playerCharacter.MoveT == Character.CharacterType.Fly)
            _renderer.sortingOrder = _baseSortIndex  + 6;
        else
            _renderer.sortingOrder = _baseSortIndex  + 3;

        //Physics moving
        //_myRigidbody2D.velocity = movement.normalized * currentSpeed;
    }

    private void HandleAnimation(Vector3 movement)
    {
        if (_playerCharacter.MoveT != Character.CharacterType.Fly && movement == Vector3.zero)
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
        _animator.SetFloat("x",movement.x);
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
    //TODO: Attach animation and layer
    //https://www.youtube.com/watch?v=aOqQuD_1ylA
    //https://www.youtube.com/watch?v=Y03jBu6enf8 (some movement improvement animantions on layers)


    private void SetMoveSprites()
    {
        var sprites = _playerCharacter.GetSprites();
        _right = sprites[0];
        _left = sprites[1];
        _up = sprites[2];
        _down = sprites[3];
    }

    private void SetMoveAnimation()
    {
        // Load Animation Controllers
        _playerAnimeCtrl = _playerCharacter.GetAnimator();
    }



}
