using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;


    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Debug.Log(gameObject.name + " Die");
        }
    }
}
