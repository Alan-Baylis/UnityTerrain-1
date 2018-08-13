using System;
using UnityEngine;

[Serializable]
public class Recipe{
    public int Id { get; set; }
    public int FirstItemId { get; set; }
    public int FirstItemCnt { get; set; }
    public int SecondItemId { get; set; }
    public int SecondItemCnt { get; set; }
    public int FinalItemId { get; set; }
    public int FinalItemCnt { get; set; }
    public int DurationMinutes { get; set; }
    public int Energy { get; set; }
    public bool IsEnable { get; set; }


    private ItemDatabase _itemDatabase;

    public virtual string GetDescription()
    {
        _itemDatabase = ItemDatabase.Instance();
        try
        {
            return "Ready to mix " +
                   FirstItemCnt + " of "+
                   _itemDatabase.FindItem(FirstItemId).Name+
                   " with " + SecondItemCnt + " of " +
                   _itemDatabase.FindItem(SecondItemId).Name +
                   " to make " + FinalItemCnt + " of " +
                   _itemDatabase.FindItem(FinalItemId).Name +
                   " for " + Energy + " energy ? ";
            //todo: current font doesn't support ( ) 
            //return "Ready to mix " +
            //       _itemDatabase.FindItem(FirstItemId).Name
            //       + "(" + FirstItemCnt + ") +" +
            //       _itemDatabase.FindItem(SecondItemId).Name
            //       + "(" + SecondItemCnt + ") and make " +
            //       _itemDatabase.FindItem(FinalItemId).Name
            //       + "(" + FinalItemCnt + ") for " +
            //       Energy + " energy ? ";
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return "Ready to mix items?";
        }
    }


}
