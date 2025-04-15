using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    
    void Start()
    {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.flipX = true;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
}
