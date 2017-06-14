using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Saving;
using Assets.Scripts.VRScenario;

public class NewScenario : MonoBehaviour
{
    public GameObject[] environments;

	// Use this for initialization
	void Start ()
    {
        GetEnvironments();
        GameObject.Find("PathField").GetComponent<InputField>().text = SaveLoad.SaveDirectory + "/";
    }


    /// <summary>
    /// Get the available environments from the Resources.
    /// </summary>
    public void GetEnvironments()
    {
        environments = Resources.LoadAll<GameObject>("Omgevingen/");
        Dropdown envDropDown = GameObject.Find("EnvDropdown").GetComponent<Dropdown>();
        List<Dropdown.OptionData> environmentOptions = new List<Dropdown.OptionData>();

        foreach (GameObject env in environments)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(env.name);
            environmentOptions.Add(option);
        }

        envDropDown.ClearOptions();
        envDropDown.AddOptions(environmentOptions);
    }

    public void CreateNewScenario()
    {
        string path = GameObject.Find("PathField").GetComponent<InputField>().text;
        int envID = GameObject.Find("EnvDropdown").GetComponent<Dropdown>().value;
        GameObject Environment = environments[envID];
        string name = SaveLoad.GetFileName(path);
        Scenario scenario = new Scenario(name, Environment);

        if (SaveLoad.SaveScenario(scenario, path))
        {
            SaveLoad.CurrentPath = path;
            SaveLoad.CurrentScenario = scenario;
            SceneManager.LoadScene("Editor", LoadSceneMode.Single);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Editor"));
        }
        else
        {
            MessagePopup.CreateMessagePopup("Scenario Maken Mislukt", "Er is iets fout gegaan bij het maken van het scenario, hier kunnen veel redenen voor zijn. Probeer een ander pad.");
        }
    }

    /// <summary>
    /// Return to the main menu.
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }
}
