using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageCooldown = 2f;

    private float lastDamageTime = -Mathf.Infinity;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    Debug.Log("damage dealt: " + damageAmount);
                    playerHealth.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}