using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    /// <summary>
    /// Go to the specified scene.
    /// </summary>
    /// <param name="name">The internal name of the scene.</param>
    public void OpenScene(string name)
    {
        //Een scene laden, in single mode zodat deze scene gesloten wordt
        SceneManager.LoadScene(name, LoadSceneMode.Single);
        //En deze actief zetten
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
}
