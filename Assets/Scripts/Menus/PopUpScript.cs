using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpScript : MonoBehaviour {

    /// <summary>
    /// Close the application.
    /// </summary>
    public void CloseApplication()
    {
        Application.Quit();
    }

    /// <summary>
    /// Back to the main menu.
    /// </summary>
    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }
}
