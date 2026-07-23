using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 Movement { get; private set; }
    public Vector2 Look { get; private set; }

    public event Action Jump;
    public event Action Crouch;
    public event Action CrouchRelease;

    public event Action Sprint;
    public event Action SprintRelease;
    
    public event Action Interact;
    
    public event Action PrimaryFire;
    public event Action PrimaryFireRelease;
    public event Action SecondaryFire;
    public event Action SecondaryFireRelease;

    public event Action OffensiveAbilityActivate;
    public event Action DefensiveAbilityActivate;
    public event Action UtilityAbilityActivate;
    
    private InputAction move;
    private InputAction look;

    private void Start()
    {
        InputSystem.actions.Enable();
        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        InputSystem.actions.FindAction("Jump").started += OnJumpPressed;
        InputSystem.actions.FindAction("Crouch").started += OnCrouchPressed;
        InputSystem.actions.FindAction("Crouch").canceled += OnCrouchReleased;
        InputSystem.actions.FindAction("Sprint").started += OnSprintPressed;
        InputSystem.actions.FindAction("Sprint").canceled += OnSprintReleased;
        InputSystem.actions.FindAction("Interact").started += OnInteractPressed;
        InputSystem.actions.FindAction("Primary Fire").started += OnPrimaryFirePressed;
        InputSystem.actions.FindAction("Primary Fire").canceled += OnPrimaryFireReleased;
        InputSystem.actions.FindAction("Secondary Fire").started += OnSecondaryFirePressed;
        InputSystem.actions.FindAction("Secondary Fire").canceled += OnSecondaryFireReleased;
        InputSystem.actions.FindAction("Offensive Ability").started += OnOffensiveAbilityPressed;
        InputSystem.actions.FindAction("Defensive Ability").started += OnDefensiveAbilityPressed;
        InputSystem.actions.FindAction("Utility Ability").started += OnUtilityAbilityPressed;
    }
    
    // Update is called once per frame
    private void Update()
    {
        Movement = move.ReadValue<Vector2>();
        Look = look.ReadValue<Vector2>();
    }

    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        Jump?.Invoke();
    }

    private void OnCrouchPressed(InputAction.CallbackContext context)
    {
        Crouch?.Invoke();
    }

    private void OnCrouchReleased(InputAction.CallbackContext context)
    {
        CrouchRelease?.Invoke();
    }

    private void OnInteractPressed(InputAction.CallbackContext context)
    {
        Interact?.Invoke();
    }

    private void OnPrimaryFirePressed(InputAction.CallbackContext context)
    {
        PrimaryFire?.Invoke();
    }

    private void OnPrimaryFireReleased(InputAction.CallbackContext context)
    {
        PrimaryFireRelease?.Invoke();
    }
    
    private void OnSecondaryFirePressed(InputAction.CallbackContext context)
    {
        SecondaryFire?.Invoke();
    }

    private void OnSecondaryFireReleased(InputAction.CallbackContext context)
    {
        SecondaryFireRelease?.Invoke();
    }

    private void OnSprintPressed(InputAction.CallbackContext context)
    {
        Sprint?.Invoke();
    }

    private void OnSprintReleased(InputAction.CallbackContext context)
    {
        SprintRelease?.Invoke();
    }
    
    private void OnOffensiveAbilityPressed(InputAction.CallbackContext context)
    {
        OffensiveAbilityActivate?.Invoke();
    }

    private void OnDefensiveAbilityPressed(InputAction.CallbackContext context)
    {
        DefensiveAbilityActivate?.Invoke();
    }
    
    private void OnUtilityAbilityPressed(InputAction.CallbackContext context)
    {
        UtilityAbilityActivate?.Invoke();
    }
}
