using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 300f; // Adjust speed here in Inspector

    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        // Start with the cursor locked and hidden
        LockCursor();
        
        // Initialize rotation to current camera direction
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        yRotation = currentRotation.y;
        xRotation = currentRotation.x;
    }

    void Update()
    {
        // 1. ESCAPE KEY: Unlocks the cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        // 2. MOUSE CLICK: Locks the cursor again (if you want to resume)
        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }

        // 3. LOOK AROUND (Only if cursor is locked)
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}