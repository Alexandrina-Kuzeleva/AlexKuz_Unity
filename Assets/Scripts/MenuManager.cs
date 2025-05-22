using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_1");
    }
    
    public void ContinueGame()
    {
        SaveData data = SaveSystemJSON.Load();
        if (data != null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(data.sceneName);
            // позицию и очки восстановим позже в самой сцене
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    
}