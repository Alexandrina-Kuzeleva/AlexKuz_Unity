using UnityEngine;

public class CrocodileEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolSpeed = 2f; // Скорость патрулирования
    public float patrolRange = 3f; // Диапазон патрулирования (от начальной точки)
    private Vector2 startPosition; // Начальная позиция крокодила
    private bool movingRight = true; // Направление движения

    [Header("Chase Settings")]
    public float chaseSpeed = 4f; // Скорость погони
    public float detectionRange = 5f; // Дальность обнаружения игрока
    public Transform player; // Ссылка на игрока
    private bool isChasing = false; // Флаг погони

    [Header("Attack Settings")]
    public float damage = 20f; // Урон от атаки
    private float attackCooldown = 1f; // Задержка между атаками
    private float lastAttackTime; // Время последней атаки

    [Header("Sprite Settings")]
    public bool facesRightByDefault = false; // Укажите, смотрит ли спрайт вправо по умолчанию

    [Header("Ground Check Settings")]
    public LayerMask groundLayer; // Слой земли/платформ
    public float groundCheckDistance = 0.2f; // Дистанция проверки земли
    public Transform groundCheck; // Точка для проверки земли (ниже крокодила)

    private Animator animator; // Для управления анимациями
    private HealthSystem healthSystem; // Для получения урона
    private Rigidbody2D rb; // Для управления физикой
    private bool isGrounded; // Находится ли крокодил на земле

    void Start()
    {
        startPosition = transform.position; // Сохраняем начальную позицию
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody2D>();

        // Проверка на наличие компонентов
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on " + gameObject.name);
        }
        if (healthSystem == null)
        {
            Debug.LogError("HealthSystem component is missing on " + gameObject.name);
        }
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on " + gameObject.name);
        }
        if (groundCheck == null)
        {
            Debug.LogError("Ground Check Transform is not assigned on " + gameObject.name);
        }

        // Находим игрока по тегу
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player with tag 'Player' not found in the scene!");
            }
        }

        // Устанавливаем начальное состояние анимации
        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
        }
    }

    void Update()
    {
        if (player == null || animator == null || healthSystem == null || rb == null) return;

        // Проверяем, находится ли крокодил на земле
        CheckGround();

        // Проверяем, видит ли крокодил игрока
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true; // Начинаем погоню
            animator.SetBool("IsAttacking", true); // Открываем пасть
        }
        else
        {
            isChasing = false; // Возвращаемся к патрулированию
            animator.SetBool("IsAttacking", false); // Закрываем пасть
        }

        // Движение крокодила
        if (isGrounded)
        {
            if (isChasing)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
            }
        }

        // Поворот спрайта в зависимости от направления
        UpdateSpriteDirection();
    }

    void CheckGround()
    {
        // Проверяем, есть ли земля под крокодилом
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    void Patrol()
    {
        // Определяем целевую позицию для патрулирования
        Vector2 targetPosition = movingRight
            ? new Vector2(startPosition.x + patrolRange, transform.position.y)
            : new Vector2(startPosition.x - patrolRange, transform.position.y);

        // Проверяем, есть ли платформа впереди
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D edgeCheck = Physics2D.Raycast(groundCheck.position, direction, 0.6f, groundLayer);
        RaycastHit2D groundAhead = Physics2D.Raycast(groundCheck.position + (Vector3)(direction * 0.6f), Vector2.down, groundCheckDistance, groundLayer);

        // Если впереди нет платформы или есть препятствие, меняем направление
        if (!groundAhead || edgeCheck.collider != null)
        {
            movingRight = !movingRight;
        }

        // Двигаем крокодила
        float speed = movingRight ? patrolSpeed : -patrolSpeed;
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
    }

    void ChasePlayer()
    {
        // Определяем направление к игроку
        float directionToPlayer = player.position.x - transform.position.x;
        float speed = directionToPlayer > 0 ? chaseSpeed : -chaseSpeed;

        // Проверяем, есть ли платформа впереди
        Vector2 direction = directionToPlayer > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D groundAhead = Physics2D.Raycast(groundCheck.position + (Vector3)(direction * 0.6f), Vector2.down, groundCheckDistance, groundLayer);

        // Двигаем только если впереди есть платформа
        if (groundAhead)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Останавливаемся у края
        }
    }

    void UpdateSpriteDirection()
    {
        // Определяем направление движения
        float direction = isChasing ? (player.position.x - transform.position.x) : (movingRight ? 1 : -1);

        // Учитываем изначальную ориентацию спрайта
        if (facesRightByDefault)
        {
            if (direction > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            if (direction > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time - lastAttackTime >= attackCooldown)
        {
            HealthSystem playerHealth = other.gameObject.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                lastAttackTime = Time.time;
                animator.SetBool("IsAttacking", true);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time - lastAttackTime >= attackCooldown)
        {
            HealthSystem playerHealth = other.gameObject.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                lastAttackTime = Time.time;
                animator.SetBool("IsAttacking", true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Визуализация проверки земли
        if (groundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance);
        }
    }
}