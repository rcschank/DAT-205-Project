using UnityEngine;
using UnityEngine.InputSystem;

// PlayerLook
// Attached to the Player prefab. Mouse look for a first-person camera.
// Left/right turns the whole player body. Up/down tilts only the camera,
// so the body never leans backward.
public class PlayerLook : MonoBehaviour
{
    [SerializeField] private InputActionReference lookAction;  // The Look action (mouse delta / right stick)
    [SerializeField] private Transform playerCamera;           // The camera, a child of the player
    [SerializeField] private float sensitivity = 0.1f;         // Degrees of rotation per pixel of mouse movement

    [SerializeField] private bool lookUpAndDown = true; // Variable to control whether the player can look up and down or not
    private Vector2 lookInput;
    private float verticalRotation = 0f;  // Current up/down angle of the camera, kept so we can clamp it

    // Lock and hide the mouse cursor while the player exists,
    // and give it back when the player is disabled or destroyed.
    void OnEnable()
    {
        lookAction.action.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        lookAction.action.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {

        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Playing)  // If there is no GameManager, or the game is not in the Playing state, do not process mouse look
        {
            return;
        }

       // Mouse delta is already "how far the mouse moved this frame",
       // so it is NOT multiplied by Time.deltaTime.
       lookInput = lookAction.action.ReadValue<Vector2>();

       // Turn the player left/right around the Y (up) axis
       transform.Rotate(0f, lookInput.x * sensitivity, 0f);

       // Subtract because moving the mouse up gives a positive y,
       // but a positive X rotation tilts the camera down
       verticalRotation -= lookInput.y * sensitivity;
       verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);  // Stop the camera from flipping over the top
       if (lookUpAndDown)
       {
           // localRotation is relative to the parent (the player), so this
           // only tilts the camera and leaves the left/right turn alone
           playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
       }
    }

}
