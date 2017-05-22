using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour {

    Rigidbody rb;
    Collider bc;
    Material m;

    bool CanPlace = true;
    void OnEnable()
    {
        ControlScript.OnClicked += Move;
        rb = GetComponent<Rigidbody>();       
        rb.useGravity = false;
        bc = GetComponent<Collider>();
        bc.isTrigger = true;
        gameObject.AddComponent<WireframeScript>();
        WireframeScript wireframe = GetComponent<WireframeScript>();
        wireframe.render_mesh_normally = false;
        wireframe.lineColor = Color.green;
        wireframe.lineWidth = 1;
    }

    void OnDisable()
    {
        ControlScript.OnClicked -= Move;
        rb.useGravity = true;
        bc.isTrigger = false;
        gameObject.AddComponent<EnabledScript>();
        WireframeScript wireframe = GetComponent<WireframeScript>();
        wireframe.render_mesh_normally = true;
    }

    void OnTriggerStay(Collider other)
    {
        WireframeScript wireframe = GetComponent<WireframeScript>();
        wireframe.lineColor = Color.red;
        CanPlace = false;
    }

    void OnTriggerExit(Collider other)
    {
        WireframeScript wireframe = GetComponent<WireframeScript>();
        wireframe.lineColor = Color.green;
        CanPlace = true;
    }

    void Disable()
    {
        if(CanPlace)
        {
            this.enabled = false;
        }       
    }

    void Move(ControlScript.MyEvents e)
    {
        rb.velocity = Vector3.zero;
        if (e == ControlScript.MyEvents.Up)
        {
            transform.position += (Vector3.forward / 10);
        }
        if (e == ControlScript.MyEvents.Right)
        {
            transform.position += (Vector3.right / 10);
        }
        if (e == ControlScript.MyEvents.Down)
        {
            transform.position += (Vector3.forward / 10 * -1);
        }
        if (e == ControlScript.MyEvents.Left)
        {
            transform.position += (Vector3.right / 10 * -1);
        }
        if (e == ControlScript.MyEvents.Forward)
        {
            transform.position += (Vector3.up / 10);
        }
        if (e == ControlScript.MyEvents.Backward)
        {
            transform.position += (Vector3.up / 10 * -1);
        }
        if (e == ControlScript.MyEvents.Space)
        {
            Disable();
        }
        if (e == ControlScript.MyEvents.Shift)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        if (!this.enabled) this.enabled = true;
        else if (GetComponent<EnabledScript>() != null)
        {
            this.enabled = false;
        }
        else Destroy(this.gameObject);
    }
}
