using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scenario
{
    public List<Cube> Cubes;
    
    public Scenario()
    {
        Cubes = new List<Cube>();
    }
}

[System.Serializable]
public struct Cube
{
    public Cube(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    public float X;
    public float Y;
    public float Z;
}
