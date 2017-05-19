using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToSettings : MonoBehaviour {

	// Use this for initialization
	public void OpenScene ()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Settings"));
    }
}
