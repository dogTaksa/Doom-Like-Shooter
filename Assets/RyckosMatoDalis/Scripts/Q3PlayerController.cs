using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FastPlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float acceleration = 20f;
    public float airAcceleration = 6f;
    public float friction = 6f;

    [Header("Jumping")]
    public float jumpForce = 8f;
    public float gravity = 20f;
    public bool autoBhop = true;

    [Header("Air Control")]
    public float airControlFactor = 0.3f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Stamina")]
    public int maxStaminaCharges = 3;
    public float staminaRegenDelay = 5f;

    public Slider staminaSlider;
    private int currentStaminaCharges;
    private float lastStaminaUseTime;
    private float lastRegenTime;

    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float lastDashTime = -999f;
    private Vector3 dashDirection;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 inputDirection;
    private bool jumpQueued;

    private float currentMoveSpeed;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentStaminaCharges = maxStaminaCharges;
        lastRegenTime = Time.time;

        UpdateStaminaBar();
    }

    private void Update()
    {
        HandleInput();
        QueueJump();

        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown && currentStaminaCharges > 0)
        {
            StartDash();
        }

        HandleStaminaRegen();

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            velocity = dashDirection * dashSpeed;

            if (dashTimeLeft <= 0f)
                isDashing = false;

            controller.Move(velocity * Time.deltaTime);
            return;
        }

        if (controller.isGrounded)
        {
            if (jumpQueued)
            {
                velocity.y = jumpForce;
                jumpQueued = false;
            }
            else if (velocity.y < 0)
            {
                velocity.y = -2f;
            }

            GroundMove();
        }
        else
        {
            AirMove();
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        UpdateStaminaBar();
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;

        dashDirection = inputDirection != Vector3.zero ? inputDirection : transform.forward;

        currentStaminaCharges--;
        lastStaminaUseTime = Time.time;

        DecreaseStaminaBar();
    }

    void DecreaseStaminaBar()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = (float)currentStaminaCharges / (float)maxStaminaCharges;
        }
    }

    void HandleStaminaRegen()
    {
        if (currentStaminaCharges < maxStaminaCharges && Time.time - lastStaminaUseTime >= staminaRegenDelay && Time.time - lastRegenTime >= staminaRegenDelay && !isDashing)
        {
            currentStaminaCharges++;
            lastRegenTime = Time.time;

            RegenerateStaminaBar();
        }
    }

    void RegenerateStaminaBar()
    {
        if (staminaSlider != null && currentStaminaCharges > 0 && !isDashing)
        {
            staminaSlider.value = (float)currentStaminaCharges / (float)maxStaminaCharges;
        }
    }

    void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputDirection = (transform.right * h + transform.forward * v).normalized;
    }

    void QueueJump()
    {
        if (autoBhop)
        {
            if (Input.GetButton("Jump"))
                jumpQueued = true;
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
                jumpQueued = true;
        }
    }

    void GroundMove()
    {
        ApplyFriction();

        float targetSpeed = moveSpeed;

        if (inputDirection != Vector3.zero)
        {
            velocity = Accelerate(velocity, inputDirection, targetSpeed, acceleration);
        }

        currentMoveSpeed = velocity.magnitude;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (jumpQueued)
        {
            velocity.y = jumpForce;
            jumpQueued = false;
        }
    }

    void AirMove()
    {
        float targetSpeed = moveSpeed;

        if (inputDirection != Vector3.zero)
        {
            velocity = Accelerate(velocity, inputDirection, targetSpeed, airAcceleration);
            AirControl();
        }

        currentMoveSpeed = velocity.magnitude;
    }

    void AirControl()
    {
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        float speed = horizontalVelocity.magnitude;

        if (speed == 0 || inputDirection.z == 0) return;

        float dot = Vector3.Dot(horizontalVelocity.normalized, inputDirection);
        if (dot > 0)
        {
            Vector3 newDir = horizontalVelocity + inputDirection * airControlFactor;
            newDir.Normalize();
            velocity.x = newDir.x * speed;
            velocity.z = newDir.z * speed;
        }
    }

    void ApplyFriction()
    {
        Vector3 horizontalVel = new Vector3(velocity.x, 0, velocity.z);
        float speed = horizontalVel.magnitude;
        float drop = speed * friction * Time.deltaTime;

        float newSpeed = Mathf.Max(speed - drop, 0);
        if (speed > 0)
        {
            velocity.x *= newSpeed / speed;
            velocity.z *= newSpeed / speed;
        }
    }

    Vector3 Accelerate(Vector3 currentVelocity, Vector3 wishDir, float maxSpeed, float accel)
    {
        float projSpeed = Vector3.Dot(currentVelocity, wishDir);
        float addSpeed = maxSpeed - projSpeed;

        if (addSpeed <= 0)
            return currentVelocity;

        float accelSpeed = accel * Time.deltaTime * maxSpeed;
        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;

        currentVelocity += wishDir * accelSpeed;
        return currentVelocity;
    }

    void UpdateStaminaBar()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = (float)currentStaminaCharges / (float)maxStaminaCharges;
        }
    }

    public int GetStaminaCharges()
    {
        return currentStaminaCharges;
    }

    public float GetCurrentMoveSpeed()
    {
        return currentMoveSpeed;
    }
}
