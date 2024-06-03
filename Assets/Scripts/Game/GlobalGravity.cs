using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGravity : MonoBehaviour
{
    public static GlobalGravity Instance;

    public float gravity = -9.81f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
