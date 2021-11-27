using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;

    private void Start()
    {
        Time.timeScale = 1f;

        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1 ? true:false;
        vsyncToggle.isOn = PlayerPrefs.GetInt("VSync") == 1 ? true:false;

        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int CurrentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string Option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(Option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
        resolutionDropdown.RefreshShownValue();
    }

    // Main Menu
    public void Continue(){}

    public void StartGame()
    {
        // This will load the scene that is next to the MainMenu in the build order.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Tutorial(){}

    public void Quit()
    {
        Application.Quit();
    }

    // Options Menu
    public void MasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void MusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void ChangeResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }

    public void FullScreen(bool toggle)
    {
        Screen.fullScreen = toggle;
        PlayerPrefs.SetInt("Fullscreen", toggle ? 1:0);
    }

    public void VSync(bool toggle)
    {
        QualitySettings.vSyncCount = toggle ? 1:0;
        PlayerPrefs.SetInt("VSync", toggle ? 1:0);
    }

    public void RestoreDefaults()
    {
        masterSlider.value = 0f;
        musicSlider.value = 0f;
        sfxSlider.value = 0f;
        fullscreenToggle.isOn = false;
    }
}
