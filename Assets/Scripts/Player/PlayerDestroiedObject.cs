using UnityEngine;
using Photon.Pun;

public class PlayerDestroiedObject : MonoBehaviourPun
{
    private float destroyDelay = 2.5f;
    private float onActive;

    private void OnEnable()
    {
        onActive = Time.time;
    }

    private void Update()
    {
        if(Time.time - onActive >= destroyDelay)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
