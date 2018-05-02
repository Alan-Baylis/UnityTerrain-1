using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInside : MonoBehaviour
{

    public float Speed = 1;
    public float MaxSpeed = 3;
    public KeyCode EnableFastSpeedWithKey = KeyCode.LeftShift;
    public Transform TurnWithMovement;

    private Vector3 previousPosition = Vector3.zero;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var currentSpeed = Speed;
        if (Input.GetKey(EnableFastSpeedWithKey))
        {
            currentSpeed = MaxSpeed;
        }
        var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(movement * currentSpeed * Time.deltaTime);
        if (Mathf.Abs(movement.x) > 0.1f || Mathf.Abs(movement.y) > 0.1f)
        {
            TurnWithMovement.rotation = Quaternion.LookRotation(Vector3.back, movement.normalized);
        }

        Vector3 currentPos = transform.position;
        previousPosition = currentPos;
    }
}
