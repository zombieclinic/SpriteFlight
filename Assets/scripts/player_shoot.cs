using UnityEngine;

public class player_shoot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 8f;
    [SerializeField] float fireCooldown = 0.25f;

    private float nextFireTime;

    // Called by Input System (Attack action)
    public void OnAttack()
    {
        // Cooldown check
        if (Time.time < nextFireTime)
            return;

        FireBullet();
        nextFireTime = Time.time + fireCooldown;
    }

    private void FireBullet()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            transform.position,
            transform.rotation
        );

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * bulletSpeed;
    }
}
