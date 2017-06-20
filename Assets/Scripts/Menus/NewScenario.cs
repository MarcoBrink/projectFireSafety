using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Saving;
using Assets.Scripts.VRScenario;

public class NewScenario : MonoBehaviour
{
    public GameObject[] environments;

    ///<Summary>
    /// Use this for initialization
    /// </Summary>
    void Start ()
    {
        //Roep alle omgevingen op
        GetEnvironments();
        //De standaard plaats instellen om op te slaan, kan nog veranderd worden
        GameObject.Find("PathField").GetComponent<InputField>().text = SaveLoad.SaveDirectory + "/";
    }


    /// <summary>
    /// Get the available environments from the Resources.
    /// </summary>
    public void GetEnvironments()
    {
        //alle resources in de Omgevingen folder inladen
        environments = Resources.LoadAll<GameObject>("Omgevingen/");
        //Dropdown menu aanmaken om een omgeving mee te kunnen kiezen
        Dropdown envDropDown = GameObject.Find("EnvDropdown").GetComponent<Dropdown>();
        //En een lijst aanmaken voor de inhoud van het Dropdown menu
        List<Dropdown.OptionData> environmentOptions = new List<Dropdown.OptionData>();

        //Vervolgens alle omgevingen aan deze lijst toevoegen
        foreach (GameObject env in environments)
        {
            //Opties maken
            Dropdown.OptionData option = new Dropdown.OptionData(env.name);
            //En een waarde aan de optie meegeven, in dit geval een omgeving
            environmentOptions.Add(option);
        }

        //Standaardopties van het Dropdown menu verwijderen en eigen opties erin zetten
        envDropDown.ClearOptions();
        envDropDown.AddOptions(environmentOptions);
    }

    /// <summary>
    /// Functie om een nieuw scenario te maken
    /// </summary>
    public void CreateNewScenario()
    {
        //Het pad opvragen en vastzetten
        string path = GameObject.Find("PathField").GetComponent<InputField>().text;
        //Omgevings ID opvragen en vastzetten
        int envID = GameObject.Find("EnvDropdown").GetComponent<Dropdown>().value;
        //En deze in een GameObject zetten om hem te gebruiken in het scenario
        GameObject Environment = environments[envID];
        //Bestandsnaam opvragen en vastzetten
        string name = SaveLoad.GetFileName(path);
        //En vast klaarmaken om op te slaan
        Scenario scenario = new Scenario(name, Environment);

        //Bij bestand opslaan...
        if (SaveLoad.SaveScenario(scenario, path))
        {
            //Als het succesvol is meteen CurrentPath en CurrentScenario updaten en vastzetten
            SaveLoad.CurrentPath = path;
            SaveLoad.CurrentScenario = scenario;
            //En deze openen in de Editor
            SceneManager.LoadScene("Editor", LoadSceneMode.Single);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Editor"));
        }
        else
        {
            //Anders een popup maken dat er iets mis is gegaan
            MessagePopup.CreateMessagePopup("Scenario Maken Mislukt", "Er is iets fout gegaan bij het maken van het scenario, hier kunnen veel redenen voor zijn. Probeer een ander pad.");
        }
    }

    /// <summary>
    /// Return to the main menu.
    /// </summary>
    public void ReturnToMainMenu()
    {
        //Menu inladen in de Single mode, zodat andere scenes gesloten worden
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        //en menu actief zetten
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }
}
