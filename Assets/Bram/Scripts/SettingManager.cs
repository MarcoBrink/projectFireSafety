using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour {
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Toggle muteVolumeToggle;
    public Slider brightnessSlider;
    public Button applyButton;

    public AudioSource audioSource;
    public Resolution[] resolutions;
    public DemoSettings demoSettings;

    void OnEnable()
    {
        demoSettings = new DemoSettings();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });
        muteVolumeToggle.onValueChanged.AddListener(delegate { OnMuteVolumeToggle(); });
        brightnessSlider.onValueChanged.AddListener(delegate { OnBrightnessChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });

        resolutions = Screen.resolutions;

        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();
    }

    public void OnFullscreenToggle()
    {
        Screen.fullScreen = demoSettings.fullscreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        demoSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void OnVolumeChange()
    {
        audioSource.volume = demoSettings.volume = volumeSlider.value;
    }

    public void OnMuteVolumeToggle()
    {
        bool muted = muteVolumeToggle.isOn;

        if (muted)
            audioSource.volume = 0.0F;
        else
            audioSource.volume = demoSettings.volume;
    }

    public void OnBrightnessChange()
    {
        RenderSettings.ambientLight = new Color(demoSettings.rbgValue, demoSettings.rbgValue, demoSettings.rbgValue, 1);
        demoSettings.rbgValue = brightnessSlider.value;
    }

    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(demoSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/demosettings.json", jsonData);
    }
    
    public void LoadSettings()
    {
        demoSettings = JsonUtility.FromJson<DemoSettings>(File.ReadAllText(Application.persistentDataPath + "/demosettings.json"));

        fullscreenToggle.isOn = demoSettings.fullscreen;
        resolutionDropdown.value = demoSettings.resolutionIndex;
        volumeSlider.value = demoSettings.volume;
        Screen.fullScreen = demoSettings.fullscreen;
        muteVolumeToggle.isOn = demoSettings.muted;

        resolutionDropdown.RefreshShownValue();
    }
}
