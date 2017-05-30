using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToEditor : MonoBehaviour {
	//Code om de Scene Editor te openen en deze Scene te sluiten
    public void OpenScene()
    {
        SceneManager.LoadScene("EditorUI", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("EditorUI"));
    }
}
