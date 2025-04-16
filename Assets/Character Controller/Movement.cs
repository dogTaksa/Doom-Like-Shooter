using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float sprintTime;
    [SerializeField] private bool canSprint;
    [SerializeField] private float waitTime;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private bool isCrouching;
    [SerializeField] private Transform mainCamera;
    private float upDownRotation;
    [SerializeField] private bool inSprint; // check is person sprinting now


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        canSprint = true;
        isCrouching = false;
        inSprint = false;
    }

    private void Update()
    {

        Look();
        Move();

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            if (canSprint)
            {
                inSprint = true;
                playerSpeed = 5f;
                sprintTime += Time.deltaTime;
                if (sprintTime >= 4f)
                {
                    StartCoroutine(SprintCooldown());
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerSpeed = 3f;
            inSprint = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !inSprint)
        {
            if (!isCrouching)
            {
                mainCamera.position = new Vector3(mainCamera.position.x, 0.3f, mainCamera.position.z);
                isCrouching = true;
                playerSpeed = 2f;
            }
           else if (isCrouching)
            {
                mainCamera.position = new Vector3(mainCamera.position.x, 1, mainCamera.position.z);
                isCrouching = false;
                playerSpeed = 3f;
            }
        }
    }

    IEnumerator SprintCooldown()
    {
        yield return new WaitForSeconds(waitTime);
        playerSpeed = 3f;
        sprintTime = 0f;
        canSprint = false;
        inSprint = false;
    }

    private void Look()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        upDownRotation -= mouseY;
        upDownRotation = Mathf.Clamp(upDownRotation, -90f, 90f);

        mainCamera.localRotation = Quaternion.Euler(upDownRotation, 0, 0);
        transform.Rotate(0, mouseX, 0);
    }
    private void Move()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var z = Input.GetAxisRaw("Vertical");

        var move = (transform.right * x + transform.forward * z) * playerSpeed * Time.deltaTime;
        characterController.Move(move);
    }
}

