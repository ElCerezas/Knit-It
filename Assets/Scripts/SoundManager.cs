using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public SoundCategory category;
        [Range(0f, 1f)] public float volume = 1.0f;
    }
    public enum SoundCategory { Music, SFX }
    public List<Sound> soundList;
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    private Dictionary<SoundCategory, float> categoryVolumes = new Dictionary<SoundCategory, float>
    {
        { SoundCategory.Music, 1.0f },
        { SoundCategory.SFX, 1.0f }
    };

    private Dictionary<string, float> individualVolumes = new Dictionary<string, float>();

    private Dictionary<string, AudioSource> activeSounds = new Dictionary<string, AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        categoryVolumes[SoundCategory.Music] = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        categoryVolumes[SoundCategory.SFX] = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        foreach (Sound sound in soundList)
        {
            soundDictionary[sound.name] = sound;
            individualVolumes[sound.name] = PlayerPrefs.GetFloat("Volume_" + sound.name, 1.0f);
        }
        if (musicSlider != null)
        {
            musicSlider.value = categoryVolumes[SoundCategory.Music];
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = categoryVolumes[SoundCategory.SFX];
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void PlaySound(string soundName, bool loop = false)
    {
        if (!soundDictionary.ContainsKey(soundName))
            return;

        Sound sound = soundDictionary[soundName];

        AudioSource source;
        if (!activeSounds.ContainsKey(soundName))
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = categoryVolumes[sound.category] * sound.volume;
            activeSounds[soundName] = source;
        }
        else
        {
            source = activeSounds[soundName];
        }
        if (loop)
        {
            source.loop = true;
            source.Play();
        }
        else
        {
            source.loop = false;
            source.Play();
        }
    }

    public void StopSound(string soundName)
    {
        if (activeSounds.ContainsKey(soundName))
        {
            activeSounds[soundName].Stop();
            Destroy(activeSounds[soundName]);
            activeSounds.Remove(soundName);
        }
    }

    public void SetCategoryVolume(SoundCategory category, float volume)
    {
        categoryVolumes[category] = Mathf.Clamp01(volume);

        PlayerPrefs.SetFloat(category == SoundCategory.Music ? "MusicVolume" : "SFXVolume", volume);
        PlayerPrefs.Save();

        foreach (var pair in activeSounds)
        {
            if (soundDictionary.ContainsKey(pair.Key) && soundDictionary[pair.Key].category == category)
            {
                pair.Value.volume = categoryVolumes[category] * individualVolumes[pair.Key];
            }
        }
    }
    public void SetMusicVolume(float volume)
    {
        SetCategoryVolume(SoundCategory.Music, volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetCategoryVolume(SoundCategory.SFX, volume);
    }

    public void SetIndividualVolume(string soundName, float volume)
    {
        if (!soundDictionary.ContainsKey(soundName)) return;

        soundDictionary[soundName].volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("Volume_" + soundName, volume);
        PlayerPrefs.Save();

        if (activeSounds.ContainsKey(soundName))
        {
            activeSounds[soundName].volume = categoryVolumes[soundDictionary[soundName].category] * soundDictionary[soundName].volume;
        }
    }

    public float GetCategoryVolume(SoundCategory category)
    {
        return categoryVolumes.ContainsKey(category) ? categoryVolumes[category] : 1.0f;
    }

    public float GetIndividualVolume(string soundName)
    {
        return individualVolumes.ContainsKey(soundName) ? individualVolumes[soundName] : 1.0f;
    }
    public void AddSoundToMusic(AudioSource audio)
    {
    }
}
