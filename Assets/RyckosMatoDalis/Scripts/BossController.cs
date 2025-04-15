using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip chargeSound;
    public AudioClip fireSound;
    public AudioSource audioSource;

    [Header("Player Target")]
    public Transform player;

    [Header("Basic Bullet Attack")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletCooldown = 2f;
    public float bulletSpeed = 25f;

    [Header("Spinning Laser Attack")]
    public GameObject laserPrefab;
    public Transform laserContainer;
    public float laserCooldown = 10f;
    public float laserChargeTime = 2f;
    public float laserDuration = 3f;

    [Header("Special Attacks")]
    public GameObject radialLaserPrefab;
    public GameObject homingFireballPrefab;
    public Transform[] teleportPoints;

    [Header("Special Timing")]
    public float specialCooldown = 10f;
    [Range(0f, 1f)] public float specialChance = 0.8f;

    [Header("Laser Spin")]
    public float spinSpeed = 90f;
    public float orbitRadius = 5f;

    private float bulletTimer;
    private float laserTimer;
    private float specialTimer;
    private bool isFiringLaser = false;
    private bool isUsingSpecial = false;

    private GameObject[] activeLasers;

    void Update()
    {
        if (player == null) return;

        // Look at player (on Y axis only)
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f;
        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);

        bulletTimer -= Time.deltaTime;
        laserTimer -= Time.deltaTime;
        specialTimer -= Time.deltaTime;

        if (bulletTimer <= 0f && !isFiringLaser)
        {
            FireBullet();
            bulletTimer = bulletCooldown;
        }

        if (laserTimer <= 0f && !isFiringLaser)
        {
            StartCoroutine(FireSpinningLasers());
            laserTimer = laserCooldown;
        }

        if (!isFiringLaser && !isUsingSpecial && specialTimer <= 0f)
        {
            specialTimer = specialCooldown;

            if (Random.value < specialChance)
            {
                int rand = Random.Range(0, 3);
                Debug.Log("SPECIAL TRIGGERED: " + rand);

                switch (rand)
                {
                    case 0: StartCoroutine(LaserSlam()); break;
                    case 1: StartCoroutine(FireHomingFireballs()); break;
                    case 2: StartCoroutine(TeleportBulletSpray()); break;
                }
            }
        }
    }

    void FireBullet()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (player.position - firePoint.position).normalized;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }

    IEnumerator FireSpinningLasers()
    {
        isFiringLaser = true;

        if (chargeSound && audioSource)
            audioSource.PlayOneShot(chargeSound);

        yield return new WaitForSeconds(laserChargeTime);

        if (fireSound && audioSource)
            audioSource.PlayOneShot(fireSound);

        activeLasers = new GameObject[4];
        float[] baseAngles = new float[4];

        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90f;
            baseAngles[i] = angle;

            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * orbitRadius;

            GameObject laser = Instantiate(laserPrefab, transform.position + offset, Quaternion.identity, laserContainer);
            laser.transform.LookAt(transform.position);
            activeLasers[i] = laser;
        }

        float timer = 0f;

        while (timer < laserDuration)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < activeLasers.Length; i++)
            {
                if (activeLasers[i] != null)
                {
                    float currentAngle = baseAngles[i] + timer * spinSpeed;
                    float rad = currentAngle * Mathf.Deg2Rad;

                    Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * orbitRadius;
                    activeLasers[i].transform.position = transform.position + offset;
                    activeLasers[i].transform.LookAt(transform.position);
                }
            }

            yield return null;
        }

        foreach (GameObject laser in activeLasers)
        {
            if (laser != null)
                Destroy(laser);
        }

        isFiringLaser = false;
    }

    IEnumerator LaserSlam()
    {
        isUsingSpecial = true;

        if (chargeSound && audioSource)
            audioSource.PlayOneShot(chargeSound);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * Vector3.forward;

            Instantiate(radialLaserPrefab, transform.position + direction * 1.5f, Quaternion.LookRotation(direction));
        }

        if (fireSound && audioSource)
            audioSource.PlayOneShot(fireSound);

        yield return new WaitForSeconds(1f);
        isUsingSpecial = false;
    }

    IEnumerator FireHomingFireballs()
    {
        isUsingSpecial = true;

        if (chargeSound && audioSource)
            audioSource.PlayOneShot(chargeSound);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4; i++)
        {
            Vector3 spawnOffset = Quaternion.Euler(0, i * 90f, 0) * Vector3.forward * 2f;
            Instantiate(homingFireballPrefab, transform.position + spawnOffset + Vector3.up * 1.5f, Quaternion.identity);
        }

        if (fireSound && audioSource)
            audioSource.PlayOneShot(fireSound);

        yield return new WaitForSeconds(1f);
        isUsingSpecial = false;
    }

    IEnumerator TeleportBulletSpray()
    {
        isUsingSpecial = true;

        if (teleportPoints.Length == 0)
        {
            isUsingSpecial = false;
            yield break;
        }

        if (chargeSound && audioSource)
            audioSource.PlayOneShot(chargeSound);

        yield return new WaitForSeconds(0.5f);

        Transform point = teleportPoints[Random.Range(0, teleportPoints.Length)];
        transform.position = point.position;

        yield return new WaitForSeconds(0.5f);

        if (fireSound && audioSource)
            audioSource.PlayOneShot(fireSound);

        for (int i = 0; i < 12; i++)
        {
            float angle = i * 30f;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 dir = rotation * Vector3.forward;

            GameObject bullet = Instantiate(bulletPrefab, transform.position + dir, Quaternion.LookRotation(dir));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = dir * bulletSpeed;
            }
        }

        yield return new WaitForSeconds(0.5f);
        isUsingSpecial = false;
    }
}
