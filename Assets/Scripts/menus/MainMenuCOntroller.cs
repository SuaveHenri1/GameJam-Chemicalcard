using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject optionsPanel;

    [Header("Cenas do Jogo")]
    [Tooltip("Mundo, Player, Puzzles")]
    public string[] cenasParaCarregar = new string[] { "Mundo", "Player", "Puzzles" };

    [Header("Configurações de Áudio e Vídeo")]
    public AudioMixer mainAudioMixer;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetupResolutions();
    }


    public void PlayGame()
    {
        if (cenasParaCarregar.Length == 0)
        {
            Debug.LogWarning("Nenhuma cena configurada para carregar no MenuManager!");
            return;
        }

        SceneManager.LoadScene(cenasParaCarregar[0], LoadSceneMode.Single);

        for (int i = 1; i < cenasParaCarregar.Length; i++)
        {
            SceneManager.LoadScene(cenasParaCarregar[i], LoadSceneMode.Additive);
        }
    }

    public void QuitGame()
    {
        Debug.Log("O jogo está a fechar...");
        Application.Quit();
    }


    public void OpenOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
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