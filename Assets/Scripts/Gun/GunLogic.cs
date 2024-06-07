using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class GunLogic : MonoBehaviourPun
{
    private PlayerInputHandler inputHandler;

    // RPC Effects
    private GunEffects effects;

    // Effects Objects
    private VisualEffect fireShotEffect;

    // Audio
    [Header("Audio")]
    public AudioSource audioSource;
    private AudioClip gunSound;
    private AudioClip drySound;
    private AudioClip reloadSound;

    // Rate control
    [Space]
    [Header("Fire Rate")]
    private float clickedTime = 0f;
    public float shootDelay = 0.15f;

    // Recoil Sys
    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    [Space]
    [Header("Recoil System")]
    public float recoilBack = 0.1f;
    public float recoilUp = 0.1f;
    public float recoilLength = 0.01f;
    public float recoverLength = 0.02f;

    private bool recoiling;
    private bool recovering;

    // Gun Sway
    [Space]
    [Header("Gun Sway")]
    public float swayClamp = 0.06f;
    public float smoothing = 3f;

    // For Raycast
    [Space]
    [Header("Damage and Hit")]
    public LayerMask hitMasks;
    private Ray ray;
    private float shootDistance = 1000f;
    public int gunDamage = 10;

    // Bullet Holes
    public GameObject bulletHolePrefab;

    // Ammo Control UI
    [Space]
    [Header("Ammo Logic")]
    private AmmoStatus ammoStatus;
    public bool ammoClipDry = false;
    public bool ammoTotalDry = false;
    public bool reloading = false;

    // Reloading
    private Animation gunAnimation;

    // Weapon Spread
    [Space]
    [Header("Spread Logic")]
    public float spreadHorizontal = 0.025f;
    public float spreadVertical = 0.0125f;

    private void Start()
    {
        inputHandler = transform.root.root.GetComponent<PlayerInputHandler>();

        originalPosition = transform.localPosition;
        effects = transform.root.gameObject.transform.root.GetComponentInChildren<GunEffects>();
        gunAnimation = GetComponent<Animation>();
        gunSound = Resources.Load<AudioClip>("Sounds/ak47-2_Sound");
        drySound = Resources.Load<AudioClip>("Sounds/ak47-2_DryAmmo");
        reloadSound = Resources.Load<AudioClip>("Sounds/ak47-2_Reload");
        audioSource = GetComponent<AudioSource>();
        fireShotEffect = GetComponentInChildren<VisualEffect>();
        ammoStatus = transform.root.root.GetComponentInChildren<AmmoStatus>();
    }

    private void FixedUpdate()
    {
        if (ammoStatus == null) ammoStatus = transform.root.root.GetComponentInChildren<AmmoStatus>();
    }

    private void Update()
    {
        if (inputHandler != null && photonView.IsMine)
        {
            ShootRay();
            if (recoiling) Recoiling();
            if (recovering) Recovering();
            if (!recoiling) GunSway();
            if (ammoClipDry && !reloading) AmmoDrySound();
            if (inputHandler.reloadInput && ammoStatus.ammoCurrentAmount < 30
                && !reloading && !ammoTotalDry) Reload();
        }
    }

    private void GunSway()
    {
        Vector2 input = inputHandler.lookInput;

        input.x = Mathf.Clamp(input.x, -swayClamp, swayClamp);
        input.y = Mathf.Clamp(input.x, -swayClamp, swayClamp);

        Vector3 target = new Vector3(-input.x, -input.y, 0f);
        transform.localPosition =
            Vector3.Lerp(transform.localPosition, target + originalPosition, Time.deltaTime * smoothing);
    }

    private void ShootRay()
    {
        if (inputHandler.fireInput && Time.time - clickedTime > shootDelay
            && !ammoClipDry && !reloading)
        {
            clickedTime = Time.time;
            recoiling = true;
            recovering = false;

            if (photonView.IsMine)
            {
                ShootEffects();
                ammoStatus.DecreaseAmmoClipAmount();
            }

            photonView.RPC("RPCShootEffectForOthers", RpcTarget.Others, transform.position);

            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            ray = new Ray(Camera.main.transform.position, GunSpread(ray.direction));

            Debug.DrawRay(ray.origin, ray.direction, Color.blue, 100f);

            if (Physics.Raycast(ray, out RaycastHit hit, shootDistance, hitMasks))
            {
                Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));

                photonView.RPC("RPCCreateBulletHole", RpcTarget.All, hit.point, hit.normal);
                photonView.RPC("RPCShootEffectForOthers", RpcTarget.Others, transform.position);

                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    photonView.RPC("RPCCreateBulletHole", RpcTarget.All, hit.point, hit.normal);

                    PhotonView targetPhotonView = hit.collider.gameObject.GetComponent<PhotonView>();

                    if (targetPhotonView != null)
                    {
                        targetPhotonView.RPC("TakeDamage", RpcTarget.All, gunDamage);
                    }
                }
            }
        }
    }

    private Vector3 GunSpread(Vector3 direction)
    {
        float spreadX = Random.Range(-spreadHorizontal, spreadHorizontal);
        float spreadY = Random.Range(-spreadVertical, spreadVertical);
        return direction + new Vector3(spreadX, spreadY, 0);
    }

    private void Reload()
    {
        if (ammoStatus.ammoCurrentAmount == 0) ammoClipDry = true;
        audioSource.Stop();
        audioSource.PlayOneShot(reloadSound);
        gunAnimation.Play();

        reloading = true;

        StartCoroutine(WaitUntilFinishReloading(reloadSound.length));
    }

    private IEnumerator WaitUntilFinishReloading(float duration)
    {
        yield return new WaitForSeconds(duration);
        ammoStatus.Reload();
        reloading = false;
        ammoClipDry = false;
    }

    private void AmmoDrySound()
    {
        if (inputHandler.fireInput && Time.time - clickedTime > shootDelay)
        {
            clickedTime = Time.time;
            audioSource.PlayOneShot(drySound);
        }
    }

    private void ShootEffects()
    {
        audioSource.PlayOneShot(gunSound);
        fireShotEffect.Play();
    }

    private void Recoiling()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUp, originalPosition.z - recoilBack);

        transform.localPosition =
            Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);

        if (Mathf.Approximately(transform.localPosition.x, finalPosition.x) &&
            Mathf.Approximately(transform.localPosition.y, finalPosition.y) &&
            Mathf.Approximately(transform.localPosition.z, finalPosition.z))
        {
            recoiling = false;
            recovering = true;
        }
    }

    private void Recovering()
    {
        Vector3 finalPosition = originalPosition;

        transform.localPosition =
            Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

        if (Mathf.Approximately(transform.localPosition.x, finalPosition.x) &&
            Mathf.Approximately(transform.localPosition.y, finalPosition.y) &&
            Mathf.Approximately(transform.localPosition.z, finalPosition.z))
        {
            recoiling = false;
            recovering = false;
        }
    }

    [PunRPC]
    private void RPCShootEffectForOthers(Vector3 shooterPosition)
    {
        effects.PlayEffects(shooterPosition);
    }

    [PunRPC]
    private void RPCCreateBulletHole(Vector3 hitPosition, Vector3 hitNormal)
    {
        Instantiate(bulletHolePrefab, hitPosition, Quaternion.LookRotation(hitNormal));
    }
}
