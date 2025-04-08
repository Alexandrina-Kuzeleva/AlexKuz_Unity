using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public TextMeshProUGUI musicVolumeText; // Текст для отображения громкости музыки
    public TextMeshProUGUI sfxVolumeText; // Текст для отображения громкости звуков
    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        SetupSliders();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void SetupSliders()
    {
        if (musicMixer != null && musicVolumeSlider != null)
        {
            float musicVolume;
            musicMixer.GetFloat("BackgroundMusicVolume", out musicVolume);
            musicVolumeSlider.value = Mathf.Pow(10, musicVolume / 20);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            UpdateMusicVolumeText(musicVolumeSlider.value);
        }

        if (sfxMixer != null && sfxVolumeSlider != null)
        {
            float sfxVolume;
            sfxMixer.GetFloat("SFXVolume", out sfxVolume);
            sfxVolumeSlider.value = Mathf.Pow(10, sfxVolume / 20);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
            UpdateSFXVolumeText(sfxVolumeSlider.value);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicMixer != null)
        {
            float dB = volume > 0 ? 20 * Mathf.Log10(volume) : -80f;
            musicMixer.SetFloat("BackgroundMusicVolume", dB);
            UpdateMusicVolumeText(volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxMixer != null)
        {
            float dB = volume > 0 ? 20 * Mathf.Log10(volume) : -80f;
            sfxMixer.SetFloat("SFXVolume", dB);
            UpdateSFXVolumeText(volume);
        }
    }

    private void UpdateMusicVolumeText(float volume)
    {
        if (musicVolumeText != null)
        {
            musicVolumeText.text = $"Music Volume: {(int)(volume * 100)}%";
        }
    }

    private void UpdateSFXVolumeText(float volume)
    {
        if (sfxVolumeText != null)
        {
            sfxVolumeText.text = $"SFX Volume: {(int)(volume * 100)}%";
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}