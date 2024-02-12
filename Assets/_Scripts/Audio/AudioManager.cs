using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioMixer _mainAudioMixer;

    [Header("Gameplay Music")]
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioClip _beginningMusic;
    [SerializeField] AudioClip _halftimeMusic;
    [SerializeField] AudioClip _bossMusic;

    [Header("Enemy Death SFX")]
    [SerializeField] List<AudioSource> _enemyExplosionSources;
    [SerializeField] List<AudioClip> _enemyExplosionClips;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayRandomShortExplosion()
    {
        int randomIndex = Random.Range(0, _enemyExplosionClips.Count);
        foreach (var source in _enemyExplosionSources)
        {
            if (!source.isPlaying)
            {
                source.clip = _enemyExplosionClips[randomIndex];
                source.Play();
                return;
            }
        }
    }

    public void SetMainMixerVolume(float volume)
    {
        _mainAudioMixer.SetFloat("MainMixerVolume", volume);
    }

}