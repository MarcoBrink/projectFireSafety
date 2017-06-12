using System.Collections;
using System.Collections.Generic;
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
    /// The directory for scenario files.
    /// </summary>
    private string Dir;

    /// <summary>
    /// The currently selected path.
    /// </summary>
    public string CurrentPath
    {
        get
        {
            return FindObjectOfType<InputField>().text;
        }
        set
        {
            FindObjectOfType<InputField>().text = value;
        }
    }


    /// <summary>
    /// Assigned in editor; the prefab to use for file entries in the picker.
    /// </summary>
    public GameObject FileEntryPrefab;

	// Use this for initialization
	void Start ()
    {
        Dir = Application.persistentDataPath + "/Scenarios/";
        Files = SaveLoad.GetSavedScenarios(Dir);
        ShowFiles();
	}

    private void ShowFiles()
    {
        foreach (string file in Files)
        {
            GameObject entry = Instantiate(FileEntryPrefab);
            entry.GetComponentInChildren<Text>().text = Dir + file + ".pvrs";
            ScrollRect entryView = FindObjectOfType<ScrollRect>();
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
        Scenario scenario;

        if (SaveLoad.LoadSavedScenario(CurrentPath, out scenario))
        {
            SaveLoad.CurrentScenario = scenario;
            SceneManager.LoadScene("Editor", LoadSceneMode.Single);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Editor"));
        }
        else
        {
            Debug.LogError("Loading failed");
        }
    }

    /// <summary>
    /// Go back to the main menu.
    /// </summary>
    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }
}
