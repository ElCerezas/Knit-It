using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem particlesOnBeat;
    private void Start()
    {
        float bpm = 60 / SongManager.Instance.BPM;
        SongManager.OnBeat += ParticleEmit;
    }

    private void OnDisable()
    {
        SongManager.OnBeat -= ParticleEmit;
    }

    private void ParticleEmit()
    {
        particlesOnBeat.Play();
    }
    
}
