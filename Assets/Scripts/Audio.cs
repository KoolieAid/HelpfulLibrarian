using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

public class Audio : MonoBehaviour
{
    public static Audio Instance;

    [SerializedDictionary("Audio Name", "Audio")]
    public SerializedDictionary<string, AudioClip> gameSfx;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySfx(string _clipName)
    {
        StartCoroutine(StartAudioClip(_clipName));
    }

    IEnumerator StartAudioClip(string _audioName)
    {
        var s = gameObject.AddComponent<AudioSource>();
        s.PlayOneShot(gameSfx[_audioName], 0.13f);
        
        yield return new WaitUntil(() => !s.isPlaying);
        
        Destroy(s);
    }
}
