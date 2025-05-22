using UnityEngine;

public class PlayerRestore : MonoBehaviour
{
    private PauseMenu pauseMenu;

    void Awake()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenu not found!");
        }
    }

    void Start()
    {
        SaveData data = SaveSystemJSON.Load();
        if (data != null)
        {
            // Получаем компоненты
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            Animator animator = GetComponent<Animator>();

            if (rb != null)
            {
                transform.position = new Vector3(data.checkpointX, data.checkpointY, data.checkpointZ);
                rb.linearVelocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.None; // Сбрасываем ограничения
                rb.isKinematic = false; // Убедимся, что Rigidbody2D не кинематический
                rb.simulated = true; // Убедимся, что физика активна
            }
            else
            {
                transform.position = new Vector3(data.checkpointX, data.checkpointY, data.checkpointZ);
            }

            if (playerMovement != null)
            {
                PlayerMovement.collectibleCount = data.score;
                playerMovement.UpdateCollectibleUI();
                playerMovement.ResetPlayerState();
                playerMovement.enabled = true; // Убедимся, что компонент активен
            }

            if (animator != null)
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsRunning", false);
                animator.enabled = true; // Убедимся, что аниматор активен
            }

            if (pauseMenu != null)
            {
                pauseMenu.ResumeGame(); // Вызываем ResumeGame для полной синхронизации
                Time.timeScale = 1f; // Дополнительно убеждаемся, что время не на паузе
            }

            playerMovement.speed = 5f;
            playerMovement.jump = 300f;
            playerMovement.raycastDistance = 0.2f;

            Debug.Log($"Player restored at position: ({data.checkpointX}, {data.checkpointY}), score: {data.score}");
            Debug.Log($"Time.timeScale: {Time.timeScale}, PauseMenu.isPaused: {pauseMenu.isPaused}, PlayerMovement enabled: {playerMovement.enabled}, Rigidbody simulated: {rb.simulated}");
        }
        else
        {
            Debug.LogWarning("No save data to load.");
        }
    }
}