using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPanel : MonoBehaviour
{
    public string ObjectPrefab
    {
        get
        {
            Text GUIText = GetComponentInChildren<Text>();
            return GUIText.text;
        }
        set
        {
            Text GUIText = GetComponentInChildren<Text>();
            GUIText.text = value;
        }
    }

    public Image Thumbnail
    {
        get
        {
            Image img = transform.GetChild(0).GetComponent<Image>();
            return img;
        }
        set
        {
            Image img = transform.GetChild(0).GetComponent<Image>();
            img = value;
        }
    }

    public void UseThisObject()
    {
        EditorManager manager = FindObjectOfType<EditorManager>();
        manager.ChangePrefab(ObjectPrefab);
        manager.ToggleMenu(0);
        GameObject.Find("ModeDropDown").GetComponent<Dropdown>().value = 0;
    }
}
