using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour {
    public enum MyEvents { Up, Right, Down, Left, Forward, Backward, Space, Shift }
    public delegate void ClickAction(MyEvents e);
    public static event ClickAction OnClicked;
    public string SelectedResource = "Prefabs/Block";

    Object[] SelectablePrefabs;
    GameObject CurrentCursor;
    Vector3 CursorLocation;
    void Awake()
    {
       CursorLocation = GetCursorPosition();
       SelectablePrefabs = Resources.LoadAll("Prefabs/");
    }

    void Update()
    {
        if(OnClicked == null)
        {
            CurrentCursor = (GameObject)Instantiate(Resources.Load(SelectedResource), CursorLocation, new Quaternion());
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
            CursorLocation = GetCursorPosition();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SelectedResource = "Prefabs/" + GetObjectByName();
            CursorLocation = GetCursorPosition();
            OnClicked(MyEvents.Shift);
        }
    }

    private string GetObjectByName()
    {
        for (int i = 0; i < SelectablePrefabs.Length; i++)
        {
            if ("Prefabs/" + SelectablePrefabs[i].name == SelectedResource)
            {
                if(i+1 < SelectablePrefabs.Length)
                {
                    return SelectablePrefabs[i + 1].name;
                }
            }
        }          
        return SelectablePrefabs[0].name;
    }

    private Vector3 GetCursorPosition()
    {
        if(CurrentCursor != null)
        {
            return CurrentCursor.transform.position;
        }
        return Vector3.zero;
    }
}
