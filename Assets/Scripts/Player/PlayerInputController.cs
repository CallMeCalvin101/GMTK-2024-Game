using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInputController : MonoBehaviour
{
    private PlayerControls playerControls;
    
    private InputAction move;
    private InputAction jump;
    private InputAction interact;

    private Rigidbody rb;

    public GameObject cameraObj;
    public Rigidbody shipRb;

    public float distanceToFloor = 5f;
    public float moveSpeed = 1;

    public float maxVelocity = 10f;

    public float interactRange = 10f;


    private bool isOnShip = false;
    private bool isGrounded = false;

    private void Awake()
    {
        playerControls = inputSingleton.instance.GetControls();

        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        interact = playerControls.Player.Interact;

        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        playerControls.Player.Enable();

        // Subscribe input event
        jump.started += Jump;
        interact.started += Interact;
    }
    private void OnDisable()
    {
        //playerControls.Player.Enable();
        UnsubscribeEvents();
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    private void UnsubscribeEvents() 
    {
        jump.started -= Jump;
        interact.started -= Interact;
    }
    // Run when jump is called
    private void Jump(InputAction.CallbackContext ctx)
    {
        //todo Detect when grounded
        if(!isGrounded)
            return;

        Vector3 jumpForce = new Vector3(0f, 30f, 0f);

        rb.AddForce(jumpForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, cameraObj.transform.forward * interactRange, Color.green);
        //Ship movement
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, distanceToFloor))
        {
            isGrounded = true;
            // Check if ship or not
            if (hit.rigidbody.gameObject.layer == LayerMask.GetMask("boat"))
            {
                Debug.DrawRay(transform.position, Vector3.down * distanceToFloor, Color.red);
                isOnShip = true;
            } else {
                isOnShip = false;
            }

        } else
        {
            isGrounded = false;
            Debug.DrawRay(transform.position, Vector3.down * distanceToFloor, Color.white);
        }
        Vector3 moveDirection = move.ReadValue<Vector3>();
        

        if (isOnShip)
        {
            MovePlayerOnShip(moveDirection);
        } else
        {
            // Lande Movement
            MovePlayer(moveDirection);
        }
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        int layer_mask = LayerMask.GetMask("interactable");

        if(Physics.Raycast(transform.position, cameraObj.transform.forward, out RaycastHit hit, interactRange, layer_mask))
        {
            print(hit.collider.gameObject.name);
            // Interact with ship
            var shipScript = hit.collider.gameObject.GetComponent<shipDriveController>();

            shipScript.enterSteering(gameObject);
            
        }
    }
    private void MovePlayer(Vector3 moveDirection)
    {
        var forwardMove = moveDirection.y;
        var rightMove = moveDirection.x;

        Vector3 movement3d = transform.forward * forwardMove + transform.right * rightMove;
        rb.AddForce(movement3d.normalized * moveSpeed, ForceMode.Force);


        // Clamp Speed
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }
    private void MovePlayerOnShip(Vector3 moveDirection)
    {
        var forwardMove = moveDirection.y;
        var rightMove = moveDirection.x;

        Vector3 movement3d = transform.forward * forwardMove + transform.right * rightMove;
        rb.AddForce(shipRb.velocity + movement3d.normalized * moveSpeed, ForceMode.Force);


        // Clamp Speed
        if (rb.velocity.magnitude > maxVelocity + shipRb.velocity.magnitude)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }
}
