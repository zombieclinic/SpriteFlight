using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float thrustForce = 10f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] GameObject boosterFlame;
    [SerializeField] GameObject explosionEffect;

    Rigidbody2D rb;
    PlayerInput playerInput;
    InputAction thrustAction;
    InputAction lookAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  // this is finding the Rigidbody2D and storing it in rb for later use awake starts it right away before any other script

        playerInput = GetComponent<PlayerInput>();  // this is grabing playerInput component and storing it in playerInput
        thrustAction = playerInput.actions["Thrust"]; 
           // Retrieves the "Thrust" InputAction from the Input Actions asset
            // and stores it so we can check if thrust is being pressed
        lookAction = playerInput.actions["Look"]; // doing the same as the thrust but for look 

        if (boosterFlame) boosterFlame.SetActive(false); // this sets boosterflame to false at start
    }

    void Update()
    {
        bool thrustHeld = thrustAction.IsPressed();   // checks the input system to see if thrust is being pressed stores bool in thrustheld
        if (boosterFlame) boosterFlame.SetActive(thrustHeld); // checks if the booster is pressed if so sets value to true

        if (!thrustHeld) return;  // if thruster is not pressed it stops here the rest only happens when the thrust is being pressed

        Vector2 lookScreenPos = lookAction.ReadValue<Vector2>();  // reads the look action as a vector 2  x y
 
        Vector3 screen = new Vector3(    // this builds a 3d screen space and converts it into world position 
            lookScreenPos.x,
            lookScreenPos.y,
            -Camera.main.transform.position.z  // so this will take 10 units in fron of the camera and convert it to z =0i
        );

        Vector3 world = Camera.main.ScreenToWorldPoint(screen);  // converts secreen pixel cords into real world cordinates so we know where we are aiming at
        Vector2 direction = (world - transform.position).normalized;  // computes the direction from player to aim point

        transform.up = direction;  // rotates player 

        rb.AddForce(direction * thrustForce);  // adds force to get the player moving 

        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.GameOver();  // sets game over true and sends it to script game manager
        Instantiate(explosionEffect, transform.position, transform.rotation);  //spawns explosionEffect to cords
        Destroy(gameObject);  // destorys player
    }
}
