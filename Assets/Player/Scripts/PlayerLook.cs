using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    void Start()
    {
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 lookDirection = PlayerInputs.GetAimingDirection();
        lookDirection *= Time.deltaTime;
        lookDirection.x *= sensX;
        lookDirection.y *= sensY;
        
        yRotation += lookDirection.x;
        xRotation -= lookDirection.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam & orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
