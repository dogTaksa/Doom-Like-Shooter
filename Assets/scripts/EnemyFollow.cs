using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFollow : MonoBehaviour
{
    public float stoppingDistance = 5;
    
    private NavMeshAgent agent;
    private Transform target;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(target == null) return;
        
        var distance = Vector3.Distance(transform.position, target.position);
        
        if (distance > stoppingDistance)
            agent.SetDestination(target.position);
        else 
            agent.SetDestination(transform.position);
    }
}
