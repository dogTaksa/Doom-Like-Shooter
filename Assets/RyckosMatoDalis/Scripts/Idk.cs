using UnityEngine;

public class BossAttackDamage : MonoBehaviour
{
    public int damageAmount = 10;

    void Start()
    {
        if (CompareTag("Volt"))
        {
            GameObject[] volts = GameObject.FindGameObjectsWithTag("Volt");
            Collider myCollider = GetComponent<Collider>();

            foreach (GameObject volt in volts)
            {
                if (volt == gameObject) continue;

                Collider otherCollider = volt.GetComponent<Collider>();
                if (myCollider != null && otherCollider != null)
                {
                    Physics.IgnoreCollision(myCollider, otherCollider);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null)
                health.TakeDamage(damageAmount);
        }

        Destroy(gameObject);
    }
}