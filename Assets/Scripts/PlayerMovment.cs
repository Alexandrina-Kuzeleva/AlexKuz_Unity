using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

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
    public AudioSource jumpSound;
    public AudioSource stepSound;
    public AudioMixer mixer;

    // Для выстрела
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private bool canShoot = true;
    private float nextFireTime;
    // Задержка между звуками шагов
    private float stepDelay = 0.3f;
    private float lastStepTime;

    // Ссылка на систему здоровья
    private HealthSystem healthSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>(); // Получаем компонент HealthSystem
        UpdateCollectibleUI();
    }

    void Update()
    {
        Move = Input.GetAxisRaw("Horizontal");
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

        // Выстрел
        if (Input.GetKeyDown(KeyCode.F) && canShoot && Time.time >= nextFireTime)
        {
            StartCoroutine(Shoot());
            nextFireTime = Time.time + fireRate;
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
            StartCoroutine(FadeMusic());
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                // Устанавливаем направление через метод
                float direction = transform.localScale.x > 0 ? 1 : -1;
                bulletScript.SetDirection(new Vector2(direction, 0));
            }
        }
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private IEnumerator FadeMusic()
    {
        if (mixer == null)
        {
            yield break;
        }

        float fadeTime = 0.5f;
        float targetVolume = -20f;
        float originalVolume = 0f;

        float currentTime = 0f;
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(originalVolume, targetVolume, currentTime / fadeTime);
            mixer.SetFloat("BackgroundMusicVolume", newVolume);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        currentTime = 0f;
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(targetVolume, originalVolume, currentTime / fadeTime);
            mixer.SetFloat("BackgroundMusicVolume", newVolume);
            yield return null;
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