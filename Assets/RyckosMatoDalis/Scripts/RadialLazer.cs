using UnityEngine;

public class RadialLaser : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    public int damageAmount = 10; // Amount of damage to deal
    public float damageCooldown = 2f; // Time before damage can be dealt again

    private float lastDamageTime = -Mathf.Infinity; // Keep track of the cooldown timer

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
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
                    Debug.Log("RadialLaser damage dealt: " + damageAmount);
                }
            }
        }

        Destroy(gameObject);
    }
}