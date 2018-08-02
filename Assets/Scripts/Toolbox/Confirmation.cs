using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confirmation : MonoBehaviour
{


    public GameObject ConfirmationUI;

    private bool _setFalse;
    private bool _confirmed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
	        if (ConfirmationUI.activeSelf)
	            ConfirmationUI.SetActive(false);
	        else
	            ConfirmationUI.SetActive(true);
        }

	    if (_setFalse)
	    {
	        _setFalse = true;
	        _confirmed = false;
        }
	}

    private void ConfirmOk()
    {
        ConfirmationUI.SetActive(false);
        _confirmed = true;
    }


    private void ConfirmCancel()
    {
        ConfirmationUI.SetActive(false);
        _confirmed = false;
    }

    public bool isConfirmed()
    {
        _setFalse = true;
        return _confirmed;
    }
}
