using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpScript : MonoBehaviour {

    /// <summary>
    /// Close the application.
    /// </summary>
    public void CloseApplication()
    {
        //Afsluiten na het afhandelen van alle threads/nu lopende acties
        Application.Quit();
    }

    /// <summary>
    /// Back to the main menu.
    /// </summary>
    public void ToMainMenu()
    {
        //Menu scene inladen op de Single methode (sluit alle andere scenes)
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        //Menu als actieve scene zetten
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }
}
