using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator animator;
    private bool isActive = false;

    void Start()
    {
        animator = GetComponent<Animator>(); // Предполагается, что у спрайта чекпоинта есть Animator с анимациями
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            ActivateCheckpoint();
        }
    }

    void ActivateCheckpoint()
    {
        isActive = true;
        DeathScript.lastCheckpoint = transform.position;

        if (animator != null)
        {
            animator.SetBool("isActive", true); 
        }

        Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint checkpoint in allCheckpoints)
        {
            if (checkpoint != this)
            {
                checkpoint.DeactivateCheckpoint();
            }
        }
    }

    void DeactivateCheckpoint()
    {
        isActive = false;
        if (animator != null)
        {
            animator.SetBool("isActive", false); 
        }
    }
}