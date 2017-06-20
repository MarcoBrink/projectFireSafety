struct Trigger
{
    public string Name { get; set; }
    public double TriggerTime { get; set; }


    public Trigger(string name, double triggerTime)
    {
        Name = name;
        TriggerTime = triggerTime;
    }
}
