using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource Music;
    public AudioMixer AudioMixer;

    private void Start()
    {
        Music = gameObject.GetComponent<AudioSource>();
        Music.Play();
    }

    private void Update()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Start_Scene" :
            case "Main_Scene" :
                break;
            default:
                Music.Stop();
                break;
        }
    }

    public void StartMainMenuMusic()
    {
        Music.Play();
    }

    public void StopMainMenuMusic()
    {
        Music.Stop();
    }
    
    public void PauseMainMenuMusic()
    {
        Music.Pause();
    }

    public void UnpauseMainMenuMusic()
    {
        Music.UnPause();
    }

    public void VolChangeBGM(float value)
    {
        AudioMixer.SetFloat("BGM_Vol", value);
    }

    public void VolChangeSFX(float value)
    {
        AudioMixer.SetFloat("SFX_Vol", value);
    }
}
