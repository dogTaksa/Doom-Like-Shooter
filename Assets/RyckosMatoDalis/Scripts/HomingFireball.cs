using UnityEngine;

public class HomingFireball : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 3f;
    public float lifeTime = 6f;
    public int damageAmount = 10;
    public float damageCooldown = 2f;

    private Transform target;
    private float lastDamageTime = -Mathf.Infinity;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, turnSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                    Debug.Log("HomingFireball damage dealt: " + damageAmount);
                }
            }
        }

        Destroy(gameObject);
    }
}