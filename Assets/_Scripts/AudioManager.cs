using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioClip _playerShotSound;
    [SerializeField] List<AudioClip> _enemyBombSounds;

    [SerializeField] AudioSource _playerShotSource;
    [SerializeField] AudioSource _enemyBombSource;

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

    private void Start()
    {
        _playerShotSource.clip = _playerShotSound;
    }

    public void PlayPlayerShotSound()
    {
        _playerShotSource.pitch = Random.Range(0.85f, 1.15f);
        _playerShotSource.Play();
    }

    public void PlayEnemyBombSound()
    {
        int randomIndex = Random.Range(0, _enemyBombSounds.Count);
        _enemyBombSource.clip = _enemyBombSounds[randomIndex];
        _enemyBombSource.pitch = Random.Range(0.9f, 1.1f);
        _enemyBombSource.Play();
    }
}