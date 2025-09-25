using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    public event EventHandler OnInteractAction;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed; // Subscribe a listener to the performed event
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty); // Fire the OnInteractAction from this class, check for subscribers before firing
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>(); // Returns Vector2 value as per the Action Map Action Action Type

        inputVector = inputVector.normalized;

        return inputVector;
    }

}
