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
    private SceneData _sceneData;
    
    public AudioSource theMusic;
    
    public bool startPlaying;
    public bool moveNextLevel = true;
    public SceneList NextScene = SceneList.NULL;

    private int hitCount = 0;
    private string sceneName;
    private bool isNotPlaying = false;
    
    private void Start()
    {
        Initialize();
    }
    
    // 싱글톤 구성
    public static GameManager Instance
    {
        get { return instance; }
    }
    
    // 초기 설정
    private void Initialize()
    {
        // 싱글톤 구성
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        // DontDestroyOnLoad(gameObject);
        instance = this;
        
        // 변수 초기화
        ResetVariables();

        // 컴포넌트 연결
        _beatScroller = GameObject.Find("NoteHolder").GetComponent<BeatScroller>();
        _loading = gameObject.GetComponent<LoadingSceneManager>();
        _sceneData = GameObject.Find("SaveData").GetComponent<SceneData>();
        
        _sceneData.ClearSceneInfo();
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        // 게임이 시작되지 않았을 경우 실행하게 만듦
        if (!startPlaying)
        {
            startPlaying = true;
            _beatScroller.setStart(true);
            theMusic.Play();
        }
        
        CheckHitNotes();
    }

    // 노트가 적중할 경우 NoteObject에서 호출
    public void NoteHit()
    {
        // 게임을 플레이 하고 있을 경우에만 카운트
        if (!isNotPlaying)
        {
            hitCount++;
            Debug.Log("Hit On Time");   
        }
    }

    // 노트가 미스할 경우 NoteObject에서 호출
    public void NoteMissed()
    {
        // 게임을 플레이 하고 있을 경우에만 호출
        if (!isNotPlaying)
        { 
            Debug.Log("Missed Note");
        }
    }

    // 일정 수 이상 노트를 적중 했을 경우 동작하는 메소드
    public void CheckHitNotes()
    {
        // 히트 카운트가 5회 이상, moveNextLevel이 참 일 경우 실행
        if (hitCount >= 50 && moveNextLevel)
        {
            // 다음 씬으로 로딩
            isNotPlaying = true;
            _loading.StartLoad();
            SelecteScene();
            _sceneData.SetNextSceneName(sceneName);
            moveNextLevel = false;
        }
    }

    public void ResetVariables()
    {
        hitCount = 0;
        sceneName= null;
        isNotPlaying = false;
        
        GameObject.Find("SaveData").GetComponent<SceneData>().SetNextSceneName(null);
    }
    
    private void SelecteScene()
    {
        switch (NextScene)
        {
            case SceneList.LoadingScene:
                sceneName = "Scenes/Loading_Scene";
                break;
            case SceneList.TouchSwipe_Test_Mobile:
                sceneName = "Scenes/TouchSwipe_Test_Mobile";
                break;
            case SceneList.RhythmGame_Test_PC:
                sceneName = "Scenes/RhythmGame_Test_PC";
                break;
            case SceneList.StoneAge:
                sceneName = "Scenes/Stage_StoneAge";
                break;
            case SceneList.Touch_Test:
                sceneName = "Scenes/TouchTset";
                break;
            case SceneList.Main_Scene:
                sceneName = "Scenes/Main_Scene";
                break;
            case SceneList.Start_Scene:
                sceneName = "Scenes/Start_Scene";
                break;
            default:
                sceneName = "Scenes/Start_Scene";
                break;
        }
    }
}

public enum SceneList
{
    // 디버그용 씬
    RhythmGame_Test_PC, TouchSwipe_Test_Mobile, Touch_Test, 
    // 게임 플레이 스테이지
    StoneAge, MiddleAge, ModernAge, SciFi,
    // 메뉴 화면 씬
    Main_Scene, Start_Scene,LoadingScene,
    // 예외처리
    NULL
}
