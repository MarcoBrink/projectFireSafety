using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationDataScript : MonoBehaviour {

    public string IP;
    public bool IsHost;
    public string ScenarioFile;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
