using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Скорость пули
    private Vector2 direction; // Направление движения пули
    private Camera mainCamera;
    private Vector2 screenBounds;

    void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Устанавливаем направление пули на основе её поворота
        direction = transform.right; // transform.right учитывает поворот объекта
    }

    void Update()
    {
        // Двигаем пулю в заданном направлении
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Уничтожаем пулю, если она вышла за границы экрана
        if (Mathf.Abs(transform.position.x) > screenBounds.x || Mathf.Abs(transform.position.y) > screenBounds.y)
        {
            Destroy(gameObject);
        }
    }
}