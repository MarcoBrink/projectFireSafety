using UnityEngine;
using System.Collections;

public class OpenMenu : MonoBehaviour
{
    private Canvas Objectmenu; // Assign in inspector

    void Start()
    {
        Objectmenu = GetComponent<Canvas>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Objectmenu.enabled = !Objectmenu.enabled;
        }
    }
}