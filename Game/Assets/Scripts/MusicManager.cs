using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioSource Music;
    public AudioMixer AudioMixer;

    private void Start()
    {
        Music = gameObject.GetComponent<AudioSource>();
        Music.Play();
    }

    public void StartMainMenuMusic()
    {
        
    }

    public void StopMainMenuMusic()
    {
        
    }

    public void VolChangeBGM(float value)
    {
        if (value != 0)
        {
            value = value * 10;
        }
        else
        {
            value = -80f;
        }
        
        Debug.Log(value);
        Debug.Log(AudioMixer.outputAudioMixerGroup.name);
        // Debug.Log(AudioMixer.FindMatchingGroups("BGM").GetValue(0));
    }
}
