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

    public Transform head;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<StatsManager>();
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 horizontalForce = input.Movement;
        horizontalForce.y = 0;
        horizontalForce = Vector3.ClampMagnitude(horizontalForce, 1f);
        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0;
        horizontalForce -= (Vector3.ClampMagnitude(horizontalVelocity, 1f) * horizontalForce.magnitude) / stats.GetStat(Stat.Speed);
        
        rb.AddForce(horizontalForce, ForceMode.Force);
    }
}
