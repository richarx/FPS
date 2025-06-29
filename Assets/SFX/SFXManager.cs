using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private AudioSource audioSourcePrefab3D;
    
    public static SFXManager instance;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    public AudioSource PlayRandomSFX(List<AudioClip> clips, float volume = 0.1f, float delay = 0.0f, bool loop = false)
    {
        int index = Random.Range(0, clips.Count);

        return PlaySFXAtLocation(clips[index], null, volume, delay, loop);
    }
    
    public AudioSource PlayRandomSFXAtLocation(List<AudioClip> clips, Transform target, float volume = 0.1f, float delay = 0.0f, bool loop = false)
    {
        int index = Random.Range(0, clips.Count);

        return PlaySFXAtLocation(clips[index], target, volume, delay, loop);
    }
    
    public AudioSource PlaySFX(AudioClip clip, float volume = 0.1f, float delay = 0.0f, bool loop = false)
    {
        return PlaySFXAtLocation(clip, null, volume, delay, loop);
    }

    public AudioSource PlaySFXAtLocation(AudioClip clip, Transform target, float volume = 0.1f, float delay = 0.0f, bool loop = false)
    {
        Transform parent = target != null ? target : transform;

        AudioSource source = Instantiate(target != null ? audioSourcePrefab3D : audioSourcePrefab, parent.position, Quaternion.identity, parent);

        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.pitch = Random.Range(0.98f, 1.02f);
        if (delay <= 0.0f)
            source.Play();
        else
            source.PlayDelayed(delay);
        
        if (!loop)
            Destroy(source.gameObject, clip.length + delay);

        return source;
    }
}
