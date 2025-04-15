using UnityEngine;

public class BossHealthUI : MonoBehaviour
{
    public float maxHealth = 500f;
    private float currentHealth;

    [Header("UI")]
    public RectTransform bossHealthBar;
    private Vector3 originalScale;
    private Vector3 targetScale;

    public float shrinkSpeed = 10f;

    void Start()
    {
        currentHealth = maxHealth;

        if (bossHealthBar != null)
        {
            originalScale = bossHealthBar.localScale;
            targetScale = originalScale;
            bossHealthBar.pivot = new Vector2(0f, 0.5f);
        }
    }

    void Update()
    {
        if (bossHealthBar != null)
        {
            bossHealthBar.localScale = Vector3.Lerp(bossHealthBar.localScale, targetScale, Time.deltaTime * shrinkSpeed);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        float percent = currentHealth / maxHealth;
        targetScale = new Vector3(originalScale.x * percent, originalScale.y, originalScale.z);

        if (currentHealth <= 0)
        {
            StartCoroutine(DieWhenBarGone());
        }
    }

    System.Collections.IEnumerator DieWhenBarGone()
    {
        while (bossHealthBar != null && bossHealthBar.localScale.x > 0.01f)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}