using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float sensitivity = 0.1f;

    private Vector2 lookInput;
    private float verticalRotation = 0f;

    void OnEnable()
    {
        lookAction.action.Enable();
    }

    void OnDisable()
    {
        lookAction.action.Disable();
    }

    void Update()
    {

        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)  // If the game is not in the Playing state, do not process player movement
        {
            return;
        }

       lookInput = lookAction.action.ReadValue<Vector2>();

       transform.Rotate(0f, lookInput.x * sensitivity, 0f);

       verticalRotation -= lookInput.y * sensitivity;
       verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
       playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

}
