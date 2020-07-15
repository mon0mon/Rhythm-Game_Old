using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMusicManager : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip Stone_Age;
    public AudioClip Middle_Age;
    public AudioClip Modern_Age;
    public AudioClip SciFi_Age;
    public AudioClip Clip;

    private bool TriggerActive = false;
    private float EndSceneOpenTime = 1.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage_StoneAge" :
                Clip = Stone_Age;
                break;
            case "Stage_MiddleAge" :
                Clip = Middle_Age;
                break;
            case "Stage_ModernAge" :
                Clip = Modern_Age;
                break;
            case "Stage_SciFiAge" :
                Clip = SciFi_Age;
                break;
        }

        AudioSource.clip = Clip;
        EndSceneOpenTime = GameObject.Find("Manager").GetComponent<IngameUIManager>().EndSceneOpenTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!AudioSource.isPlaying && TriggerActive)
        {
            TriggerActive = false;
            StartCoroutine(DelayTime(EndSceneOpenTime));
        }
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

    IEnumerator DelayTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameObject.Find("Manager").GetComponent<IngameUIManager>().EnableEndScene();
    }
}
