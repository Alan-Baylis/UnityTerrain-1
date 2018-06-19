using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;

[Serializable]
public class Recipe{
    public int Id { get; set; }
    public int FirstItemId { get; set; }
    public int FirstItemCnt { get; set; }
    public int SecondItemId { get; set; }
    public int SecondItemCnt { get; set; }
    public int FinalItemId { get; set; }
    public int FinalItemCnt { get; set; }
    public bool IsEnable { get; set; }
}
