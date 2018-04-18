using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float Speed = 1;
    public float MaxSpeed = 3;
    public KeyCode EnableFastSpeedWithKey = KeyCode.LeftShift;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var currentSpeed = Speed;
        if (Input.GetKey(EnableFastSpeedWithKey))
        {
            currentSpeed = MaxSpeed;
        }
		var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0);
        transform.Translate(movement * currentSpeed * Time.deltaTime);
    }
}
