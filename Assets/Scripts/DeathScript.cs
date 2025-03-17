using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public static Vector3 lastCheckpoint = Vector3.zero; // Статическая переменная для хранения последнего положения чекпоинта
    public GameObject Player;

    void Start()
    {
        // Инициализация lastCheckpoint начальной позицией, если чекпоинт еще не пройден
        if (lastCheckpoint == Vector3.zero)
        {
            lastCheckpoint = Player.transform.position; // Или установите конкретную начальную точку
        }
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.transform.position = lastCheckpoint; // Телепортация к последнему чекпоинту
        }
    }
}