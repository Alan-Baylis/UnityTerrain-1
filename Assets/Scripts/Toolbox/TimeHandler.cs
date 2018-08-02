using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler {

    public static string PrintTime(TimeSpan timer)
    {
        if (timer.Days > 0)
            return string.Format("{0:0}d {1:00}h {2:00}m", timer.Days, timer.Hours, timer.Minutes);
        else if (timer.Hours > 0)
            return string.Format("{0:0}h {1:00}m {2:00}s", timer.Hours, timer.Minutes, timer.Seconds);
        else if (timer.Minutes > 0)
            return string.Format("{0:0}m {1:0}s",  timer.Minutes, timer.Seconds);
        else
            return string.Format("{0:0}s", timer.Seconds);
    }
}
