using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float thrustForce = 2.0f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] GameObject boosterFlame;
    [SerializeField] GameObject explosionEffect;

    Rigidbody2D rb;
    PlayerInput playerInput;

    InputAction thrustAction;
    InputAction lookAction;

    bool thrustHeld;
    Vector2 lookScreenPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // Grab actions from the PlayerInput's Actions Asset
        thrustAction = playerInput.actions["Thrust"];
        lookAction   = playerInput.actions["Look"];

        if (boosterFlame) boosterFlame.SetActive(false);
    }

    void OnEnable()
    {
        thrustAction.performed += OnThrustPerformed;
        thrustAction.canceled  += OnThrustCanceled;

        lookAction.performed += OnLook;
        lookAction.canceled  += OnLook;
    }

    void OnDisable()
    {
        thrustAction.performed -= OnThrustPerformed;
        thrustAction.canceled  -= OnThrustCanceled;

        lookAction.performed -= OnLook;
        lookAction.canceled  -= OnLook;
    }

    void OnThrustPerformed(InputAction.CallbackContext ctx)
    {
        thrustHeld = true;
        if (boosterFlame) boosterFlame.SetActive(true);
    }

    void OnThrustCanceled(InputAction.CallbackContext ctx)
    {
        thrustHeld = false;
        if (boosterFlame) boosterFlame.SetActive(false);
    }

    void OnLook(InputAction.CallbackContext ctx)
    {
        lookScreenPos = ctx.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (!thrustHeld) return;

        Vector3 screen = new Vector3(
            lookScreenPos.x,
            lookScreenPos.y,
            -Camera.main.transform.position.z
        );

        Vector3 world = Camera.main.ScreenToWorldPoint(screen);
        Vector2 direction = (world - transform.position).normalized;

        transform.up = direction;

        rb.AddForce(direction * thrustForce);

        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.GameOver();
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
