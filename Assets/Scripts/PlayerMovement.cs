using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform camTransform;
    private float mouseSens = 300f, moveSpeed = 5f, vRotation, mouseX, mouseY, kbH, kbV, camFOV = 60f;
    private enum MovementState {Normal, Sprinting, Crouching}
    private MovementState CurrentMovementState;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        camTransform = GetComponentInChildren<Camera>().transform;
        camFOV = GetComponentInChildren<Camera>().fieldOfView;
    }
    void Update()
    {
        StateCheck();
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSens;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSens;
        kbH = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        kbV = Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;
        Move();
    }

    void StateCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (kbH != 0 || kbV != 0)
            {
                CurrentMovementState = MovementState.Sprinting;
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            CurrentMovementState = MovementState.Crouching;
        }
        else
        {
            CurrentMovementState = MovementState.Normal;
        }
        switch (CurrentMovementState)
        {
            case MovementState.Normal:
                moveSpeed = 5f;
                Camera.main.fieldOfView = 60f;
                camTransform.localPosition = new Vector3(0, 0.75f, 0);
                break;
            case MovementState.Sprinting:
                moveSpeed = 10f;
                Camera.main.fieldOfView = 70f;
                break;
            case MovementState.Crouching:
                moveSpeed = 2.5f;
                camTransform.localPosition = new Vector3(0, 0.25f, 0);
                break;
        }
    }

    void Move()
    {
        vRotation -= mouseY;
        vRotation = Mathf.Clamp(vRotation, -90, 90);
        camTransform.localEulerAngles = new Vector3(vRotation, 0, 0);
        transform.Rotate(0, mouseX, 0);
        transform.Translate(kbH, 0, kbV);
    }
}
