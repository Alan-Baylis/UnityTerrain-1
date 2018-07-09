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


    private Character _playerCharacter;

    private Sprite _up;
    private Sprite _down;
    private Sprite _right;
    private Sprite _left;

    private RuntimeAnimatorController _upAnime;
    private RuntimeAnimatorController _downAnime;
    private RuntimeAnimatorController _righAnimet;
    private RuntimeAnimatorController _leftAnime;


    private GameObject _player;
    private SpriteRenderer _renderer;
    private Animator _animator;


    // Use this for initialization
    private void Start ()
    {
        _playerCharacter = CharacterManager.Instance.GetCharacterFromDatabase(1);

        _renderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();

        _animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        if (_animator == null)
            _animator = GameObject.FindGameObjectWithTag("Player").AddComponent<Animator>();

        SetMoveSprites(1);
        SetMoveAnimation(1);

        if (DoAnimation)
            _animator.runtimeAnimatorController = _downAnime;
        else
            _renderer.sprite = _down;


    }

    // Update is called once per frame
    void Update () {

        var currentSpeed = Speed;
	    if (Input.GetKey(EnableFastSpeedWithKey))
	    {
	        currentSpeed = MaxSpeed;
	    }

        //Get the value of the movemoen x -1(Left) .. +1(Right) & y -1(Down) .. +1(UP)
	    var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        transform.Translate(movement * currentSpeed * Time.deltaTime);
        if ( movement != Vector3.zero)
            _animator.speed = 1;
        //Change Sprite or Animation according to the direction of moving
        if (movement.x > 0.1f && Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            if(DoAnimation)
                _animator.runtimeAnimatorController = _righAnimet;
            else
                _renderer.sprite = _right;
        if (movement.x < -0.1f && Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            if (DoAnimation)
                _animator.runtimeAnimatorController = _leftAnime;
            else
                _renderer.sprite = _left;
        if (movement.y > 0.1f && Mathf.Abs(movement.y) >= Mathf.Abs(movement.x))
            if (DoAnimation)
                _animator.runtimeAnimatorController = _upAnime;
            else
                _renderer.sprite = _up;
        if (movement.y < -0.1f && Mathf.Abs(movement.y) >= Mathf.Abs(movement.x))
            if (DoAnimation)
                _animator.runtimeAnimatorController = _downAnime;
            else
	            _renderer.sprite = _down;
        print("###Inside move manager: " + (int) _playerCharacter.MoveType);
        if ( _playerCharacter.MoveType != Character.CharacterType.Fly && movement == Vector3.zero)
        {
            _animator.speed = 0;
        }
        //Rotate game object according to the direction
        //if (Mathf.Abs(movement.x) > 0.1f || Mathf.Abs(movement.y) > 0.1f)
        //    TurnWithMovement.rotation = Quaternion.LookRotation(Vector3.back, movement.normalized);
    }



    //Draw the item sprite in the Rect 
    public void GetCharacter(int id)
    {
        return;
    }


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
        string character = _playerCharacter.Name;
        // Load Animation Controllers
        string animationPath = "Characters/Animations/";

        _righAnimet = (RuntimeAnimatorController)Resources.Load(animationPath+ character+"RightWalk"); ; 
        _leftAnime = (RuntimeAnimatorController)Resources.Load(animationPath + character + "LeftWalk"); ; 
        _upAnime = (RuntimeAnimatorController)Resources.Load(animationPath + character + "UpWalk"); ; 
        _downAnime = (RuntimeAnimatorController)Resources.Load(animationPath + character + "DownWalk"); ; 
    }



}
