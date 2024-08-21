using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    public GameObject cameraObj;

    private PlayerControls playerControls;

    private InputAction mouse;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private void Awake()
    {
        playerControls = inputSingleton.instance.GetControls();

        mouse = playerControls.Player.Mouse;

        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        Vector2 mouseDelta = mouse.ReadValue<Vector2>();
        MoveCamera(mouseDelta);
    }

    private void MoveCamera(Vector2 mouseDelta)
    {
        // Camera follows mouse
        yaw += mouseDelta.x * 0.55f;
        pitch -= mouseDelta.y * 0.55f;

        // Limit y movement so can't flip camera around.
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        cameraObj.transform.position = transform.position;
        cameraObj.transform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);

        transform.transform.eulerAngles = new Vector3(0f, yaw, 0.0f);

    }
}
