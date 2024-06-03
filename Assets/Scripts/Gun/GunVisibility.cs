using Photon.Pun;
using UnityEngine;

public class GunVisibility : MonoBehaviourPun
{
    public MeshRenderer localGunRenderer; // MeshRenderer для локальной модели
    public MeshRenderer networkGunRenderer; // MeshRenderer для сетевой модели

    private void Start()
    {
        if (photonView.IsMine)
        {
            // Видимая только владельцу
            localGunRenderer.enabled = true;
            networkGunRenderer.enabled = false;
        }
        else
        {
            // Видимая всем кроме владельца
            localGunRenderer.enabled = false;
            networkGunRenderer.enabled = true;
        }
    }
}
