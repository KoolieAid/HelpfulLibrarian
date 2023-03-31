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
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(this.gameObject);
    }
}
