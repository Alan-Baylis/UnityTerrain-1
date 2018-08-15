using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MoveManager : MonoBehaviour {
    
    public float Speed = 3;
    public float MaxSpeed = 2;
    public KeyCode EnableFastSpeedWithKey = KeyCode.LeftShift;
    //public Transform TurnWithMovement;
    public bool DoAnimation = true;


    private CharacterManager _characterManager;
    private Character _playerCharacter;

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

        _renderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        _myRigidbody2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();


        _animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        if (_animator == null)
            _animator = GameObject.FindGameObjectWithTag("Player").AddComponent<Animator>();

        SetMoveSprites(99);
        SetMoveAnimation(99);

        if (DoAnimation)
            _animator.runtimeAnimatorController = _playerAnimeCtrl;
        else
            _renderer.sprite = _down;
    }

    // Update is called once per frame
    void Update () {

        //New moving System by touch/Mouse
        //Touch touch = Input.GetTouch(0);
        //Vector3 touchpos = GetComponentInChildren<Camera>().ScreenToWorldPoint(touch.position);
        //Vector3 touchpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 playerpos = transform.position;
        //_movement = (touchpos - playerpos).normalized;
        //print( touchpos + " - " + playerpos + " = " + (touchpos - playerpos) + " normalized = " + _movement); 



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
        DoMove(_movement);
    }
    private void DoMove(Vector3 movement)
    {
        var currentSpeed = Speed;
        if (Input.GetKey(EnableFastSpeedWithKey))
        {
            currentSpeed = MaxSpeed;
        }
        //Old moving system
        transform.Translate(movement * currentSpeed * Time.deltaTime);
        //Physics moving
        //_myRigidbody2D.velocity = movement.normalized * currentSpeed;
    }

    private void HandleAnimation(Vector3 movement)
    {
        if (_playerCharacter.MoveType != Character.CharacterType.Fly && movement == Vector3.zero)
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


    private void SetMoveSprites(int charId)
    {
        string character = _playerCharacter.Name;
        // Load all sprites in atlas
        Sprite[] abilityIconsAtlas = Resources.LoadAll<Sprite>("Characters/"+character);
        // Get specific sprite
        _right = abilityIconsAtlas.Single(s => s.name == "right_3");
        _left = abilityIconsAtlas.Single(s => s.name == "left_3");
        _up = abilityIconsAtlas.Single(s => s.name == "up_3");
        _down = abilityIconsAtlas.Single(s => s.name == "down_3");
    }

    private void SetMoveAnimation(int charId)
    {
        // Load Animation Controllers
        string animationPath = "Characters/Animations/";
        _playerAnimeCtrl = (RuntimeAnimatorController)Resources.Load(animationPath + _playerCharacter.Name + "Controller");
    }



}
