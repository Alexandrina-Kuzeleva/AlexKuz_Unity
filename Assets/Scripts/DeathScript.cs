using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public static Vector3 lastCheckpoint = Vector3.zero; // Последний чекпоинт
    public GameObject Player;
    private HealthSystem healthSystem; // Ссылка на систему здоровья игрока

    // Для урона от ловушек при длительном контакте
    private float damageInterval = 0.1f; // Интервал нанесения урона (в секундах)
    private float lastDamageTime; // Время последнего нанесения урона
    private bool isOnTrap; // Флаг, находится ли игрок на ловушке

    void Start()
    {
        // Инициализация начального чекпоинта
        if (lastCheckpoint == Vector3.zero)
        {
            lastCheckpoint = Player.transform.position;
        }
        healthSystem = Player.GetComponent<HealthSystem>();
        lastDamageTime = Time.time; // Инициализация времени
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Урон от ловушек при первом контакте
            if (gameObject.CompareTag("Trap"))
            {
                healthSystem.TakeDamage(20f); // Начальный урон от ловушки
                isOnTrap = true; // Игрок вступил на ловушку
                Debug.Log("Is trap");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Trap"))
        {
            // Нанесение урона с интервалом, пока игрок находится на ловушке
            if (Time.time - lastDamageTime >= damageInterval)
            {
                healthSystem.TakeDamage(10f); // Урон при длительном контакте
                lastDamageTime = Time.time; // Обновляем время последнего урона
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Trap"))
        {
            isOnTrap = false; // Игрок покинул ловушку
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Pit"))
        {
            // Падение в яму: обнуление HP и телепортация
            healthSystem.TakeDamage(healthSystem.currentHP); // Обнуляем HP
        }
    }
}