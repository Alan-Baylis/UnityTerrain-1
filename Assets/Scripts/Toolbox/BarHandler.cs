using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class BarHandler : MonoBehaviour
{
    [SerializeField]
    private float _lerpSpeed=2;

    private Image _content;
    private Text _text;

    public float MaxValue { get; set; }
    public float FillAmount { get; set; }

    // Use this for initialization
    void Start ()
    {
        var contents = GetComponentsInChildren<Image>();
        foreach (var conten in contents)
            if (conten.type ==Image.Type.Filled)
                _content = conten;
        var texts = GetComponentsInChildren<Text>();
        foreach (var text in texts)
                _text = text;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    ChangeFillAmount(0);
	}

    private void ChangeFillAmount(float amount)
    {
        //if (amount == 0)
        //    return;

        FillAmount += amount;
        FillAmount = Mathf.Clamp(FillAmount, 0, MaxValue);
        _text.text = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", FillAmount);
        //Doing thi with lerp Over time =>  _content.fillAmount = MapConvert(FillAmount, 100) 
        _content.fillAmount = Mathf.Lerp(_content.fillAmount, MapConvert(FillAmount, 100), Time.deltaTime *_lerpSpeed);
    }
    private float MapConvert(float value, float inMax)
    {
        return MapConvert(value, 0, inMax, 0, 1);
    }

    private float MapConvert(float value, float inMin, float inMax, float outMin, float outMax)
    {
        //Map the value between inMin and inMax to a value bewtween outMin and outMax
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
