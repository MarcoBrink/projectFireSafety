using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileEntry : MonoBehaviour
{
    private string Path;

    private FilePicker Picker;

	// Use this for initialization
	void Start ()
    {
        this.Path = GetComponentInChildren<UnityEngine.UI.Text>().text;
        this.Picker = FindObjectOfType<FilePicker>();
	}

    /// <summary>
    /// Pick the path on this entry.
    /// </summary>
    public void Pick()
    {
        Picker.Pick(Path);
    }
}
