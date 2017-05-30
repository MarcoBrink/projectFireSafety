using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMenus : MonoBehaviour
{
    private Canvas Menu;

    void Start()
    {
        Menu = GetComponent<Canvas>();
        Menu.enabled = false;
    }
}
