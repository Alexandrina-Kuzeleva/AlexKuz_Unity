using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Скорость пули
    public float damage = 25f; // Урон от пули
    private Vector2 direction; // Направление движения пули
    private Camera mainCamera;
    private Vector2 screenBounds;
    private Rigidbody2D rb; // Ссылка на Rigidbody2D

    void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        direction = transform.right; // Направление по умолчанию (вправо)
        rb = GetComponent<Rigidbody2D>(); // Получаем Rigidbody2D
        if (rb != null)
        {
            rb.linearVelocity = direction * speed; // Устанавливаем начальную скорость
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on Bullet!");
        }

        // Уничтожаем пулю через 5 секунд, если она не попала
        Destroy(gameObject, 5f);
    }

    // Публичный метод для установки направления
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Нормализуем направление
        if (rb != null)
        {
            rb.linearVelocity = direction * speed; // Обновляем скорость при изменении направления
        }
    }

    void Update()
    {
        // Уничтожаем пулю, если она вышла за пределы экрана
        if (Mathf.Abs(transform.position.x) > screenBounds.x || Mathf.Abs(transform.position.y) > screenBounds.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, есть ли HealthSystem и это не игрок
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem != null && !healthSystem.isPlayer)
        {
            healthSystem.TakeDamage(damage); // Наносим урон
            Destroy(gameObject); // Уничтожаем пулю
        }
        // Уничтожаем пулю при столкновении с другими объектами (кроме игрока)
        else if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}