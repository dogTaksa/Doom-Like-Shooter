using UnityEngine;
using UnityEngine.UI;

public class EnhancedMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    public RectTransform uiElement;

    private float xRotation = 0f;

    [Header("UI Sway")]
    public float swayAmount = 5f;
    public float swaySmooth = 6f;

    [Header("UI Bob")]
    public float bobSpeed = 6f;
    public float bobAmount = 5f;

    private float bobTimer;
    private Vector2 initialUILocalPos;

    private float walkSpeed = 2f;
    private float runSpeed = 5f;
    public float gravityInfluence = 0.5f;

    public float swayOnMovement = 0.4f;
    public float walkingSwaySpeed = 0.3f;

    private FastPlayerMovement movementScript;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (uiElement != null)
            initialUILocalPos = uiElement.anchoredPosition;

        movementScript = playerBody.GetComponent<FastPlayerMovement>();

        walkSpeed = 2f;
        runSpeed = 10f;
    }

    void Update()
    {
        LookAround();

        if (uiElement != null)
        {
            ApplyUISway();
            ApplyUIHeadBob();
            ApplyUIPlayerMovement();
        }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void ApplyUISway()
    {
        float moveX = -Input.GetAxis("Mouse X") * swayAmount;
        float moveY = -Input.GetAxis("Mouse Y") * swayAmount;

        float moveSpeed = movementScript != null ? movementScript.GetCurrentMoveSpeed() : 0f;
        float movementSway = Mathf.Clamp(moveSpeed, 0f, runSpeed) * swayOnMovement;

        Vector2 targetPos = initialUILocalPos + new Vector2(moveX, moveY + movementSway);
        uiElement.anchoredPosition = Vector2.Lerp(uiElement.anchoredPosition, targetPos, Time.deltaTime * swaySmooth);
    }

    void ApplyUIHeadBob()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        if (isMoving)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmount;

            Vector2 targetPos = uiElement.anchoredPosition;
            targetPos.y = initialUILocalPos.y + bobOffset;

            float playerMoveSpeed = movementScript != null ? movementScript.GetCurrentMoveSpeed() : 0f;
            float speedFactor = Mathf.Lerp(walkSpeed, runSpeed, playerMoveSpeed / runSpeed);

            uiElement.anchoredPosition = new Vector2(
                uiElement.anchoredPosition.x,
                Mathf.Lerp(uiElement.anchoredPosition.y, targetPos.y, Time.deltaTime * swaySmooth * speedFactor)
            );
        }
        else
        {
            bobTimer = 0;
            uiElement.anchoredPosition = Vector2.Lerp(uiElement.anchoredPosition, initialUILocalPos, Time.deltaTime * swaySmooth);
        }
    }

    void ApplyUIPlayerMovement()
    {
        if (movementScript != null)
        {
            float verticalVelocity = movementScript.GetComponent<CharacterController>().velocity.y;
            float fallAmount = Mathf.Clamp(verticalVelocity, -10f, 10f);

            Vector2 targetPos = initialUILocalPos;
            targetPos.y += fallAmount * gravityInfluence;

            uiElement.anchoredPosition = Vector2.Lerp(uiElement.anchoredPosition, targetPos, Time.deltaTime * swaySmooth);
        }
    }
}
