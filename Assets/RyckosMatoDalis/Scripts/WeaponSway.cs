using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float swayAmount = 0.1f;
    public float swaySpeed = 3f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        ApplyWeaponSway();
    }

    void ApplyWeaponSway()
    {
        float swayX = -Input.GetAxis("Mouse X") * swayAmount;
        float swayY = -Input.GetAxis("Mouse Y") * swayAmount;

        Vector3 sway = new Vector3(swayX, swayY, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + sway, Time.deltaTime * swaySpeed);
    }
}