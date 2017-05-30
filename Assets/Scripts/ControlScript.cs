using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyStatus { None = 0, Up, Down, Hold }
public enum ControlKeyCode { Mouse1, Mouse2, Up, Down, Forward, Backward, Left, Right, Shift, Control, Space, Alt}

public struct ControlKey
{
    public ControlKeyCode Key;
    public KeyStatus Status;

    public ControlKey(ControlKeyCode key, KeyStatus status)
    {
        this.Key = key;
        this.Status = status;
    }
}

public class ControlScript : MonoBehaviour
{
    public delegate void ControlEvent(ControlEventArgs args);
    public static event ControlEvent OnInput;

    private float PreviousVerticalAxis = 0F;
    private float PreviousHorizontalAxis = 0F;

    void Update()
    {
        List<ControlKey> keys = new List<ControlKey>();
        GetAxes(ref keys);
        GetKeys(ref keys);
        OnInput(new ControlEventArgs(keys));
    }

    private void GetAxes(ref List<ControlKey> keys)
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // Get vertical axis.
        if (vertical != PreviousVerticalAxis)
        {
            if (PreviousVerticalAxis == 0F)
            {
                if (vertical > 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Forward, KeyStatus.Down));
                }
                else if (vertical < 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Backward, KeyStatus.Down));
                }
            }
            else if (PreviousVerticalAxis != 0F)
            {
                if (vertical > 0F && PreviousVerticalAxis > 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Forward, KeyStatus.Hold));
                }
                else if (vertical > 0F && PreviousVerticalAxis < 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Forward, KeyStatus.Down));
                    keys.Add(new ControlKey(ControlKeyCode.Backward, KeyStatus.Up));
                }
                else if (vertical < 0F && PreviousVerticalAxis < 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Backward, KeyStatus.Hold));
                }
                else if (vertical < 0F && PreviousVerticalAxis > 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Backward, KeyStatus.Down));
                    keys.Add(new ControlKey(ControlKeyCode.Forward, KeyStatus.Up));
                }
                else if (vertical == 0)
                {
                    if (PreviousVerticalAxis > 0F)
                    {
                        keys.Add(new ControlKey(ControlKeyCode.Forward, KeyStatus.Up));
                    }
                    else if (PreviousVerticalAxis < 0F)
                    {
                        keys.Add(new ControlKey(ControlKeyCode.Backward, KeyStatus.Up));
                    }
                }
            }
        }

        // Get horizontal axis.
        if (horizontal != PreviousHorizontalAxis)
        {
            if (PreviousHorizontalAxis == 0F)
            {
                if (horizontal > 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Right, KeyStatus.Down));
                }
                else if (horizontal < 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Left, KeyStatus.Down));
                }
            }
            else if (PreviousHorizontalAxis != 0F)
            {
                if (horizontal > 0F && PreviousHorizontalAxis > 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Right, KeyStatus.Hold));
                }
                else if (horizontal > 0F && PreviousHorizontalAxis < 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Right, KeyStatus.Down));
                    keys.Add(new ControlKey(ControlKeyCode.Left, KeyStatus.Up));
                }
                else if (horizontal < 0F && PreviousHorizontalAxis < 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Left, KeyStatus.Hold));
                }
                else if (horizontal < 0F && PreviousHorizontalAxis > 0F)
                {
                    keys.Add(new ControlKey(ControlKeyCode.Left, KeyStatus.Down));
                    keys.Add(new ControlKey(ControlKeyCode.Right, KeyStatus.Up));
                }
                else if (horizontal == 0)
                {
                    if (PreviousHorizontalAxis > 0F)
                    {
                        keys.Add(new ControlKey(ControlKeyCode.Right, KeyStatus.Up));
                    }
                    else if (PreviousHorizontalAxis < 0F)
                    {
                        keys.Add(new ControlKey(ControlKeyCode.Left, KeyStatus.Up));
                    }
                }
            }
        }

        PreviousVerticalAxis = vertical;
        PreviousHorizontalAxis = horizontal;
    }

    /// <summary>
    /// Get the status of all current keys.
    /// </summary>
    /// <param name="keys">The list to which the keys will be added.</param>
    private void GetKeys(ref List<ControlKey> keys)
    {
        // Get Key Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            keys.Add(new ControlKey(ControlKeyCode.Down, KeyStatus.Down));
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            keys.Add(new ControlKey(ControlKeyCode.Down, KeyStatus.Up));
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            keys.Add(new ControlKey(ControlKeyCode.Down, KeyStatus.Hold));
        }

        // Get key E
        if (Input.GetKeyDown(KeyCode.E))
        {
            keys.Add(new ControlKey(ControlKeyCode.Up, KeyStatus.Down));
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            keys.Add(new ControlKey(ControlKeyCode.Up, KeyStatus.Up));
        }
        else if (Input.GetKey(KeyCode.E))
        {
            keys.Add(new ControlKey(ControlKeyCode.Up, KeyStatus.Hold));
        }

        // Get key space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keys.Add(new ControlKey(ControlKeyCode.Space, KeyStatus.Down));
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            keys.Add(new ControlKey(ControlKeyCode.Space, KeyStatus.Up));
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            keys.Add(new ControlKey(ControlKeyCode.Space, KeyStatus.Hold));
        }

        // Get key shift
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            keys.Add(new ControlKey(ControlKeyCode.Shift, KeyStatus.Down));
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            keys.Add(new ControlKey(ControlKeyCode.Shift, KeyStatus.Up));
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            keys.Add(new ControlKey(ControlKeyCode.Shift, KeyStatus.Hold));
        }

        // Get key alt
        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
        {
            keys.Add(new ControlKey(ControlKeyCode.Alt, KeyStatus.Down));
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt))
        {
            keys.Add(new ControlKey(ControlKeyCode.Alt, KeyStatus.Up));
        }
        else if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            keys.Add(new ControlKey(ControlKeyCode.Alt, KeyStatus.Hold));
        }

        // Get key control
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            keys.Add(new ControlKey(ControlKeyCode.Control, KeyStatus.Down));
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            keys.Add(new ControlKey(ControlKeyCode.Control, KeyStatus.Up));
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            keys.Add(new ControlKey(ControlKeyCode.Control, KeyStatus.Hold));
        }

        // Get mouse1
        if (Input.GetMouseButtonDown(0))
        {
            keys.Add(new ControlKey(ControlKeyCode.Mouse1, KeyStatus.Down));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            keys.Add(new ControlKey(ControlKeyCode.Mouse1, KeyStatus.Up));
        }
        else if (Input.GetMouseButton(0))
        {
            keys.Add(new ControlKey(ControlKeyCode.Mouse1, KeyStatus.Hold));
        }

        // Get mouse2
        if (Input.GetMouseButtonDown(1))
        {
            keys.Add(new ControlKey(ControlKeyCode.Mouse2, KeyStatus.Down));
        }
        else if (Input.GetMouseButtonUp(1))
        {
            keys.Add(new ControlKey(ControlKeyCode.Mouse2, KeyStatus.Up));
        }
        else if (Input.GetMouseButton(1))
        {
            keys.Add(new ControlKey(ControlKeyCode.Mouse2, KeyStatus.Hold));
        }
    }
}

public class ControlEventArgs
{
    private List<ControlKey> Keys;

    /// <summary>
    /// Returns the status for the given key.
    /// </summary>
    /// <param name="key">The key for which to get the status.</param>
    /// <returns>The KeyStatus belonging to the requested key.</returns>
    public KeyStatus this[ControlKeyCode key]
    {
        get
        {
            ControlKey foundKey = Keys.Find(thisKey => thisKey.Key == key);

            return foundKey.Status;
        }
        // Set does nothing here; the version of .NET used with Unity doesn't allow read only Properties.
        private set { }
    }

    public ControlEventArgs(List<ControlKey> keys)
    {
        this.Keys = keys;
    }
}
