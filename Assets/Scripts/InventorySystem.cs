using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int types_of_ammunition = 4;
    public int types_of_keys = 3;
    public int amount_of_weapons = 5;

    public GameObject gunLoc;
    public string tagIdentifier; // kad script zino kokios ginklais yra player

    GameObject[] weapons;
    int current_amount_of_weapons;
    int[] ammunition;
    int[] maxAmmunition;
    bool[] keycards;
    int currentweapon;

    public TextMeshProUGUI debugText; // nesvarbu, jeigu norite galite nustatyti teksta debug tikslais
    void Start()
    {
        currentweapon = 0;
        ammunition = new int[types_of_ammunition];
        maxAmmunition = new int[types_of_ammunition];
        for(int i = 0; i < maxAmmunition.Length; i++)
        {
            maxAmmunition[i] = 100;
        }
        keycards = new bool[types_of_keys];
        weapons = new GameObject[amount_of_weapons];
        GetGunInventory(gunLoc, tagIdentifier);
        
        
    }
    void Update()
    {
        // weapon switcher
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            var i = currentweapon + 1;
            if (i >= current_amount_of_weapons) i = 0;

            SwitchGun(i);
        }
        else if (scroll < 0)
        {
            var i = currentweapon - 1;
            if (i < 0) i = current_amount_of_weapons - 1;

            SwitchGun(i);
        }

        // if TextMeshProUGUI has been set
        if (debugText != null)
        {
            DebugMode();
        }
    }
    void SwitchGun(int i)
    {
        weapons[currentweapon].SetActive(false);
        currentweapon = i;
        weapons[currentweapon].SetActive(true);
    }
    void GetGunInventory(GameObject parent, string tag)
    {
        Transform t = parent.transform;
        int t2 = 0;

        //print(parent);
        //print(tag);
        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.CompareTag(tag))
            {
                //print(weapons.Length);
                //print("Before:" + aaat);
                weapons[t2] = t.GetChild(i).gameObject;
                t2++;
                //print("After:" + aaat);
            }

        }
        current_amount_of_weapons = t2;
    }


    void DebugMode()
    {
        string temp = "Debug Mode for inventory \n";
        for (int i = 0; i < ammunition.Length; i++)
        {
            temp += "ammunition" + i + " " + ammunition[i] + "\n";
        }
        for (int i = 0; i < maxAmmunition.Length; i++)
        {
            temp += "maxAmmunition" + i + " " + maxAmmunition[i] + "\n";
        }
        for (int i = 0; i < keycards.Length; i++)
        {
            temp += "keycard" + i + " " + keycards[i] + "\n";
        }
        for (int i = 0; i < weapons.Length; i++)
        {
            temp += "Weapon " + i + " " + weapons[i] + "\n";
        }
        debugText.text = temp;
    }

    //
    // public functions for getting items (via collision, trigger, event, etc.)
    //

    // for guns - function needs to collide with an object
    public void GetNewGun(GameObject collidedGun)
    {
        var newgun = Instantiate(collidedGun, new Vector3(0, 0.4f, 0.7f), Quaternion.identity);
        weapons[current_amount_of_weapons] = newgun;
        newgun.SetActive(false);
        newgun.transform.parent = gunLoc.transform;
        current_amount_of_weapons++;
    }
    // for ammo - function needs to know which type of ammo you picked up
    public void GetAmmunition(int amount, int id)
    {
        int ammo = ammunition[id];
        int maxammo = maxAmmunition[id];

        ammo += amount;
        ammo = Mathf.Max(ammo, 0);
        ammo = Mathf.Min(ammo, maxammo);

        ammunition[id] = ammo;
    }
    // for keycards - function needs to know which type of keycard you picked up
    public void GetKeycard(int type)
    {
        keycards[type] = true;
    }
}
