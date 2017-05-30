using UnityEngine;
using System.Collections;

public class ToggleModeMenu : MonoBehaviour
{
    private Canvas Modusmenu; // Assign in inspector

    public void ToggleObject()
    {
        Modusmenu = GetComponent<Canvas>();
        Modusmenu.enabled = !Modusmenu.enabled;
    }
}