using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShootingController : MonoBehaviour
{
    [Header("Weapon References")]
    [Tooltip("List of weapons to be used in the game, weapondata scriptable object")]
    public List<WeaponData> weapons = new List<WeaponData>();
    [Tooltip("List of weapon models to be displayed in the game, child of player camera")]
    public List<GameObject> weaponModels = new List<GameObject>();
    public int currentWeaponIndex = 0;
    
    [Header("Ammo System")]
    public List<int> ammoCounters = new List<int>();
    
    [Header("UI References")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI weaponNameText;
    private float nextFireTime = 0f;
    private AudioSource audioSource;
    private Vector3 initialWeaponPosition;
    private float bobTimer = 0f;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        if (ammoCounters.Count == 0)
        {
            foreach (WeaponData weapon in weapons)
            {
                ammoCounters.Add(weapon.maxAmmo);
            }
        }
        
        foreach (GameObject model in weaponModels)
        {
            if (model != null)
                model.SetActive(false);
        }
        
        SwitchWeapon(currentWeaponIndex);
        
        if (weaponModels.Count > 0 && weaponModels[currentWeaponIndex] != null)
        {
            initialWeaponPosition = weaponModels[currentWeaponIndex].transform.localPosition;
        }
        
        UpdateUI();
    }
    
    void Update()
    {
        if (weapons.Count == 0 || weaponModels.Count == 0) 
            return;
            
        if (currentWeaponIndex >= weapons.Count || currentWeaponIndex >= weaponModels.Count)
            return;
            
        WeaponData currentWeapon = weapons[currentWeaponIndex];
        
        if (Input.GetMouseButton(0) && currentWeapon.isAutomatic)
        {
            FireWeapon();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            FireWeapon();
        }
        
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) && i <= weapons.Count)
                SwitchWeapon(i - 1);
        }
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
            SwitchWeapon((currentWeaponIndex + 1) % weapons.Count);
        else if (scroll < 0)
            SwitchWeapon((currentWeaponIndex - 1 + weapons.Count) % weapons.Count);
    }
    
    void SwitchWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count || index >= weaponModels.Count)
            return;
            
        if (currentWeaponIndex < weaponModels.Count && weaponModels[currentWeaponIndex] != null)
            weaponModels[currentWeaponIndex].SetActive(false);
            
        currentWeaponIndex = index;
        if (weaponModels[currentWeaponIndex] != null)
        {
            weaponModels[currentWeaponIndex].SetActive(true);
            initialWeaponPosition = weaponModels[currentWeaponIndex].transform.localPosition;
        }
        
        UpdateUI();
    }
    
    bool FireWeapon()
    {
        WeaponData weapon = weapons[currentWeaponIndex];
        
        if (Time.time < nextFireTime)
            return false;
            
        if (ammoCounters[currentWeaponIndex] <= 0 && currentWeaponIndex != 0)
        {
            if (weapon.emptySound)
                audioSource.PlayOneShot(weapon.emptySound);
            return false;
        }
        
        if (currentWeaponIndex != 0)
        {
            ammoCounters[currentWeaponIndex]--;
        }
        
        nextFireTime = Time.time + weapon.fireRate;
        
        if (weapon.fireSound)
            audioSource.PlayOneShot(weapon.fireSound);
            
        if (weapon.muzzleFlash)
        {
            GameObject flash = Instantiate(weapon.muzzleFlash, 
                                          Camera.main.transform.position + Camera.main.transform.forward * 0.5f, 
                                          Camera.main.transform.rotation);
            Destroy(flash, 0.1f);
        }
        
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, weapon.range))
        {
            if (weapon.impactEffect)
            {
                GameObject impact = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }
        }
        
        UpdateUI();
        return true;
    }
    
    public void AddAmmo(int amount)
    {
        if (currentWeaponIndex < ammoCounters.Count && currentWeaponIndex < weapons.Count)
        {
            WeaponData weapon = weapons[currentWeaponIndex];
            ammoCounters[currentWeaponIndex] = Mathf.Min(
                ammoCounters[currentWeaponIndex] + amount, 
                weapon.maxAmmo
            );
            UpdateUI();
        }
    }
    
    private void UpdateUI()
    {
        if (ammoText != null && currentWeaponIndex < ammoCounters.Count)
        {
            ammoText.text = currentWeaponIndex == 0 ? "∞" : ammoCounters[currentWeaponIndex].ToString();
        }
        
        if (weaponNameText != null && currentWeaponIndex < weapons.Count)
        {
            weaponNameText.text = weapons[currentWeaponIndex].weaponName;
        }
    }
}