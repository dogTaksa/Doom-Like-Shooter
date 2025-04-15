using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
//eina ant NPC 
public class DialogBox : MonoBehaviour
{
    public GameObject canvas; //canvas
    public TextMeshProUGUI text;//canvas text
    public GameObject playerCam; //the camera
    public string[] dialog; //cia reikes iseiles kaip dialog tures eiti
    
    bool touch = false;
    void Start()
    {
        canvas.SetActive(false);
    }

    void Update()
    {
        if(touch)
        {
            canvas.SetActive(true);
            
            
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                for(int i = 0; i<dialog.Length; i++)
                {
                    text.text = dialog[i];
                    
                    
                }
                
            }
        }
    }

 

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Cia player tag"))
        {
            var test = playerCam.GetComponent<NewBehaviourScript>();//camera movement script kaip POV
            test.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            touch = true;


        }
    }
    
}
