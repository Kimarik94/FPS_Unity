using Photon.Pun;
using UnityEngine;

public class GunVisibility : MonoBehaviourPun
{
    public MeshRenderer localGunRenderer; // MeshRenderer ��� ��������� ������
    public MeshRenderer networkGunRenderer; // MeshRenderer ��� ������� ������

    private void Start()
    {
        if (photonView.IsMine)
        {
            // ������� ������ ���������
            localGunRenderer.enabled = true;
            networkGunRenderer.enabled = false;
        }
        else
        {
            // ������� ���� ����� ���������
            localGunRenderer.enabled = false;
            networkGunRenderer.enabled = true;
        }
    }
}
