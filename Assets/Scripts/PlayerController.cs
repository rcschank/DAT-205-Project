using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private CharacterController characterController;

    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;

    private float verticalVelocity = 0f;

    private Vector2 moveInput;
    private Vector3 moveDirection;
    
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

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
        moveInput = moveAction.action.ReadValue<Vector2>();

        moveDirection.x = moveInput.x * speed;
        moveDirection.z = moveInput.y * speed;
        moveDirection.y = 0;

        if (characterController.isGrounded)
        {
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;

        moveDirection = transform.TransformDirection(moveDirection);

        characterController.Move(moveDirection * Time.deltaTime);

    }   


}
