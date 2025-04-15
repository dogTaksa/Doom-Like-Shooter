using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public TMP_Text healthText;
    public Image crackOverlay;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (crackOverlay != null)
            crackOverlay.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (crackOverlay != null && currentHealth <= 20)
            crackOverlay.enabled = true;

        if (currentHealth <= 0)
            Die();
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = currentHealth.ToString();
    }

    void Die()
    {
        Debug.Log("Player died!");
    }
}
