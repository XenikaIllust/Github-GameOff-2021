using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] _resolutions;

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
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;
        vsyncToggle.isOn = PlayerPrefs.GetInt("VSync") == 1 ? true : false;

        _resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width
                && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
        resolutionDropdown.RefreshShownValue();
    }

    // Main Menu
    public void Continue()
    {
        LevelManager.Instance.LoadLastLevel();
    }

    public void StartGame()
    {
        LevelManager.Instance.LoadNewGame();
    }

    public void Tutorial()
    {
    }

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
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }

    public void FullScreen(bool toggle)
    {
        Screen.fullScreen = toggle;
        PlayerPrefs.SetInt("Fullscreen", toggle ? 1 : 0);
    }

    public void VSync(bool toggle)
    {
        QualitySettings.vSyncCount = toggle ? 1 : 0;
        PlayerPrefs.SetInt("VSync", toggle ? 1 : 0);
    }

    public void RestoreDefaults()
    {
        masterSlider.value = 0f;
        musicSlider.value = 0f;
        sfxSlider.value = 0f;
        fullscreenToggle.isOn = false;
    }
}
