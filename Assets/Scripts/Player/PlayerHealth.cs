using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPun
{
    private bool isDead = false;
    public int health = 100;

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (photonView.IsMine && !isDead)
        {
            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} Die");

        photonView.RPC("RPCRemovePlayer", RpcTarget.AllBuffered);

        RespawnManager.instance.SpawnDieObject(transform);
        RespawnManager.instance.RespawnPlayer();
    }

    [PunRPC]
    private void RPCRemovePlayer()
    {
        Debug.Log($"{gameObject.name} is being removed");
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
