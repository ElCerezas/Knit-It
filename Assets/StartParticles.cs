using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem particlesOnBeat;
    private void Start()
    {
        float bpm = 60 / SongManager.Instance.BPM;
        particlesOnBeat.startLifetime = bpm;
        SongManager.OnHalfBeat += ParticleEmit;
    }

    private void OnDisable()
    {
        SongManager.OnHalfBeat -= ParticleEmit;
    }

    private void ParticleEmit()
    {
        particlesOnBeat.Play();
    }
    
}
