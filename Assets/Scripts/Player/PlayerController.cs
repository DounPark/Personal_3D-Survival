using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce;
    private Vector2 curMovementInput;
    public LayerMask groundLayer;
    private bool isGrounded = true;
    public float jumpStamina = 20f;
    
    [Header("Camera")]
    public CinemachineFreeLook freeLook;

    private Rigidbody rb;
    private PlayerAnimation playerAnimation;
    private Interaction interaction;
    public Action inventory;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        interaction = GetComponent<Interaction>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void FixedUpdate()
    {
        Move();
        
        bool grounded = IsGrounded();
        if (grounded != isGrounded)
        {
            isGrounded = grounded;
            if (isGrounded)
            {
                playerAnimation?.PlayGrounded();
            }
        }
    }

    void Move()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        Vector3 dir = cameraForward * curMovementInput.y + cameraRight * curMovementInput.x;
        dir.Normalize();
        dir *= moveSpeed;
        dir.y = rb.velocity.y;
        
        rb.velocity = dir;

        if (curMovementInput != Vector2.zero)
        {
            Vector3 moveDirFlat = new Vector3(dir.x, 0f, dir.z);

            if (moveDirFlat != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirFlat);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
            }
        }
        
        bool isMoving = curMovementInput != Vector2.zero;
        playerAnimation?.UpdateMovement(isMoving);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            return;
        
        if (!IsGrounded())
            return;
        if (!CharacterManager.Instance.Player.condition.UseStamina(jumpStamina))
            return;
        
        Jump();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            playerAnimation?.PlayRoll();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerAnimation?.PlayJump();
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f,groundLayer))
            {
                return true;
            }
        }
        return false;
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        interaction.InteractInput(context);
    }

    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void Boost(float amount, float duration)
    {
        StartCoroutine(BoostSpeed(amount, duration));
    }

    private IEnumerator BoostSpeed(float amount, float duration)
    {
        moveSpeed += amount;
        yield return new WaitForSeconds(duration);
        moveSpeed -= amount;
    }
}
