using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public RectTransform healthBar;

    private float originalWidth;
    private float barHeight;
    private Vector2 targetSize;

    public float shrinkSpeed = 10f;

    [Header("Death Effect")]
    public GameObject deathEffect;

    [Header("Death Effect Rotation")]
    public float xRotation = 90f;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            originalWidth = healthBar.sizeDelta.x;
            barHeight = healthBar.sizeDelta.y;
            targetSize = new Vector2(originalWidth, barHeight);
            healthBar.pivot = new Vector2(0f, 0.5f);
        }
    }

    void Update()
    {
        if (healthBar != null)
        {
            healthBar.sizeDelta = Vector2.Lerp(healthBar.sizeDelta, targetSize, Time.deltaTime * shrinkSpeed);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        float percent = currentHealth / maxHealth;
        targetSize = new Vector2(originalWidth * percent, barHeight);

        if (currentHealth <= 0)
        {
            StartCoroutine(DieWhenBarGone());
        }
    }

    IEnumerator DieWhenBarGone()
    {
        while (healthBar != null && healthBar.sizeDelta.x > 0.01f)
        {
            yield return null;
        }

        Destroy(gameObject);

        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.Euler(xRotation, 0f, 0f));
            Destroy(effect, 10f);
        }
    }
}
