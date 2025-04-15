using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockmoveFB : MonoBehaviour
{
    public float speed = 20f; // greitis
    public float distance = 7f;

    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float move = speed * Time.deltaTime;

        if (movingRight)
            transform.Translate(move, 0, 0);
        else
            transform.Translate(-move, 0, 0);

        // Tikrina ar pasiekė maksimalų atstumą nuo pradinės vietos
        if (Vector3.Distance(transform.position, startPosition) >= distance)
            movingRight = !movingRight;
    }
}