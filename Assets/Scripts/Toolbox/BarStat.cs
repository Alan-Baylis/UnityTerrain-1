using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BarStat
{
    [SerializeField]
    private BarHandler _bar;


    public float FillAmount
    {
        get { return _bar.FillAmount; }
        set
        {
            _bar.FillAmount = value;
        }
    }

    public float MaxValue
    {
        get { return _bar.MaxValue; }
        set
        {
            _bar.MaxValue = value;
        }
    }

}
