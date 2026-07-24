using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StatsManager))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    private StatsManager stats;
    private Rigidbody rb;
    private PlayerInput input;
    private CapsuleCollider col;

    private float sensitivity;

    [SerializeField]
    private float maxSpeed = 5f;
    
    [SerializeField]
    private float sprintMultiplier = 2f;

    [SerializeField]
    private float jumpForce = 5f;
    
    [SerializeField]
    private float acceleration = 0.2f;

    [SerializeField]
    private float airAcceleration = 0.08f;

    [SerializeField]
    private float normalDrag = 0.1f;
    
    [SerializeField]
    private float slidingDrag = 0.01f;

    [SerializeField]
    private PhysicsMaterial normalPhysics;

    [SerializeField]
    private PhysicsMaterial slidingPhysics;
    
    public Transform head;
    
    public bool Sprinting { get; private set; }
    
    public bool Grounded { get; private set; }

    private int jumpsRemaining;
    
    public bool Crouching { get; private set; }

    private float speed;

    private int maxJumps;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerMovement.GrabCursor();
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<StatsManager>();
        input = GetComponent<PlayerInput>();
        col = GetComponent<CapsuleCollider>();
        stats.OnStatModifierChanged += OnStatChanged;
        col.material = normalPhysics;
        input.Jump += OnJump;
        input.Sprint += OnSprint;
        input.SprintRelease += OnSprintRelease;
        input.Crouch += OnCrouch;
        input.CrouchRelease += OnUncrouch;
        head.GetComponentInChildren<Camera>().fieldOfView = PlayerPrefs.GetFloat("FOV", 40f);
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        maxJumps = (int)stats.GetStat(Stat.Jumps);
        jumpsRemaining = maxJumps;
        speed = stats.GetStat(Stat.Speed);
    }

    private void OnStatChanged(StatModifierChangedEventArgs args)
    {
        if (args.Stat == Stat.Speed)
        {
            speed = stats.GetStat(Stat.Speed);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Handle horizontal movement
        if (!Crouching)
        {
            Vector3 wishDir = rb.transform.TransformDirection(new Vector3(input.Movement.x, 0, input.Movement.y));
            Vector3 velocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        
            float baseMaxSpeed = maxSpeed * speed;
            if (Sprinting)
            {
                baseMaxSpeed *= sprintMultiplier;
            }
        
            wishDir = wishDir.normalized;
            Vector3 direction = wishDir * baseMaxSpeed - velocity;
            float newAcceleration = Grounded ? acceleration : airAcceleration;
            newAcceleration *= direction.magnitude * 2f;
        
            direction = direction.normalized * (wishDir == Vector3.zero ? newAcceleration * 2 : newAcceleration);
            float magn = direction.magnitude;
            direction = direction.normalized;
            direction *= magn;
        
            rb.AddForce(direction, ForceMode.Acceleration);   
        }
        
        if (!Grounded)
        {
            Grounded = Physics.CheckSphere(transform.position + Vector3.down * 0.5f, 0.3f, LayerMask.GetMask("Ground"));
            if (Grounded)
            {
                jumpsRemaining = (int)stats.GetStat(Stat.Jumps);

                if (Crouching)
                {
                    rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up);
                }
            }
        }
        else
        {
            Grounded = Physics.CheckSphere(transform.position + Vector3.down * 0.5f, 0.3f, LayerMask.GetMask("Ground"));
        }
        
        // Handle look movement
        Vector2 look = input.Look * sensitivity;
        Vector3 eulerAngles = head.localEulerAngles;
        eulerAngles.z = 0;
        eulerAngles.y = 0;
        eulerAngles.x -= (look.y * Time.deltaTime * 35f);
        transform.Rotate(Vector3.up, look.x * Time.deltaTime * 35f);
        head.localEulerAngles = eulerAngles;
    }

    private void FixedUpdate()
    {
        col.height = Crouching ? Mathf.Max(0.6f, col.height - Time.fixedDeltaTime * 10f) : Mathf.Min(1.54f, col.height + Time.fixedDeltaTime * 10f);
    }
    
    public void OnJump()
    {
        if (Grounded || jumpsRemaining > 0)
        {
            jumpsRemaining--;
            
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnCrouch()
    {
        if (Grounded && !Crouching)
        {
            col.material = slidingPhysics;
            rb.linearDamping = slidingDrag;
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
            col.material = normalPhysics;
            rb.linearDamping = normalDrag;
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
