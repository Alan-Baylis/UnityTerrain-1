using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{

    private ItemContainer _item;
    private GameObject _tooltip;

	// Use this for initialization
	void Start () {
	    _tooltip = GameObject.Find("Tooltip");
	    _tooltip.SetActive(false);

    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (_tooltip.activeSelf)
	    {
	        _tooltip.transform.position = Input.mousePosition;

	    }
	}

    public void Activate(ItemContainer Item)
    {
        if (Item.Id != -1)
        {
            _tooltip.transform.GetChild(0).GetComponent<Text>().text = Item.GetTooltip();
            _tooltip.SetActive(true);
        }
    }

    public void Dectivate()
    {
        _tooltip.SetActive(false);
    }

}
