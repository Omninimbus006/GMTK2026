using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StatsManager))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    private StatsManager stats;
    private Rigidbody rb;
    private PlayerInput input;

    private float sensitivity;
    
    public Transform head;
    
    public bool Grounded { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerMovement.GrabCursor();
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<StatsManager>();
        input = GetComponent<PlayerInput>();
        head.GetComponentInChildren<Camera>().fieldOfView = PlayerPrefs.GetFloat("FOV", 40f);
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
    }

    // Update is called once per frame
    private void Update()
    {
        // Handle horizontal movement
        Vector3 horizontalForce = input.Movement;
        horizontalForce.z = horizontalForce.y;
        horizontalForce.y = 0;
        horizontalForce = Vector3.ClampMagnitude(horizontalForce, 1f);
        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.z = horizontalVelocity.y;
        horizontalVelocity.y = 0;
        horizontalForce -= (Vector3.ClampMagnitude(horizontalVelocity, 1f) * horizontalForce.magnitude) / Mathf.Clamp(stats.GetStat(Stat.Speed), 0.1f, Mathf.Infinity) * (100f * Time.deltaTime);
        
        rb.AddForce(horizontalForce, ForceMode.Force);
        
        Grounded = Physics.CheckSphere(transform.position - Vector3.down, 0.25f, LayerMask.GetMask("Ground"));
        
        // Handle look movement
        Vector2 look = input.Look * sensitivity;
        Vector3 eulerAngles = head.localEulerAngles;
        eulerAngles.z = 0;
        eulerAngles.y = 0;
        eulerAngles.x = Mathf.Clamp(eulerAngles.x - (look.y * Time.deltaTime * 30f), -89f, 89f);
        transform.Rotate(Vector3.up, look.x * Time.deltaTime * 30f);
        head.localEulerAngles = eulerAngles;
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
