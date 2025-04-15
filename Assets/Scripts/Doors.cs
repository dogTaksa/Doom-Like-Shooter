using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script ant duru
public class Doors : MonoBehaviour
{
    //requires collider
    public GameObject door; //durys
    public int speed;
    public int height; 
    
    
    private bool touch = false;
    void Start()
    {
        touch = false;
        
        if(height<0)
        {
            height = height*-1;
        }
        else if(height>0)
        {
            return;
        }

    }

    
    void Update()
    {   
        
        if(touch)
        {   
            door.transform.position = new Vector3(door.transform.position.x,door.transform.position.y -(speed*Time.deltaTime), door.transform.position.z);
            if(door.transform.position.y<-height)
            {
                print(-height);
                touch = false;
                door.gameObject.SetActive(false);
            }
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
