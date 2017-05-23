using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToHome : MonoBehaviour
{
    

    // Code om het startscherm in te laden en deze Scene weer te sluiten
    public void OpenHome()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
	//moet deze hieronder nog wel bij Singular Load?
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }

    // Update is called once per frame
    void Update()
    {
	//acties om ieder frame opnieuw uit te voeren
    }
}

