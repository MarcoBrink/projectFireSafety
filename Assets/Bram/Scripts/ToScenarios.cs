using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToScenarios : MonoBehaviour
{
    //Code om de Scene Editor te openen en deze Scene te sluiten
    public void OpenScene()
    {
        SceneManager.LoadScene("Scenarios", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Scenarios"));
    }
}
