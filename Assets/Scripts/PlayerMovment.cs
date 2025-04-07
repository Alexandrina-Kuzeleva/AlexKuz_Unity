using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private float Move;
    public float jump = 300f;
    private Rigidbody2D rb;
    private bool isJumping;
    public Animator animator;
    public float raycastDistance = 0.2f;
    public LayerMask groundLayer;
    public int collectibleCount = 0;
    public Text collectibleText;
    public AudioSource collectSound;
    public AudioSource jumpSound; // Звук прыжка
    public AudioSource stepSound; // Звук шагов
    public AudioMixer mixer; // Ваш Audio Mixer

    // Задержка между звуками шагов
    private float stepDelay = 0.3f; // Настройте под ваш ритм шагов
    private float lastStepTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateCollectibleUI();
    }

    void Update()
    {
        Move = Input.GetAxisRaw("Horizontal"); // Используем GetAxisRaw для более точного ввода
        rb.linearVelocity = new Vector2(speed * Move, rb.linearVelocity.y);
        animator.SetBool("IsRunning", Move != 0);

        // Прыжок
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.AddForce(new Vector2(rb.linearVelocity.x, jump));
                isJumping = true;
                animator.SetBool("IsJumping", true);
                if (jumpSound != null)
                {
                    jumpSound.Play();
                }
            }
        }

        // Проверка приземления
        if (IsGrounded() && isJumping)
        {
            isJumping = false;
            animator.SetBool("IsJumping", false);
        }

        // Управление звуком шагов
        if (IsGrounded() && Mathf.Abs(Move) > 0.1f && Time.time - lastStepTime > stepDelay)
        {
            if (stepSound != null)
            {
                stepSound.Play();
                lastStepTime = Time.time;
            }
        }
        else if (stepSound != null && stepSound.isPlaying && (Mathf.Abs(Move) <= 0.1f || !IsGrounded()))
        {
            // Останавливаем звук шагов, если игрок стоит или в воздухе
            stepSound.Stop();
        }

        // Поворот персонажа
        if (Move > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (Move < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    bool IsGrounded()
    {
        Vector2 rayStart = new Vector2(transform.position.x, transform.position.y - 0.85f);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, raycastDistance, groundLayer);
        if (hit.collider != null)
        {
            float angle = Vector2.Angle(hit.normal, Vector2.up);
            return angle < 45f;
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            collectibleCount++;
            Destroy(other.gameObject);
            if (collectSound != null)
            {
                collectSound.Play();
            }
            UpdateCollectibleUI();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 rayStart = new Vector2(transform.position.x, transform.position.y - 0.85f);
        Gizmos.DrawRay(rayStart, Vector2.down * raycastDistance);
    }

    void UpdateCollectibleUI()
    {
        if (collectibleText != null)
        {
            collectibleText.text = "Count: " + collectibleCount;
        }
    }
}