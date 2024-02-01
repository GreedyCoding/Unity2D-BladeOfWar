using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 movementInput;

    public bool shootInput;
    public bool dashInput;


    PlayerInput playerInput;

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        playerInput = new PlayerInput();
        playerInput.DefaultMap.Enable();

        playerInput.DefaultMap.Move.performed += SetMoveInput;
        playerInput.DefaultMap.Move.canceled += SetMoveInput;

        playerInput.DefaultMap.Shoot.performed += SetShootInput;
        playerInput.DefaultMap.Shoot.canceled += ResetShootInput;

        playerInput.DefaultMap.Dash.performed += SetDashInput;
        playerInput.DefaultMap.Dash.canceled += ResetDashInput;
    }

    private void UnsubscribeFromEvents()
    {
        playerInput.DefaultMap.Move.performed -= SetMoveInput;
        playerInput.DefaultMap.Move.canceled -= SetMoveInput;

        playerInput.DefaultMap.Shoot.performed -= SetShootInput;
        playerInput.DefaultMap.Shoot.canceled -= ResetShootInput;

        playerInput.DefaultMap.Dash.performed -= SetDashInput;
        playerInput.DefaultMap.Dash.canceled -= ResetDashInput;

        playerInput.DefaultMap.Disable();
    }

    private void SetMoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void SetShootInput(InputAction.CallbackContext context)
    {
        shootInput = true;
    }
    private void ResetShootInput(InputAction.CallbackContext context)
    {
        shootInput = false;
    }

    private void SetDashInput(InputAction.CallbackContext context)
    {
        dashInput = true;
    }
    private void ResetDashInput(InputAction.CallbackContext context)
    {
        dashInput = false;
    }
}
