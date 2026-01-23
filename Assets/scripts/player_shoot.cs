
using UnityEngine;
using UnityEngine.InputSystem;

public class player_shoot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 8f;
    [SerializeField] float fireCooldown = 0.25f;
    private float nextFireTime;
    private PlayerInput playerInput;
    private InputAction AttackAction;
    

    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip[] laserSounds;




    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        AttackAction = playerInput.actions["Attack"];
    }

    void OnEnable()
    {
       AttackAction.performed += OnAttackPerformed;
    }

     void OnDisable()
    {
        AttackAction.performed -= OnAttackPerformed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (Time.time < nextFireTime) return;

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
        PlayRandom();

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * bulletSpeed;
    }

    
    private void PlayRandom()
    {
        if (laserSounds.Length == 0) return;

        int Index = Random.Range(0, laserSounds.Length);
        audioSource.pitch = Random.Range (0.95f, 1.05f);
        audioSource.PlayOneShot(laserSounds[Index]);
    }
}
