using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToPopup : MonoBehaviour
{

    // Code om de PopUp te openen en deze Scene te sluiten
    public void OpenPopup()
    {
	//popup moet toch additive en kleiner dan deze scene om een echte popup te zijn?
        SceneManager.LoadScene("Popup", LoadSceneMode.Single);
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}