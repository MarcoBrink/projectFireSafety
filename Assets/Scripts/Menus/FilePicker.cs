using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Saving;
using Assets.Scripts.VRScenario;

public class FilePicker : MonoBehaviour
{
    /// <summary>
    /// All files currently available.
    /// </summary>
    private string[] Files;

    /// <summary>
    /// The Current Directory.
    /// </summary>
    private string CurrentDirectory
    {
        get
        {
            return GameObject.Find("PathField").GetComponent<InputField>().text;
        }
        set
        {
            GameObject.Find("PathField").GetComponent<InputField>().text = value;
        }
    }

    /// <summary>
    /// The currently selected path.
    /// </summary>
    private string CurrentPath
    {
        get
        {
            return GameObject.Find("FileField").GetComponent<InputField>().text;
        }
        set
        {
            GameObject.Find("FileField").GetComponent<InputField>().text = value;
        }
    }


    /// <summary>
    /// Assigned in editor; the prefab to use for file entries in the picker.
    /// </summary>
    public GameObject FileEntryPrefab;

	// Use this for initialization
	void Start ()
    {
        // Default directory is the persistent Unity Path.
        CurrentDirectory = Application.persistentDataPath + "/Scenarios/";

        // Scan for files in that path.
        Scan();
	}

    /// <summary>
    /// Show all files in the directory.
    /// </summary>
    private void ShowFiles()
    {
        // Instantiate an entry object for each file that was found.
        foreach (string file in Files)
        {
            GameObject entry = Instantiate(FileEntryPrefab);
            entry.GetComponentInChildren<Text>().text = GetDisplayPath(CurrentDirectory) + file;
            ScrollRect entryView = GameObject.Find("FileView").GetComponent<ScrollRect>();
            entry.transform.SetParent(entryView.content.transform, false);
        }
    }

    /// <summary>
    /// Pick the given path.
    /// </summary>
    /// <param name="path">The path of the picked file.</param>
    public void Pick(string path)
    {
        this.CurrentPath = path;
    }

    /// <summary>
    /// Load the selected scenario.
    /// </summary>
    public void LoadSelected()
    {
        //Create a variable to hold the scenario
        Scenario scenario;
        //Check if it loads...
        if (SaveLoad.LoadSavedScenario(CurrentPath, out scenario))
        {
            //If it does, use it as CurrentScenario, load it into the editor in Single mode and set it as active
            SaveLoad.CurrentScenario = scenario;
            SceneManager.LoadScene("Editor", LoadSceneMode.Single);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Editor"));
        }
        else
        {
            //If it doesn't, show the user a popup something went wrong
            MessagePopup popup = MessagePopup.CreateMessagePopup("Laden Mislukt", "Er is een fout opgetreden tijdens het laden van het opgegeven bestand. Het bestand bestaat mogelijk niet of is geen geldig Provrex VR Scenario.");
        }
    }

    /// <summary>
    /// Scan the current directory for valid files.
    /// </summary>
    public void Scan()
    {
        //Clear the currently known list of files to prevent duplicate/illogical entries
        ClearCurrentFiles();
        
        if(SaveLoad.GetSavedScenarios(CurrentDirectory, out Files))
        {
            // You only need to show the files if files were found.
            ShowFiles();
        }
        else
        {
            //Or show a popup that none were found
            MessagePopup popup = MessagePopup.CreateMessagePopup("Geen Bestanden Gevonden.", "Er zijn geen Provrex VR Scenario's gevonden in het opgegeven pad.");
        }
    }

    /// <summary>
    /// Clear all files currently shown in the file picker.
    /// </summary>
    /// <remarks>
    ///     Sets Files to null.
    /// </remarks>
    private void ClearCurrentFiles()
    {
        // Get the scroll view with all files in it.
        ScrollRect fileView = GameObject.Find("FileView").GetComponent<ScrollRect>();
        int fileCount = fileView.content.transform.childCount;

        // Destroy each file if there are any.
        if (fileCount != 0)
        {
            for (int childIndex = 0; childIndex < fileCount; childIndex++)
            {
                GameObject child = fileView.content.transform.GetChild(childIndex).gameObject;
                Destroy(child);
            }
        }

        //Reset files to null;
        Files = null;
    }

    /// <summary>
    /// Properly adds the last slash to a path (if needed) for displaying in an entry.
    /// </summary>
    /// <param name="path">The path to modify.</param>
    /// <returns>The proper string to use for displaying the path in an entry.</returns>
    private string GetDisplayPath(string path)
    {
        string displayPath = path;
        char[] valChars = displayPath.ToCharArray();
        int pathLength = valChars.Length;

        if (valChars[pathLength - 1] != '/')
        {
            displayPath += "/";
        }

        return displayPath;
    }

    /// <summary>
    /// Go back to the main menu.
    /// </summary>
    public void ToMainMenu()
    {
        //Load the main menu in Single mode and set it active
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }
}
