using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializedDictionary("Particle", "Particle GameObject")]
    public SerializedDictionary<string, ParticleSystem> particles = new SerializedDictionary<string, ParticleSystem>();

    private void Awake()
    {
        Instance = this;
    }
}
