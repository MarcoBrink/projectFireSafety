using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour {

    Rigidbody rb;
    Collider bc;
    Material m;
    void OnEnable()
    {
        ControlScript.OnClicked += Move;
        rb = GetComponent<Rigidbody>();       
        rb.useGravity = false;
        bc = GetComponent<Collider>();
        bc.enabled = false;
    }

    void OnDisable()
    {
        ControlScript.OnClicked -= Move;
        rb.useGravity = true;
        bc.enabled = true;
        gameObject.AddComponent<EnabledScript>();
    }

    void Disable()
    {
        this.enabled = false;
    }

    void Move(ControlScript.MyEvents e)
    {
        rb.velocity = Vector3.zero;
        if (e == ControlScript.MyEvents.Up)
        {
            transform.Translate(transform.forward / 10);
        }
        if (e == ControlScript.MyEvents.Right)
        {
            transform.Translate(transform.right / 10);
        }
        if (e == ControlScript.MyEvents.Down)
        {
            transform.Translate(transform.forward / 10 * -1);
        }
        if (e == ControlScript.MyEvents.Left)
        {
            transform.Translate(transform.right / 10 * -1);
        }
        if (e == ControlScript.MyEvents.Forward)
        {
            transform.Translate(transform.up / 10);
        }
        if (e == ControlScript.MyEvents.Backward)
        {
            transform.Translate(transform.up / 10 * -1);
        }
        if (e == ControlScript.MyEvents.Space)
        {
            Disable();
        }
    }

    void OnMouseDown()
    {
        this.enabled = true;
    }
}
