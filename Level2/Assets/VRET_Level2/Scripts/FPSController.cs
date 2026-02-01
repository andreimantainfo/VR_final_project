using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    [Header("References")]
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private float verticalRotation = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // If camera not assigned, try to find it
        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        // Check if grounded
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Determine speed
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Calculate movement direction
        Vector3 move = (transform.right * horizontal + transform.forward * vertical).normalized;
        characterController.Move(move * speed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    // Allow unlocking cursor with Escape key
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}