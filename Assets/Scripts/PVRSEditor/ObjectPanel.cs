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

    public void UseThisObject()
    {
        EditorManager manager = FindObjectOfType<EditorManager>();
        manager.ChangePrefab(ObjectPrefab);
    }
}
