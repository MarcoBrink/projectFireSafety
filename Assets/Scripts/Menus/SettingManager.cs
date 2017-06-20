using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour {
	//De objecten om de settings mee aan te passen
	//Fullscreen aan/uit zetten
    public Toggle fullscreenToggle;
	//Dropdown menu om de resolutie te selecteren
    public Dropdown resolutionDropdown;
	//Slider om het volume in te stellen
    public Slider volumeSlider;
	//Mute aan/uit zetten
    public Toggle muteVolumeToggle;
	//Slider om het helderder/duisterder te maken
    public Slider brightnessSlider;
	//Knop om de instellingen op te slaan
    public Button applyButton;

	//AudioSource opslaan
    public AudioSource audioSource;
	//Array voor de resoluties
    public Resolution[] resolutions;
	//DemoSettings object
    public DemoSettings demoSettings;

	/// <summary>
    /// Als de settings aangezet worden
    /// </summary>
    void OnEnable()
    {
	//Nieuw Demosettings object aanmaken
        demoSettings = new DemoSettings();
	//Bijhouden welke instellingen er aangepast en opgeslagen worden
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });
        muteVolumeToggle.onValueChanged.AddListener(delegate { OnMuteVolumeToggle(); });
        brightnessSlider.onValueChanged.AddListener(delegate { OnBrightnessChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });
	//Alle ondersteunde resoluties van het scherm opvragen en in de array zetten
        resolutions = Screen.resolutions;

	//Alle ondersteunde resoluties in het dropdown menu zetten
        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

	//Al bestaande/standaard settings inladen
        LoadSettings();
    }

	/// <summary>
    /// Functie om Fullscreen aan/uit te zetten
    /// </summary>
    public void OnFullscreenToggle()
    {
        Screen.fullScreen = demoSettings.fullscreen = fullscreenToggle.isOn;
    }

	/// <summary>
    /// Functie om de resolutie aan te passen
    /// </summary>
    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        demoSettings.resolutionIndex = resolutionDropdown.value;
    }

	/// <summary>
    /// Functie om het volume aan te passen
    /// </summary>
    public void OnVolumeChange()
    {
        audioSource.volume = demoSettings.volume = volumeSlider.value;
    }

    /// <summary>
    /// Functie om het Mute aan/uit te zetten
    /// </summary>
    public void OnMuteVolumeToggle()
    {
        bool muted = muteVolumeToggle.isOn;

        if (muted)
            audioSource.volume = 0.0F;
        else
            audioSource.volume = demoSettings.volume;
    }

    /// <summary>
    /// Functie om de helderheid aan te passen
    /// </summary>
    public void OnBrightnessChange()
    {
        RenderSettings.ambientLight = new Color(demoSettings.rbgValue, demoSettings.rbgValue, demoSettings.rbgValue, 1);
        demoSettings.rbgValue = brightnessSlider.value;
    }

    /// <summary>
    /// Functie om de opslag van de settings aan te roepen
    /// </summary>
    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    /// <summary>
    /// Functie om de instellingen op te slaan
    /// </summary>
    public void SaveSettings()
    {
	//Wordt momenteel opgeslagen als JSON data
        string jsonData = JsonUtility.ToJson(demoSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/demosettings.json", jsonData);
    }

    /// <summary>
    /// Functie om de instellingen in te laden
    /// </summary>   
    public void LoadSettings()
    {
	//JSON data uitlezen van de settings
        demoSettings = JsonUtility.FromJson<DemoSettings>(File.ReadAllText(Application.persistentDataPath + "/demosettings.json"));

	//En deze instellingen toepassen
        fullscreenToggle.isOn = demoSettings.fullscreen;
        resolutionDropdown.value = demoSettings.resolutionIndex;
        volumeSlider.value = demoSettings.volume;
        Screen.fullScreen = demoSettings.fullscreen;
        muteVolumeToggle.isOn = demoSettings.muted;

	//In het dropdown menu de juiste waarde laten zien
        resolutionDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Terug naar het hoofdmenu
    /// </summary>
    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }
}
