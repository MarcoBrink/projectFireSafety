using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    ///<Summary>
    ///Zorgt ervoor dat dit het object dat dit script houdt bewaard blijft.
    ///</Summary>
    private void Awake()
    {
DontDestroyOnLoad(this);
    }

    ///<Summary>
    ///Zorgt ervoor dat onze methode luistert naar het sceneLoaded event wanneer het script aan wordt gezet.
    ///</Summary>
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    ///<Summary>
    ///Zorgt ervoor dat onze methode niet meer luistert naar het sceneLoaded event wanneer het script uit wordt gezet.
    ///</Summary>
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    ///<Summary>
    ///Hiermee wordt de ambient lighting bron naar color gezet om helderheid aan te passen.
    ///</Summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
    }
}
