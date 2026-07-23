using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform targetObject;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if(targetObject == null)
        {
            Debug.LogError("Bruh assign target to enemy, stupid.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Update target's position every frame
        if (targetObject != null)
        {
            agent.SetDestination(targetObject.position);
        }
    }
}
