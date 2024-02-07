using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioMixer audioMixer;

    [Header("Gameplay Music")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip beginningMusic;
    [SerializeField] AudioClip halftimeMusic;
    [SerializeField] AudioClip bossMusic;

    [Header("Enemy Death SFX")]
    [SerializeField] List<AudioSource> enemyExplosionSources;
    [SerializeField] List<AudioClip> enemyExplosionClips;


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
        int randomIndex = Random.Range(0, enemyExplosionClips.Count);
        foreach (var source in enemyExplosionSources)
        {
            if (!source.isPlaying)
            {
                source.clip = enemyExplosionClips[randomIndex];
                source.Play();
                return;
            }
        }
    }
}