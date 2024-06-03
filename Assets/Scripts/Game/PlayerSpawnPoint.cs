using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public static PlayerSpawnPoint Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private List<Transform> spawnPoints = new List<Transform>();

    private void Start()
    {
        foreach(var point in GetComponentsInChildren<Transform>())
        {
            spawnPoints.Add(point);
        }
    }

    public Transform GetSpawnPoint(int index)
    {
        if (index == 0)
        {
            System.Random rInt = new System.Random();
            return spawnPoints[rInt.Next(0, spawnPoints.Count)];
        }
        else if (index <= spawnPoints.Count && index > 0)
        {
            return spawnPoints[index];
        }
        else return spawnPoints[0];
    }

    
}
