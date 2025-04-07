using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Панель меню паузы
    public AudioMixer musicMixer; // Микшер для музыки
    public AudioMixer sfxMixer; // Микшер для звуков (SFX)
    public Slider musicVolumeSlider; // Слайдер для громкости музыки
    public Slider sfxVolumeSlider; // Слайдер для громкости звуков
    private bool isPaused = false;

    void Start()
    {
        // Убедимся, что панель изначально выключена
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Настраиваем слайдеры
        SetupSliders();
    }

    void Update()
    {
        // Открытие/закрытие меню паузы по клавише Esc
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

    // Настройка начальных значений слайдеров
    private void SetupSliders()
    {
        // Устанавливаем начальные значения слайдеров на основе текущей громкости
        if (musicMixer != null && musicVolumeSlider != null)
        {
            float musicVolume;
            musicMixer.GetFloat("BackgroundMusicVolume", out musicVolume);
            musicVolumeSlider.value = Mathf.Pow(10, musicVolume / 20); // Преобразуем dB в линейное значение
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxMixer != null && sfxVolumeSlider != null)
        {
            float sfxVolume;
            sfxMixer.GetFloat("SFXVolume", out sfxVolume);
            sfxVolumeSlider.value = Mathf.Pow(10, sfxVolume / 20); // Преобразуем dB в линейное значение
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    // Установка громкости музыки
    public void SetMusicVolume(float volume)
    {
        if (musicMixer != null)
        {
            // Преобразуем линейное значение слайдера (0-1) в dB (-80 до 0)
            float dB = volume > 0 ? 20 * Mathf.Log10(volume) : -80f;
            musicMixer.SetFloat("BackgroundMusicVolume", dB);
        }
    }

    // Установка громкости звуков
    public void SetSFXVolume(float volume)
    {
        if (sfxMixer != null)
        {
            // Преобразуем линейное значение слайдера (0-1) в dB (-80 до 0)
            float dB = volume > 0 ? 20 * Mathf.Log10(volume) : -80f;
            sfxMixer.SetFloat("SFXVolume", dB);
        }
    }

    // Пауза игры
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Останавливаем время в игре
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    // Возврат в игру
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Возобновляем время
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    // Выход из игры
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Для тестирования в редакторе
        #endif
    }
}
