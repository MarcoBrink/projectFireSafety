using System.Timers;
using UnityEngine;

public class TimeTrigger
{
    //Timer om de trigger op gezette tijden te laten lopen
    private Timer t;

    /// <summary>
    /// Trigger en alle waarden ervan automatisch initialiseren
    /// </summary>
    public TimeTrigger()
    {
        //Timer aanmaken
        t = new Timer();
        //Eventhandler aanmaken voor elke keer dat de interval verloopt
        t.Elapsed += new ElapsedEventHandler(onTimer);
        //Interval instellen
        t.Interval = 10000;
        //Timer opstarten
        t.Start();
    }

    /// <summary>
    /// Trigger met custom tijd initialiseren
    /// </summary>
    /// <param name="triggerTime">De gewenste interval</param>
    public TimeTrigger(double triggerTime)
    {
        //Timer initialiseren
        t = new Timer();
        //Eventhandler toevoegen
        t.Elapsed += new ElapsedEventHandler(onTimer);
        //Custom interval instellen
        t.Interval = (triggerTime * 1000);
        //Timer starten
        t.Start();
    }

    /// <summary>
    /// Actie om te ondernemen als het t.Elapsed event wordt aangeroepen
    /// </summary>
    /// <param name="source">De timer die het event veroorzaakt</param>
    /// <param name="e">Eventargs van het event</param>
    void onTimer(object source, ElapsedEventArgs e)
    {
        //Timer stoppen
        t.Stop();
    }
}
