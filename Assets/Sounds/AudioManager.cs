using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM")]
    public AudioSource bgmSource;
    public AudioClip[] bgmClips;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip[] sfxClips;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        // Singleton
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
    }

    void Start()
    {
        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
        PlayBGM(0); // auto play BGM pertama
    }

    // ===================== BGM =====================

    public void PlayBGM(int index)
    {
        if (index >= bgmClips.Length || bgmClips[index] == null) return;
        bgmSource.volume = 1f;
        bgmSource.clip = bgmClips[index];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = volume;
    }

    // ===================== SFX =====================

    public void PlaySFX(int index)
    {
        if (index >= sfxClips.Length || sfxClips[index] == null) return;

        sfxSource.PlayOneShot(sfxClips[index], sfxVolume);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
    }
}