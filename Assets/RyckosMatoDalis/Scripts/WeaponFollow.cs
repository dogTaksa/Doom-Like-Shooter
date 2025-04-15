using UnityEngine;

public class WeaponFollow : MonoBehaviour
{
    public Transform cameraTransform;
    public float smoothSpeed = 10f;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    void LateUpdate()
    {
        // Smoothly follow the camera’s position
        transform.position = cameraTransform.position + cameraTransform.TransformDirection(positionOffset);

        // Smoothly follow the camera’s rotation
        Quaternion targetRotation = cameraTransform.rotation * Quaternion.Euler(rotationOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}
