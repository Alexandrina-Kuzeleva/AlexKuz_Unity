using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 screenBounds;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }
        UpdateScreenBounds();
    }

    void Update()
    {
        UpdateScreenBounds(); // Обновляем границы каждый кадр
        Vector3 cameraPosition = mainCamera.transform.position;
        
        // Проверяем, вышла ли пуля за границы экрана с учётом позиции камеры
        if (transform.position.x > cameraPosition.x + screenBounds.x ||
            transform.position.x < cameraPosition.x - screenBounds.x ||
            transform.position.y > cameraPosition.y + screenBounds.y ||
            transform.position.y < cameraPosition.y - screenBounds.y)
        {
            Debug.Log($"Bullet destroyed at position: ({transform.position.x}, {transform.position.y}), Screen bounds: ({screenBounds.x}, {screenBounds.y}), Camera position: ({cameraPosition.x}, {cameraPosition.y})");
            Destroy(gameObject);
        }
    }

    void UpdateScreenBounds()
    {
        if (mainCamera.orthographic) // Предполагаем ортографическую камеру
        {
            float cameraHeight = mainCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            screenBounds = new Vector2(cameraWidth / 2, cameraHeight / 2);
            Debug.Log($"Updated screen bounds: {screenBounds}, Camera orthographic size: {mainCamera.orthographicSize}, Aspect: {mainCamera.aspect}");
        }
        else
        {
            Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
            Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0f));
            screenBounds = new Vector2((topRight.x - bottomLeft.x) / 2, (topRight.y - bottomLeft.y) / 2);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Уничтожаем врага
            Destroy(gameObject); // Уничтожаем пулю
        }
    }
}