using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviourPun
{
    public static RespawnManager instance;
    public Transform[] spawnPoints;
    public GameObject playerPrefab;
    public GameObject playerDieObject;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AssignSpawnPoints;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AssignSpawnPoints;
    }

    private void AssignSpawnPoints(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name != "Main_Menu")
        {
            spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        }
    }

    public void SpawnDieObject(Transform pos)
    {
        PhotonNetwork.Instantiate(playerDieObject.name, pos.position, Quaternion.identity);
    }
    
    public void RespawnPlayer()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
        
        PlayerHealth playerHealth = newPlayer.GetComponent<PlayerHealth>();
        playerHealth.health = 100;
        Debug.Log("Player respawned!");
    }
}
