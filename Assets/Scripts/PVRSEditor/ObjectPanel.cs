using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPanel : MonoBehaviour
{
    /// <summary>
    /// The name of the prefab linked to this object.
    /// </summary>
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

    /// <summary>
    /// The thumbnail image used by this object panel.
    /// </summary>
    public Image Thumbnail
    {
        get
        {
            Image img = transform.GetChild(0).GetComponent<Image>();
            return img;
        }
        private set
        {
            // Does nothing, not used and not needed; MONO's .NET doesn't support read-only properties.
        }
    }
    
    /// <summary>
    /// Used to tell the editor this object needs to be selected.
    /// </summary>
    /// Referenced in an OnClick in the Unity Editor.
    public void UseThisObject()
    {
        EditorManager manager = FindObjectOfType<EditorManager>();
        manager.ChangePrefab(ObjectPrefab);
        manager.ToggleMenu(0);
        GameObject.Find("ModeDropDown").GetComponent<Dropdown>().value = 0;
    }
}
