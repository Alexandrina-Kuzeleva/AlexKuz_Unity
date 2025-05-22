using UnityEngine;

public class PlayerRestore : MonoBehaviour
{
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
                // Устанавливаем позицию через Rigidbody2D
                rb.MovePosition(new Vector2(data.checkpointX, data.checkpointY));
                rb.linearVelocity = Vector2.zero; // Сбрасываем скорость
            }
            else
            {
                // Запасной вариант, если Rigidbody2D отсутствует
                transform.position = new Vector3(data.checkpointX, data.checkpointY, data.checkpointZ);
            }

            if (playerMovement != null)
            {
                // Устанавливаем счет
                PlayerMovement.collectibleCount = data.score;
                playerMovement.UpdateCollectibleUI();

                // Сбрасываем состояние движения
                playerMovement.ResetPlayerState();
            }

            if (animator != null)
            {
                // Сбрасываем параметры анимации
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsRunning", false);
            }

            Debug.Log($"Player restored at position: ({data.checkpointX}, {data.checkpointY}), score: {data.score}");
        }
        else
        {
            Debug.LogWarning("No save data to load.");
        }
    }
}