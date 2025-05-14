using Unity.Collections;
using UnityEngine;

public class SceneMusicRegister : MonoBehaviour
{
    public string soundName = "SceneMusic";
    public SoundManager.SoundCategory category = SoundManager.SoundCategory.Music;
    public bool playOnStart = true;

    void Start()
    {
        if (SoundManager.Instance == null) return;

        AudioSource source = GetComponent<AudioSource>();
        if (source == null || source.clip == null)
        {
            return;
        }

        if (!SoundManager.Instance.soundList.Exists(s => s.name == soundName))
        {
            SoundManager.Sound newSound = new SoundManager.Sound
            {
                name = soundName,
                clip = source.clip,
                category = category,
                volume = 1f
            };

            SoundManager.Instance.soundList.Add(newSound);
        }

        if (!SoundManager.Instance.GetIndividualVolume(soundName).Equals(1f)) 
        {
            SoundManager.Instance.SetIndividualVolume(soundName, 1f);
        }

        if (playOnStart)
        {
            SoundManager.Instance.PlaySound(soundName, true);
        }
    }
}
