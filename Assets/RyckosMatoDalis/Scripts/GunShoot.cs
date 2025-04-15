using UnityEngine;
using System.Collections;

public class GunShoot : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;
    public Transform muzzleFlashLocation;
    public GameObject hitEffectPrefab;
    public GameObject bulletEffectPrefab;
    public Animator gunAnimator;
    public Transform recoilTransform;

    [Header("Gun Settings")]
    public Vector3 recoilKickback = new Vector3(0f, 0.05f, -0.1f);
    public float recoilReturnSpeed = 5f;
    public float damage = 10f;
    public float raycastDistance = 100f;
    public float shootCooldown = 0.2f;

    [Header("Camera Shake")]
    public float cameraShakeIntensity = 0.1f;
    public float cameraShakeDuration = 0.1f;

    private Vector3 originalRecoilPos;
    private Vector3 currentRecoilOffset;
    private bool canShoot = true;
    private bool isWalking = false;

    void Start()
    {
        if (recoilTransform != null)
            originalRecoilPos = recoilTransform.localPosition;
    }

    void Update()
    {
        HandleMovement();

        if (Input.GetButtonDown("Fire1") && canShoot)
            Shoot();

        UpdateRecoil();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        isWalking = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        if (gunAnimator != null && canShoot)
        {
            gunAnimator.SetBool("isWalking", isWalking);
        }
    }

    void UpdateRecoil()
    {
        if (recoilTransform != null)
        {
            currentRecoilOffset = Vector3.Lerp(currentRecoilOffset, Vector3.zero, recoilReturnSpeed * Time.deltaTime);
            recoilTransform.localPosition = originalRecoilPos + currentRecoilOffset;
        }
    }

    void Shoot()
    {
        canShoot = false;

        if (muzzleFlash != null && muzzleFlashLocation != null)
        {
            muzzleFlash.transform.position = muzzleFlashLocation.position;
            muzzleFlash.transform.rotation = muzzleFlashLocation.rotation;
            muzzleFlash.Play();
        }

        currentRecoilOffset += recoilKickback;

        if (gunAnimator != null)
        {
            gunAnimator.SetBool("isWalking", false);
            gunAnimator.CrossFade("Shoot", 0.05f);
        }

        StartCoroutine(CameraShake());

        if (bulletEffectPrefab != null && muzzleFlashLocation != null)
        {
            Instantiate(bulletEffectPrefab, muzzleFlashLocation.position, muzzleFlashLocation.rotation);
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            if (hitEffectPrefab != null)
                Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            if (hit.collider.TryGetComponent(out EnemyHealth enemy))
                enemy.TakeDamage(damage);
        }

        StartCoroutine(ResetShootCooldown());
    }

    IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;

        if (gunAnimator != null)
            gunAnimator.SetBool("isWalking", isWalking);
    }

    IEnumerator CameraShake()
    {
        Vector3 originalPos = playerCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < cameraShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * cameraShakeIntensity;
            float y = Random.Range(-1f, 1f) * cameraShakeIntensity;
            playerCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.localPosition = originalPos;
    }
}
