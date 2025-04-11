using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBoardInput : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the character movement

    private Vector2 movement;

    public InputAction movementAction;

    void Update()
    {
        // // Get input from arrow keys or WASD keys
        // movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow
        // movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down Arrow

        // // Move the character based on the input
        // transform.Translate(movement * moveSpeed * Time.deltaTime);

        // Get input from arrow keys or WASD keys
        movement = movementAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Move the character based on the input
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
    

    public void OnEnable()
    {
        movementAction.Enable();
    }

    public void OnDisable()
    {
        movementAction.Disable();
    }
}