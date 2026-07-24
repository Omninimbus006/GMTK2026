using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StatsManager))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private StatsManager stats;
    private Rigidbody rb;
    private PlayerInput input;
    private CharacterController controller;

    private float sensitivity;

    [SerializeField]
    private float maxSpeed = 3f;
    
    [SerializeField]
    private float sprintMultiplier = 1.75f;
    
    [SerializeField]
    private float acceleration = 5f;

    [SerializeField]
    private float jumpForce = 5f;
    
    public Transform head;
    
    public bool Sprinting { get; private set; }

    private int jumpsRemaining;
    
    public bool Crouching { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerMovement.GrabCursor();
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<StatsManager>();
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        input.Jump += OnJump;
        input.Sprint += OnSprint;
        input.Sprint += OnSprintRelease;
        input.Crouch += OnCrouch;
        input.CrouchRelease += OnUncrouch;
        head.GetComponentInChildren<Camera>().fieldOfView = PlayerPrefs.GetFloat("FOV", 40f);
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        jumpsRemaining = (int)stats.GetStat(Stat.Jumps);
    }

    // Update is called once per frame
    private void Update()
    {
        // Handle horizontal movement
        float speed = stats.GetStat(Stat.Speed);
        Vector3 move = input.Movement;
        move.z = move.y;
        move.y = 0f;
        move = transform.TransformDirection(move);
        float baseMaxSpeed = speed * maxSpeed;
        if (Sprinting)
        {
            baseMaxSpeed *= sprintMultiplier;
        }
        move = Vector3.ClampMagnitude(move, baseMaxSpeed) * (speed * Time.deltaTime);
        
        controller.Move(move);
        
        // Handle look movement
        Vector2 look = input.Look * sensitivity;
        Vector3 eulerAngles = head.localEulerAngles;
        eulerAngles.z = 0;
        eulerAngles.y = 0;
        eulerAngles.x -= (look.y * Time.deltaTime * 35f);
        transform.Rotate(Vector3.up, look.x * Time.deltaTime * 35f);
        head.localEulerAngles = eulerAngles;
    }

    public void OnJump()
    {
        if (controller.isGrounded)
        {
            Crouching = false;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (jumpsRemaining > 0)
        {
            jumpsRemaining--;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnCrouch()
    {
        if (controller.isGrounded && !Crouching)
        {
            Crouching = true;
        }
    }

    public void OnSprint()
    {
        Sprinting = true;
    }

    public void OnSprintRelease()
    {
        Sprinting = false;
    }

    public void OnUncrouch()
    {
        if (Crouching)
        {
            Crouching = false;
        }
    }
    
    public static void GrabCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void ReleaseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
