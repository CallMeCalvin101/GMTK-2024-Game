using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
//using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;

public class shipDriveController : MonoBehaviour
{
    private PlayerControls controls;

    private GameObject driver;
    public Rigidbody shipRb;

    public GameObject standingPosition;

    private InputAction move;
    private InputAction leave;

    public float maxSpeed = 10f;
    public float minSpeedToTurn = 1f;
    public float forwardAcceleration = 2f;
    public float maxTurnSpeed = 1f;
    public GameObject turningPointRef;

    private void Awake()
    {
        controls = inputSingleton.instance.GetControls();
    }
    private void OnEnable() 
    {
        controls.Ship.Disable();
        ;
        move = controls.Ship.Move;
        leave = controls.Ship.Leave;

        leave.started += leaveShip;
    }
    private void OnDisable() 
    {
        //controls.Disable();
        UnsubscribeEvents();
    }
    private void OnDestroy() 
    {
        UnsubscribeEvents();
    }
    private void UnsubscribeEvents () 
    {
        leave.started -= leaveShip;
    }
    private void FixedUpdate()
    {
        if (driver == null)
            return;

        Vector3 moveDirection = move.ReadValue<Vector3>();
        MoveShip(moveDirection);
    }
    private void leaveShip(InputAction.CallbackContext ctx)
    {
        
        if (driver == null) return;


        //driver.transform.localPosition = Vector3.zero;
        driver.transform.SetParent(null, true);

        driver.GetComponent<PlayerInputController>().enabled = true;


        driver = null;
        controls.Ship.Disable();

    }
    public void enterSteering(GameObject newDriver) {
        if (driver != null) return;


        driver = newDriver;

        // lock driver in position
        driver.transform.SetParent(transform, false);

        driver.transform.localPosition = standingPosition.transform.localPosition;

        driver.GetComponent<PlayerInputController>().enabled = false;
        controls.Ship.Enable();
    }

    public void MoveShip(Vector3 moveInput)
    {
        float forwardInput = moveInput.y;
        float rightTurnInput = moveInput.x;

        // forward direction 
        shipRb.AddForce(shipRb.transform.forward * forwardInput * forwardAcceleration, ForceMode.Acceleration);

        // turning


        // Reduce Turning
        float turningPercentage = Mathf.Clamp01(maxTurnSpeed / (shipRb.velocity.magnitude));

        shipRb.AddForceAtPosition(shipRb.transform.right * rightTurnInput * turningPercentage, turningPointRef.transform.position, ForceMode.Acceleration);
        
        if (shipRb.velocity.magnitude > maxSpeed)
            shipRb.velocity = Vector3.ClampMagnitude(shipRb.velocity, maxSpeed);
    }


}
