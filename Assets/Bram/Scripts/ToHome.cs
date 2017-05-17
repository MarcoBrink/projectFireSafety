using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToHome : MonoBehaviour
{
    

    // Use this for initialization
    public void OpenHome()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}

