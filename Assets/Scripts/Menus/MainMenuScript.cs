using System.Collections;
using System.Collections.Generic;
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
        SceneManager.LoadScene(name, LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
}
