using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // Optional: If you want to rotate a body too

    float xRotation = 0f;

    void Start()
    {
        // This hides the cursor and locks it to the center
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Toggle Cursor Lock when pressing ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None; // Unlock to use Inspector
        }

        // Only lock the cursor if we are NOT hovering over UI
        if (!EventSystem.current.IsPointerOverGameObject()) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; // Ensure it's hidden
            }
        }

        // ONLY rotate if the cursor is locked
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            if (playerBody != null)
            {
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }
    }
}