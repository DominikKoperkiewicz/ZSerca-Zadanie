using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputActions playerInputActions;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float movementSpeed = 5.0f;

    private Vector2 moveDirection;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.Player.Movement.performed += Move;
        playerInputActions.Player.Movement.canceled += Move;
    }
    private void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Player.Movement.performed -= Move;
        playerInputActions.Player.Movement.canceled -= Move;
    }

    private void Move(InputAction.CallbackContext context)
    {
        rb.linearVelocityX = context.ReadValue<float>() * movementSpeed;
    }
}
