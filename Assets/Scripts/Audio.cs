using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        GameObject[] test;
        test = GameObject.FindGameObjectsWithTag("Audio");

        if (test.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }
    
    void Update()
    {
        
    }
}
