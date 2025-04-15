using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script ant item
public class NotesPickup : MonoBehaviour
{
    //requires collider
    public GameObject item; //ta item kuri reikia deti i inventory
    private bool touch;
    void Start()
    {
        touch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(touch)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {

                item.SetActive(false);
            }
            //reikia kad i inventory eitu
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("CiaPlayerTag"))
        {
            touch = true;
        }
    }
}
