using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Audio : MonoBehaviour
{
    public static Audio Instance;
    
    public AudioSource correctAnswerSfx;
    public AudioSource wrongAnswerSfx;
    public AudioSource winSfx;
    public AudioSource newVisitorSfx;

    public AudioSource miniGameCorrectAnswerSfx;
    public AudioSource miniGameWrongAnswerSfx;
    public AudioSource playerLoseSfx;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(this.gameObject);
    }
}
