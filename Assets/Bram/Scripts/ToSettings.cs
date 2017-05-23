using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToSettings : MonoBehaviour {

	// Code om de Settings Scene te openen en deze te sluiten
	public void OpenScene ()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
	//again, moet dit?
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Settings"));
    }
}
