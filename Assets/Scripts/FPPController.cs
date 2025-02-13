using UnityEngine;

public class FPPController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float verticalSpeed = 5f;
    public Camera fpsCamera;

    private float xRotation = 0f; // For vertical camera rotation
    private float mouseSensitivity = 2f; // Mouse sensitivity

    void Start()
    {
        // If the camera isn't assigned, find it in children
        if (fpsCamera == null)
        {
            fpsCamera = GetComponentInChildren<Camera>();
        }

        // Lock the cursor for FPS control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        // Get input for movement
        float moveX = Input.GetAxis("Horizontal"); // A/D keys
        float moveZ = Input.GetAxis("Vertical");   // W/S keys

        // Determine movement speed
        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        // Calculate movement direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move *= currentSpeed * Time.deltaTime;

        // Vertical movement
        if (Input.GetKey(KeyCode.Space))
        {
            move.y += verticalSpeed * Time.deltaTime; // Ascend
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            move.y -= verticalSpeed * Time.deltaTime; // Descend
        }

        // Apply movement
        transform.position += move;
    }

    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate vertically and clamp rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        fpsCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
