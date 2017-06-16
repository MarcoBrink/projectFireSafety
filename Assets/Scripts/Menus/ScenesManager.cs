using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    //Zorgt ervoor dat dit het object dat dit script houdt bewaard blijft.
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    //Zorgt ervoor dat onze methode luistert naar het sceneLoaded event wanneer het script aan wordt gezet.
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Zorgt ervoor dat onze methode niet meer luistert naar het sceneLoaded event wanneer het script uit wordt gezet.
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Hiermee wordt de ambient lighting bron naar color gezet om helderheid aan te passen.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
    }
}
