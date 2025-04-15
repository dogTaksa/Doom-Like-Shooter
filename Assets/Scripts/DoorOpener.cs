using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public Transform door;           
    public Vector3 openRotation;     
    public float openSpeed = 2f;     

    private bool isPlayerNear = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;

    void Start()
    {
        closedRotation = door.rotation;
        targetRotation = closedRotation;
    }

    void Update()
    {
        if (isPlayerNear)
            targetRotation = Quaternion.Euler(closedRotation.eulerAngles + openRotation);
        else
            targetRotation = closedRotation;

        door.rotation = Quaternion.Lerp(door.rotation, targetRotation, Time.deltaTime * openSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = false;
    }
}
