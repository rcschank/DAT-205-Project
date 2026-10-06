using UnityEngine;
using UnityEngine.InputSystem;   // The new Input System

// PlayerController
// Attached to the Player prefab. Moves the player with WASD / arrow keys /
// left stick using a CharacterController, and applies simple gravity.
// Turning is handled separately by PlayerLook.
public class PlayerController : MonoBehaviour
{

    // CharacterController is a built-in component for walking characters.
    // It handles collisions with walls without needing a Rigidbody.
    private CharacterController characterController;

    // Points at the Move action in Assets/InputActions/PlayerControls.
    // The action gives us a Vector2: x = left/right, y = forward/back.
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float speed = 5f;         // Units per second
    [SerializeField] private float gravity = -9.81f;   // Real-world gravity in meters per second squared



    // How fast we are falling. Builds up while in the air.
    private float verticalVelocity = 0f;

    private Vector2 moveInput;       // Raw input this frame
    private Vector3 moveDirection;   // Final movement this frame

    // Awake runs before OnEnable and Start, so the reference is ready early
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Input actions must be enabled before they report values.
    // OnEnable / OnDisable run when this component is turned on or off.
    void OnEnable()
    {
        moveAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Playing)  // If there is no GameManager, or the game is not in the Playing state, do not process player movement
        {
            return;
        }

        moveInput = moveAction.action.ReadValue<Vector2>();

        // Input y (up on the keyboard) becomes world z (forward).
        // At this point the direction is relative to the world, not the player.
        moveDirection.x = moveInput.x * speed;
        moveDirection.z = moveInput.y * speed;
        moveDirection.y = 0;

        // Simple gravity: when on the ground, keep a small downward push.
        // If this were 0, the controller would stop touching the ground,
        // isGrounded would flicker on and off, and the player would bounce
        // down stairs one step at a time instead of walking down them.
        if (characterController.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }


        // Convert from "world forward" to "the way the player is facing",
        // so W always moves toward where the camera looks.
        moveDirection = transform.TransformDirection(moveDirection);

        moveDirection.y = verticalVelocity;

        // Multiply by deltaTime so speed is the same no matter the frame rate
        characterController.Move(moveDirection * Time.deltaTime);

    }


}
