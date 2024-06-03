using Photon.Pun;
using UnityEngine;
using UnityEngine.VFX;

public class GunEffects : MonoBehaviourPun
{
    public AudioSource audioSource;
    public AudioClip gunSound;
    public VisualEffect fireShotEffect;
    private Animation gunShake;
    public AnimationClip gunShakeClip;

    private void Start()
    {
        gunSound = Resources.Load<AudioClip>("Sounds/ak47-2_Sound");
        audioSource = GetComponent<AudioSource>();
        fireShotEffect = GetComponentInChildren<VisualEffect>();
        gunShake = GetComponentInParent<Animation>();
        gunShake.AddClip(gunShakeClip, "RPCGunShake");
        gunShake.clip = gunShakeClip;

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found!");
        }
        if (fireShotEffect == null)
        {
            Debug.LogError("VisualEffect component not found!");
        }
        if (gunShake == null)
        {
            Debug.LogError("Animation component not found!");
        }
    }

    [PunRPC]
    public void PlayEffects(Vector3 shooterPos)
    {
        if (!photonView.IsMine)
        {
            if (audioSource != null && gunSound != null)
            {
                audioSource.PlayOneShot(gunSound);
            }
            if (fireShotEffect != null)
            {
                fireShotEffect.Play();
            }
            if (gunShake != null)
            {
                gunShake.Play();
            }
        }
    }
}
