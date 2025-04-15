using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    public float maxStamina = 3f; // Maximum stamina charges
    public float currentStamina;  // Current stamina
    public Image staminaBarImage; // Reference to the stamina image UI
    public float dashCost = 1f;  // Stamina cost per dash
    public float staminaRegenerationRate = 0.2f; // Stamina regeneration rate per second
    public float regenCooldown = 5f; // Time before stamina starts regenerating
    private float lastDashTime;   // Time of last dash

    void Start()
    {
        currentStamina = maxStamina; // Set initial stamina to max
        lastDashTime = Time.time;    // Initialize dash cooldown
    }

    void Update()
    {
        HandleStaminaRegeneration();
        UpdateStaminaBar();
    }

    void HandleStaminaRegeneration()
    {
        if (Time.time - lastDashTime >= regenCooldown && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenerationRate * Time.deltaTime;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina; // Clamp stamina to max value
            }
        }
    }

    public void UseDash()
    {
        if (currentStamina >= dashCost)
        {
            currentStamina -= dashCost; // Reduce stamina on dash
            lastDashTime = Time.time;   // Update dash time
            // Trigger dash logic (move the player, etc.)
        }
    }

    void UpdateStaminaBar()
    {
        if (staminaBarImage != null)
        {
            staminaBarImage.fillAmount = currentStamina / maxStamina; // Update stamina UI
        }
    }
}
