using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class TimeTrigger
{
    private Timer t;

    public TimeTrigger()
    {
        t = new Timer();
        t.Elapsed += new ElapsedEventHandler(onTimer);
        t.Interval = 10000;
        t.Start();
    }

    public TimeTrigger(double triggerTime)
    {
        t = new Timer();
        t.Elapsed += new ElapsedEventHandler(onTimer);
        t.Interval = (triggerTime * 1000);
        t.Start();
    }

    void onTimer(object source, ElapsedEventArgs e)
    {
        // do stuff
        Debug.Log("Timer event triggered!");
        t.Stop();
    }
}
