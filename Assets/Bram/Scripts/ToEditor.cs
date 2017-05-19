using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToEditor : MonoBehaviour {

    public void OpenScene()
    {
        SceneManager.LoadScene("Editor", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Editor"));
    }
}
