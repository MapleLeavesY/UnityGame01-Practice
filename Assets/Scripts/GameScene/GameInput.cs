using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance
    {
        private set;
        get;
    }
    public event Action OnMenuButtonPressed;
    Actions actions;
    private void Awake()
    {   
        Instance = this;
        actions = new Actions();
        actions.Enable();
        actions.Player.Menu.performed += Menu_Performed;

    }
    private void Menu_Performed(InputAction.CallbackContext context)
    {
        OnMenuButtonPressed?.Invoke();
    }

    public bool IsUpactionPressed()
    {
        return actions.Player.LanderUp.IsPressed();
    }
    public bool IsLeftactionPressed()
    {
        return actions.Player.LanderLeft.IsPressed();
    }
    public bool IsRightactionPressed()
    {
        return actions.Player.LanderRight.IsPressed();
    }

    public Vector2 GetMovementInputVector2()
    {
        return actions.Player.Movement.ReadValue<Vector2>();
    }

    public void GameInputClear()
    {
        actions.Disable();
    }
}
