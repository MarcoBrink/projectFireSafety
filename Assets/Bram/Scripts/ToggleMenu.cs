using UnityEngine;
using System.Collections;

public class ToggleMenu : MonoBehaviour
{
    private Canvas Objectmenu; // Assign in inspector

    public void ToggleObject()
    {
        Objectmenu = GetComponent<Canvas>();
        Objectmenu.enabled = !Objectmenu.enabled;
    }
}