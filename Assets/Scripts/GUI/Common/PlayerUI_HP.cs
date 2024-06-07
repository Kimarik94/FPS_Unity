using UnityEngine;
using TMPro;

public class PlayerUI_HP : MonoBehaviour
{
    private TextMeshProUGUI playerHP;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHP = GameObject.Find("HPStatus").GetComponent<TextMeshProUGUI>();
        playerHealth = transform.root.root.GetComponent<PlayerHealth>();
        playerHP.text = playerHealth.health.ToString();
    }

    private void FixedUpdate()
    {
        if (playerHealth != null)
        {
            playerHP.text = playerHealth.health.ToString();
        }
    }
}
