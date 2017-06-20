using UnityEngine;

public class SimulationDataScript : MonoBehaviour {

    //IP adres van de Host/Client
    public string IP;
    //Is wel of niet de Host
    public bool IsHost;
    public string ScenarioFile;

    /// <summary>
    /// Functie om te voorkoment dat dit script stopt
    /// </summary>
    private void Awake()
    {
        //Deze Class laten bestaan als er een OnLoad event plaatsvindt
        DontDestroyOnLoad(this);
    }
}
