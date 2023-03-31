using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio Instance;
    
    public AudioSource correctAnswerSfx;
    public AudioSource wrongAnswerSfx;
    public AudioSource winSfx;

    void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad(this.gameObject);

        GameObject[] test;
        test = GameObject.FindGameObjectsWithTag("Audio");

        if (test.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }
}
