using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public Transform target;
    public int health = 100;
    public int damage = 10;
    public int viewDistance = 10;
    public int patrolDistance = 5;
    
    public float attackCooldown = 0.5f;
    public float attackLength = 1.5f;
    
    private NavMeshAgent agent;
    private bool attacking;
    Vector3 randomDestination;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        randomDestination = transform.position;
        InvokeRepeating(nameof(RandomDestination), 2f, 3f);
        
        print(target);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        
        print(distance);
        if (distance <= viewDistance && target != null && agent.enabled) agent.SetDestination(target.position);
        else agent.SetDestination(randomDestination);
        
        if (distance - agent.stoppingDistance < 0.3f && !attacking)
        {
            StartCoroutine(AttackPlayer());
        }
    }
    
    void RandomDestination()
    {
        float randomZ = Random.Range(-patrolDistance, patrolDistance);
        float randomX = Random.Range(-patrolDistance, patrolDistance);
        
        randomDestination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }

    IEnumerator AttackPlayer()
    {
        attacking = true;
        agent.SetDestination(transform.position);
        
        //var health = target.gameObject.GetComponent<>();  // Prideti player script
        //if (health != null) health.TakeDamage(damage);
        
        yield return new WaitForSeconds(attackLength); // skirta attack animationui
        
        yield return new WaitForSeconds(attackCooldown); // cooldown tarp attack
        attacking = false;
        agent.enabled = true;
    }
    
    void Die()
    {
        agent.enabled = false;
        Destroy(this);
    }

    public void TakeDamage(int damageAmount) // can be used in player script
    {
        health -= damageAmount;
        
        if(health <= 0) Die();
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
