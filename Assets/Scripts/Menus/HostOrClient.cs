using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostOrClient : MonoBehaviour {

    private string IP = "192.168.1.2";
    private bool IsHost = false;

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }

    public void OnToggle(bool isHost)
    {
        IsHost = isHost;
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

    public void StartSimulatie()
    {
        GameObject go = Instantiate(new GameObject("SimulatieData"));
        SimulationDataScript sds = go.AddComponent<SimulationDataScript>();
        
        sds.IP = IP;
        sds.IsHost = IsHost;
        
        SceneManager.LoadScene("Simulatie", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Simulatie"));
    }
}
