using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    [Header("Paineis Pause")]
    public GameObject pauseMenuPanel;
    public GameObject optionsPanel;

    [Header("Opcoes de Jogo")]
    public AudioMixer mainAudioMixer;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    public static bool isGamePaused = false;
    
    void Start()
    {
        if (pauseMenuPanel != null) 
        { 
            pauseMenuPanel.SetActive(false);
        }
        if (optionsPanel != null) 
        {
            optionsPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        isGamePaused = false;

        SetupResolutions();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            if (isGamePaused)
            {
                if (optionsPanel != null && optionsPanel.activeSelf)
                {
                    CloseOptions();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }  
    }


    // Funcoes do Pause
    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        if(optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void QuitTOMainMenu() 
    {
        Time.timeScale = 0f;
        isGamePaused = false;

        SceneManager.LoadScene(0);
    }


    // Funcoes Opcoes
    public void OpenOptions()
    {
        pauseMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        if (mainAudioMixer != null)
        {
            mainAudioMixer.SetFloat("MasterVolume", volume);
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutions == null || resolutions.Length == 0) return;

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetupResolutions()
    {
        if (resolutionDropdown == null) return;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
}
