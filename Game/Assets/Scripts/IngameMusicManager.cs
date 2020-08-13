using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class IngameMusicManager : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public AudioSource AudioSource;
    public AudioClip Stone_Age;
    public AudioClip Middle_Age;
    public AudioClip Modern_Age;
    public AudioClip SciFi_Age;
    public AudioClip Clip;
    
    private bool TriggerActive = false;
    private float EndSceneOpenTime = 1.5f;
    private float BGM_Vol;
    private string _songInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage_StoneAge" :
                Clip = Stone_Age;
                _songInfo = "Jungle Run - SoundJay";
                break;
            case "Stage_MiddleAge" :
                Clip = Middle_Age;
                _songInfo = "SongName - Artist";
                break;
            case "Stage_ModernAge" :
                Clip = Modern_Age;
                _songInfo = "SongName - Artist";
                break;
            case "Stage_SciFiAge" :
                Clip = SciFi_Age;
                _songInfo = "뿅뿅뿅울렐레레렐 - SellBuyMusic";
                break;
        }

        AudioSource.clip = Clip;
        EndSceneOpenTime = GameObject.Find("Manager").GetComponent<IngameUIManager>().EndSceneOpenTime;
        AudioMixer.GetFloat("BGM_Vol", out BGM_Vol);
        AudioMixer.SetFloat("BGM_Speed", 1f);

        StartCoroutine(LateStart(0.01f));
    }

    // Update is called once per frame
    void Update()
    {
        // if (!AudioSource.isPlaying && TriggerActive)
        // {
        //     TriggerActive = false;
        //     StartCoroutine(DelayTime(EndSceneOpenTime));
        // }
    }

    public void PlayBGM()
    {
        TriggerActive = true;
        AudioSource.Play();
    }
    
    public void StopBGM()
    {
        TriggerActive = false;
        AudioSource.Stop();
    }
    
    public void PauseBGM()
    {
        TriggerActive = false;
        AudioSource.Pause();
    }
    
    public void UnPauseBGM()
    {
        TriggerActive = true;
        AudioSource.UnPause();
    }

    public float GetPlayTime()
    {
        return AudioSource.time;
    }

    public void VolChangeBGM(float value)
    {
        BGM_Vol = value;
        if (value == -30f)
        {
            AudioMixer.SetFloat("BGM_Vol", -100f);
        }
        else
        {
            AudioMixer.SetFloat("BGM_Vol", value);
        }
    }

    public float GetBGMVol()
    {
        return BGM_Vol;
    }

    public float GetAudioLength()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage_StoneAge" :
                return Stone_Age.length;
            case "Stage_MiddleAge" :
                return Middle_Age.length;
            case "Stage_ModernAge" :
                return Modern_Age.length;
            case "Stage_SciFiAge" :
                return SciFi_Age.length;
        }

        return 0f;
    }

    public bool CheckTrigger()
    {
        return TriggerActive;
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameObject.Find("Manager").GetComponent<IngameUIManager>().SetSongInfo(_songInfo);
    }
}
