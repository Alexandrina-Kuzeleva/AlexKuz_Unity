using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    public CanvasGroup loadingScreen; // Панель для эффекта загрузки
    public float fadeDuration = 1f; // Длительность затухания

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void ReloadCurrentScene()
    {
        StartCoroutine(LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // Плавное затемнение
        if (loadingScreen != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                loadingScreen.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
                yield return null;
            }
        }

        // Асинхронная загрузка сцены
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;

        // Ждём завершения загрузки
        while (!asyncLoad.isDone)
        {
            // Можно добавить прогресс загрузки, если есть UI элемент (например, Slider)
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // Плавное осветление
        if (loadingScreen != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                loadingScreen.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
                yield return null;
            }
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Плавное затемнение
        if (loadingScreen != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                loadingScreen.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
                yield return null;
            }
        }

        // Асинхронная загрузка сцены
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Ждём завершения загрузки
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // Плавное осветление
        if (loadingScreen != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                loadingScreen.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
                yield return null;
            }
        }
    }
}