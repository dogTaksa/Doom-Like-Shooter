using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformLR : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Juda pagal objekto pasukimą (vietinę X ašį)
        transform.position = startPos - Vector3.forward * Mathf.Sin(Time.time * speed) * distance;
    }
}
