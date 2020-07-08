using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    
    public AudioSource theMusic;
    public BeatScroller theBS;
    
    public bool startPlaying;

    private int hitCount = 0;
    
    private void Start()
    {
        Initialize();
    }
    
    // 싱글톤 구성
    public static GameManager Instance
    {
        get { return instance; }
    }
    
    private void Initialize()
    {
        // 싱글톤 구성
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        
        ResetVariables();
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (!startPlaying)
        {
            startPlaying = true;
            theBS.hasStarted = true;
                
            theMusic.Play();
        }
        
        CheckHitNotes();
    }

    public void NoteHit()
    {
        hitCount++;
        Debug.Log("Hit On Time");
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");
    }

    public void CheckHitNotes()
    {
        if (hitCount >= 5)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Loading_Scene");
        }
    }

    public void ResetVariables()
    {
        hitCount = 0;
    }
}
