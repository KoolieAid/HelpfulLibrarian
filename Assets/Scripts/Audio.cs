using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

public class Audio : MonoBehaviour
{
    public static Audio Instance;

    [SerializeField] private float volume = 0.15f;
    
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
        if (gameSfx.Count <= 0 || !gameSfx.ContainsKey(_audioName))
        {
            Debug.LogWarning("No Audio Clips Detected");
            yield break;
        }
        
        var s = gameObject.AddComponent<AudioSource>();

        s.PlayOneShot(gameSfx[_audioName], volume);
        
        yield return new WaitUntil(() => !s.isPlaying);
        
        Destroy(s);
    }
}
