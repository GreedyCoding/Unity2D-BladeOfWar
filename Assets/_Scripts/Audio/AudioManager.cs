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

    [Header("Enemy Death SFX")]
    [SerializeField] List<AudioSource> _enemyExplosionSources;
    [SerializeField] List<AudioClip> _enemyExplosionClips;

    [Header("Enemy Death SFX")]
    [SerializeField] List<AudioSource> _enemyAttackSources;
    [SerializeField] List<AudioClip> _enemyProjectileClips;
    [SerializeField] AudioClip _enemyRocketClip;
    [SerializeField] AudioClip _enemyLaserClip;


    [Header("Events")]
    [SerializeField] VoidEventChannelSO _deathSoundEventChannel;

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

    private void OnEnable()
    {
        _deathSoundEventChannel.OnEventRaised += PlayRandomShortExplosion;
    }

    private void OnDisable()
    {
        _deathSoundEventChannel.OnEventRaised -= PlayRandomShortExplosion;
    }

    private void PlayClipFromListOfSources(List<AudioSource> audioSources, AudioClip audioClip, bool randomizePitch)
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = audioClip;

                if (randomizePitch)
                {
                    source.pitch = Random.Range(0.85f, 1.15f);
                }
                else
                {
                    source.pitch = 1f;
                }

                source.Play();
                return;
            }
        }
    }

    public void PlayRandomShortExplosion()
    {
        int randomIndex = Random.Range(0, _enemyExplosionClips.Count);
        PlayClipFromListOfSources(_enemyExplosionSources, _enemyExplosionClips[randomIndex], false);
    }



    public void PlayEnemyProjectileSound()
    {
        int randomIndex = Random.Range(0, _enemyProjectileClips.Count);
        PlayClipFromListOfSources(_enemyAttackSources, _enemyProjectileClips[randomIndex], false);
    }

    public void PlayEnemyRocketSound()
    {
        PlayClipFromListOfSources(_enemyAttackSources, _enemyRocketClip, true);
    }

    public void PlayEnemyLaserSound()
    {
        PlayClipFromListOfSources(_enemyAttackSources, _enemyLaserClip, true);
    }

    public void SetMainMixerVolume(float volume)
    {
        _mainAudioMixer.SetFloat(Constants.MAIN_MIXER_VOLUME, volume);
    }

    public void SetMusicVolume(float volume)
    {
        _mainAudioMixer.SetFloat(Constants.MUSIC_VOLUME, volume);
    }

    public void SetSfxVolume(float volume)
    {
        _mainAudioMixer.SetFloat(Constants.SFX_VOLUME, volume);
    }

}