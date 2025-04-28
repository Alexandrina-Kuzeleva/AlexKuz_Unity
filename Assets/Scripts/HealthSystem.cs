using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public float maxHP = 100f; // Максимальное здоровье
    public float currentHP; // Текущее здоровье
    public Slider hpSlider; // UI Slider для отображения HP (для игрока)
    public bool isPlayer = false; // Флаг для идентификации игрока

    [Header("Flash Settings")]
    public float flashDuration = 0.5f; // Длительность мигания
    public int flashCount = 3; // Количество миганий
    public Color flashColor = Color.red; // Цвет мигания

    private DeathScript deathScript; // Ссылка для телепортации игрока
    private Image hpFillImage; // Компонент Image для заполнения слайдера
    private Color originalColor; // Исходный цвет заполнения
    private bool isFlashing; // Флаг мигания

    void Start()
    {
        currentHP = maxHP;
        UpdateHPSlider();

        if (isPlayer)
        {
            deathScript = GetComponent<DeathScript>();

            // Находим компонент Image для заполнения слайдера
            if (hpSlider != null)
            {
                Transform fillTransform = hpSlider.transform.Find("Fill Area/Fill");
                if (fillTransform != null)
                {
                    hpFillImage = fillTransform.GetComponent<Image>();
                    originalColor = hpFillImage.color; // Сохраняем исходный цвет (зелёный)
                }
                else
                {
                    Debug.LogError("Fill Image not found in Slider!");
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP = Mathf.Max(currentHP - damage, 0);
        UpdateHPSlider();

        // Если это игрок и слайдер существует, запускаем мигание
        if (isPlayer && hpSlider != null && !isFlashing)
        {
            StartCoroutine(FlashHPSlider());
        }

        if (currentHP <= 0)
        {
            if (isPlayer)
            {
                // Телепортация игрока к последнему чекпоинту
                transform.position = DeathScript.lastCheckpoint;
                ResetHP();
            }
            else
            {
                // Для врагов: просто уничтожаем
                Destroy(gameObject);
            }
        }
    }

    public void ResetHP()
    {
        currentHP = maxHP;
        UpdateHPSlider();
    }

    private void UpdateHPSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHP / maxHP;
        }
    }

    private IEnumerator FlashHPSlider()
    {
        isFlashing = true;
        float flashInterval = flashDuration / (flashCount * 2); // Делим на 2, так как каждый цикл включает два состояния (красный и нормальный)

        for (int i = 0; i < flashCount; i++)
        {
            // Меняем цвет на красный
            hpFillImage.color = flashColor;
            yield return new WaitForSeconds(flashInterval);

            // Возвращаем исходный цвет
            hpFillImage.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
        }

        // Убедимся, что цвет вернулся к исходному
        hpFillImage.color = originalColor;
        isFlashing = false;
    }
}