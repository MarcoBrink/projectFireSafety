using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour {
    public enum MyEvents { Up, Right, Down, Left, Forward, Backward, Space }
    public delegate void ClickAction(MyEvents e);
    public static event ClickAction OnClicked;
    public string SelectedResource = "Prefabs/Block";

    void Update()
    {
        if(OnClicked == null)
        {
            Instantiate(Resources.Load(SelectedResource), Vector3.up, new Quaternion());
        }
        if (Input.GetKey(KeyCode.W))
        {
            OnClicked(MyEvents.Up);
        }
        if (Input.GetKey(KeyCode.D))
        {
            OnClicked(MyEvents.Right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            OnClicked(MyEvents.Down);
        }
        if (Input.GetKey(KeyCode.A))
        {
            OnClicked(MyEvents.Left);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            OnClicked(MyEvents.Forward);
        }
        if (Input.GetKey(KeyCode.E))
        {
            OnClicked(MyEvents.Backward);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClicked(MyEvents.Space);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(SelectedResource == "Prefabs/Block")
            {
                SelectedResource = "Prefabs/Sphere";
            }
            else
            {
                SelectedResource = "Prefabs/Block";
            }
        }
    }
}
