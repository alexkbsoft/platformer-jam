using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private List<AudioSource> sounds = new List<AudioSource>();
    private AudioSource ambient, music;
    
    public float SoundsVolume { set => SetSoundsVolume(value); }
    public float AmbientVolume { set => ambient.volume = value; }
    public float MusicVolume { set => music.volume = value; }
    
    private void Awake() => FindSources();
    
    private void SetSoundsVolume(float value)
    {
        ambient.volume = value;
        
        foreach (var sound in sounds)
            sound.volume = value;
    }
    
    private void FindSources()
    {
        var sources = FindObjectsOfType<AudioSource>();

        foreach (var source in sources)
        {
            if (source.CompareTag("Ambient")) ambient = source;
            else if (source.CompareTag("Music")) music = source;
            else sounds.Add(source);
        }
    }
}