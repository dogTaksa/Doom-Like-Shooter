using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformdisapear : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
            Invoke("Disappear", 1.5f);
    }
    void Disappear() => gameObject.SetActive(false);
}
