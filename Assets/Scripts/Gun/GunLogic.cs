using Photon.Pun;
using UnityEngine;
using UnityEngine.VFX;

public class GunLogic : MonoBehaviourPun
{
    //RPC Effects
    private GunEffects effects;

    //Effects Objects
    private Animation gunAnimation;
    private VisualEffect fireShotEffect;
    public AudioSource audioSource;
    public AudioClip gunSound;

    private float clickedTime = 0f;
    public float shootDelay = 0.15f;

    //For Raycast
    public LayerMask hitMasks;
    private Ray ray;
    private float shootDistance = 1000f;

    //Damage Logic
    public int gunDamage = 10;

    //Bullet Holes
    public GameObject bulletHolePrefab;
    public LayerMask hitLayers;

    private void Start()
    {
        effects = transform.root.gameObject.transform.root.GetComponentInChildren<GunEffects>();
        gunSound = Resources.Load<AudioClip>("Sounds/ak47-2_Sound");
        audioSource = GetComponent<AudioSource>();
        gunAnimation = GetComponentInChildren<Animation>();
        fireShotEffect = GetComponentInChildren<VisualEffect>();
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance != null)
        {
            ShootRay();
        }
    }

    private void ShootRay()
    {
        if (PlayerInputHandler.Instance.fireInput && Time.time - clickedTime > shootDelay)
        {
            clickedTime = Time.time;

            if (photonView.IsMine)
            {
                ShootEffects();
            }

            photonView.RPC("RPCShootEffectForOthers", RpcTarget.Others, transform.position);

            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            ray = new Ray(Camera.main.transform.position, ray.direction);

            if (Physics.Raycast(ray, out RaycastHit hit, shootDistance, hitMasks))
            {
                Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));

                photonView.RPC("RPCCreateBulletHole", RpcTarget.All, hit.point, hit.normal);

                Debug.Log("Hit object: " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.name.StartsWith("Player"))
                {
                    hit.collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(gunDamage);
                }
            }
        }
    }

    private void ShootEffects()
    {
        audioSource.PlayOneShot(gunSound);
        fireShotEffect.Play();
        gunAnimation.Play();
    }



    [PunRPC]
    private void RPCShootEffectForOthers(Vector3 shooterPosition)
    {
        // Только запуск эффектов для других игроков, кроме стреляющего
        effects.PlayEffects(shooterPosition);
    }

    [PunRPC]
    private void RPCCreateBulletHole(Vector3 hitPosition, Vector3 hitNormal)
    {
        Instantiate(bulletHolePrefab, hitPosition, Quaternion.LookRotation(hitNormal));
    }
}
