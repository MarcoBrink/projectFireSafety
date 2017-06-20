using Assets.Scripts.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HostOrClient : MonoBehaviour {
    //IP van de Host/Client
    private string IP = "192.168.1.2";
    //Is wel/geen host, uitgaan van niet
    private bool IsHost = false;
    //Variabele aanmaken om het scenario mee op te halen
    private string ScenarioFile;
    //En een array aanmaken om alle benodigde files in te zetten
    private string[] Files;

    /// <summary>
    /// Initialiseren van de Class
    /// </summary>
    private void Start()
    {
        Dropdown dropDown = GameObject.Find("DropdownScenario").GetComponent<Dropdown>();
        List<Dropdown.OptionData> dropDownList = new List<Dropdown.OptionData>();

        if (SaveLoad.GetSavedScenarios(Application.persistentDataPath + "/Scenarios/", out Files))
        {
            foreach (string file in Files)
            {
                dropDownList.Add(new Dropdown.OptionData(file));
            }             
        }
        dropDown.options = dropDownList;
        ScenarioFile = Files[0];
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }

    public void OnToggle(bool isHost)
    {
        IsHost = isHost;
        InputField inputField = GameObject.Find("InputFieldIP").GetComponent<InputField>();
        if (IsHost)
        {            
            inputField.interactable = false; 
        }
        else
        {
            inputField.interactable = true;
        }
    }

    public void CheckNumbers(string str)
    {
        if (String.IsNullOrEmpty(str))
        {
            Debug.Log("Vul iets in.");
            return;
        }

        string[] splitValues = str.Split('.');
        if(splitValues.Length != 4)
        {
            Debug.Log("Dit is geen juist IP adres.");
            return;
        }

        byte tempForParsing;

        if(splitValues.All(r => byte.TryParse(r, out tempForParsing)))
        {
            Debug.Log("IP is juist.");
            IP = str;
            Debug.Log("Nieuw IPadres: " + IP);
        }
    }

    public void ChangeScenario(int i)
    {
        ScenarioFile = Files[i];
    }

    public void StartSimulatie()
    {
        GameObject go = Instantiate(new GameObject("SimulatieData"));
        SimulationDataScript sds = go.AddComponent<SimulationDataScript>();
        
        sds.IP = IP;
        sds.IsHost = IsHost;
        sds.ScenarioFile = ScenarioFile;
        
        SceneManager.LoadScene("Simulatie", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Simulatie"));
    }
}
