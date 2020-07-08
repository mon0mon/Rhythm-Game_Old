using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private BeatScroller _beatScroller;
    private LoadingSceneManager _loading;
    
    public AudioSource theMusic;
    
    public bool startPlaying;
    public NextLevelScene enableNextLevel = NextLevelScene.DisableNextLevel;

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

        _beatScroller = GameObject.Find("NoteHolder").GetComponent<BeatScroller>();
        _loading = gameObject.GetComponent<LoadingSceneManager>();
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (!startPlaying)
        {
            startPlaying = true;
            _beatScroller.setStart(true);
                
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
        if ((hitCount >= 5))
        {
            switch (enableNextLevel)
            {
                case NextLevelScene.DisableNextLevel:
                    Debug.Log("DisableNextLevel");
                    break;
                case NextLevelScene.EnableNextLevel:
                    SceneManager.LoadScene("Loading_Scene");
                    break;
            }
        }
    }

    public void ResetVariables()
    {
        hitCount = 0;
    }
}

public enum NextLevelScene
{
    EnableNextLevel,
    DisableNextLevel
}
