using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToPopup : MonoBehaviour
{

    // Use this for initialization
    public void OpenPopup()
    {
        SceneManager.LoadScene("Popup", LoadSceneMode.Single);
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}