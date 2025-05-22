using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject firePointObject; 
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float nextFireTime;
    private Transform firePoint; 

    void Start()
    {
        if (firePointObject != null)
        {
            firePoint = firePointObject.transform; // Получаем Transform из GameObject
        }
        else
        {
            Debug.LogError("FirePoint Object is not assigned!");
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
        if (firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = firePoint.right * bulletSpeed;
            Debug.Log("Bullet created at position: " + firePoint.position);
        }
    }
}