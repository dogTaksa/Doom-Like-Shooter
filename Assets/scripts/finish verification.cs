using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishVerification : MonoBehaviour
{
    public static bool finishedLevel = false;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            finishedLevel = true;
        }
    }
}

