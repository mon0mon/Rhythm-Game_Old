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

    private float BGM_Vol = -15;
    private float SFX_Vol = -15;

    private void Start()
    {
        Music = gameObject.GetComponent<AudioSource>();
        Music.Play();

        // 씬데이터에서 볼륨 조절 설정 가져오기
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
        BGM_Vol = value;
        AudioMixer.SetFloat("BGM_Vol", value);
    }

    public void VolChangeSFX(float value)
    {
        SFX_Vol = value;
        AudioMixer.SetFloat("SFX_Vol", value);
    }

    public float GetBGMVol()
    {
        return BGM_Vol;
    }

    public float GetSFXVol()
    {
        return SFX_Vol;
    }
}
