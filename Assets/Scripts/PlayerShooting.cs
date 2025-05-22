using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject firePointObject;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    public float spawnOffset = 0.5f; // Смещение спавна пули влево/вправо
    private float nextFireTime;
    private Transform firePoint;
    private PlayerMovement playerMovement; // Ссылка на PlayerMovement для получения направления

    void Start()
    {
        if (firePointObject != null)
        {
            firePoint = firePointObject.transform;
        }
        else
        {
            Debug.LogError("FirePoint Object is not assigned!");
        }

        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextFireTime)
        {
            Shoot();
            Debug.Log("Shooting");
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (firePoint != null && playerMovement != null)
        {
            // Определяем направление персонажа по transform.localScale.x
            float direction = transform.localScale.x > 0 ? 1f : -1f;

            // Добавляем смещение в зависимости от направления
            Vector2 spawnPosition = firePoint.position + new Vector3(spawnOffset * direction, 0f, 0f);

            // Создаём пулю с учётом смещения
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // Устанавливаем скорость пули в зависимости от направления
            rb.linearVelocity = new Vector2(bulletSpeed * direction, 0f);

            // Разворачиваем пулю в зависимости от направления
            SpriteRenderer bulletSprite = bullet.GetComponent<SpriteRenderer>();
            if (bulletSprite != null)
            {
                bulletSprite.flipX = direction < 0; // Разворачиваем спрайт, если стреляем влево
            }

            Debug.Log($"Bullet created at position: {spawnPosition}, Direction: {direction}, FlipX: {direction < 0}");
        }
    }
}