using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private List<GameObject> bulletSpots = new List<GameObject>();

    private float lastCheckBulletsTime = 0f;
    private float checkBulletsInterval = 60f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        if(Time.time - lastCheckBulletsTime > checkBulletsInterval)
        {
            CheckFillnRefreshBulletList();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Level_Base")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (scene.name == "Main_Menu")
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    private void CheckFillnRefreshBulletList()
    {
        bulletSpots.Clear();
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("BulletSpot");

        for(int i = 0; i < tempArray.Length; i++)
        {
            bulletSpots.Add(tempArray[i]);
        }

        if(bulletSpots.Count > bulletSpots.Count / 3)
        {
            for(int i = 0; i <= bulletSpots.Count/3; i++)
            {
                Destroy(bulletSpots[i]);
            }
        }
        lastCheckBulletsTime = Time.time;
    }
}
