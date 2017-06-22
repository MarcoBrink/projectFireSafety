using UnityEngine;

public class FileEntry : MonoBehaviour
{
    //Filepath currently in use
    private string Path;
    //Filepicker, starts at current Path, can be set to go elsewhere
    private FilePicker Picker;

	/// <summary>
    /// Initialization function, sets variables needed to function
    /// </summary>
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
