using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private BeatScroller _beatScroller;
    private LoadingSceneManager _loading;
    private SceneData _sceneData;
    private IngameMusicManager _ingameMusic;
    private IngameSFXManager _ingameSFX;

    public SceneList NextScene = SceneList.NULL;
    public IsPuased GameStatus = IsPuased.Playing;
    public bool EnableLoadingScreen = true;
    public float MinLoadTime;
    public float MaxLoadTime;

    private bool startPlaying;
    private int hitCount = 0;
    private string sceneName;
    private bool isNotPlaying = false;
    private bool isSceneChange = false;
    private GameObject PressedButton;
    
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
        _ingameMusic = GameObject.Find("BGM").GetComponent<IngameMusicManager>();
        _ingameSFX = GameObject.Find("SFX").GetComponent<IngameSFXManager>();
        
        _sceneData.ClearSceneInfo();
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        // 게임이 시작되지 않았을 경우 실행하게 만듦
        if (!startPlaying && !isSceneChange)
        {
            startPlaying = true;
            _beatScroller.setStart(true);
            // 음악 메소드인 IngameMusicManager 호출로 변경
            _ingameMusic.PlayBGM();
        }
        
        // if (!_ingameMusic.AudioSource.isPlaying)
        
        CheckHitNotes();
    }

    // 노트가 적중할 경우 NoteObject에서 호출
    public void NoteHit(TouchInputType type)
    {
        // 게임을 플레이 하고 있을 경우에만 카운트
        if (!isNotPlaying)
        {
            switch (type)
            {
                case TouchInputType.Tab :
                    if (PressedButton != null)
                    {
                        PressedButton.GetComponent<ButtonController>().SelectTextType();
                    }
                    break;
                case TouchInputType.Swipe :
                    break;
            }
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
        // // 히트 카운트가 5회 이상, moveNextLevel이 참 일 경우 실행
        // if (hitCount >= 50 && moveNextLevel)
        // {
        //     // 다음 씬으로 로딩
        //     MoveNextScene();
        // }
    }

    public void ResetVariables()
    {
        hitCount = 0;
        sceneName= null;
        isNotPlaying = false;
        
        GameObject.Find("SaveData").GetComponent<SceneData>().SetNextSceneName(null);
    }

    public void MoveNextScene()
    {
        isSceneChange = true;
        if (EnableLoadingScreen)
        {
            isNotPlaying = true;
            _loading.StartLoad();
            SelecteScene();
            _sceneData.SetNextSceneName(sceneName);
        }
        else
        {
            SelecteScene();
            StartCoroutine(EndTansition());
        }
    }
    
    private void SelecteScene()
    {
        switch (NextScene)
        {
            // Debug Scene
            case SceneList.TouchSwipe_Test_Mobile:
                sceneName = "Scenes/TouchSwipe_Test_Mobile";
                break;
            case SceneList.RhythmGame_Test_PC:
                sceneName = "Scenes/RhythmGame_Test_PC";
                break;
            case SceneList.Touch_Test:
                sceneName = "Scenes/TouchTset";
                break;
            // Ingame MenuScene
            case SceneList.Start_Scene:
                sceneName = "Scenes/Start_Scene";
                break;
            case SceneList.Main_Scene:
                sceneName = "Scenes/Main_Scene";
                break;
            case SceneList.LoadingScene:
                sceneName = "Scenes/Loading_Scene";
                break;
            // Ingame StageScene
            case SceneList.StoneAge:
                sceneName = "Scenes/Stage_StoneAge";
                break;
            case SceneList.MiddleAge :
            case SceneList.ModernAge :
            case SceneList.SciFi :
            default:
                sceneName = "Scenes/Start_Scene";
                break;
        }
    }

    public void GamePause()
    {
        GameStatus = IsPuased.Paused;
        Time.timeScale = 0.0f;
    }

    public void GameUnPause()
    {
        GameStatus = IsPuased.Playing;
        Time.timeScale = 1.0f;
    }

    public void SetPressedButton(GameObject obj)
    {
        PressedButton = obj;
    }

    IEnumerator EndTansition()
    {
        SceneAnimationManager.Instance.EndTransition();
        yield return new WaitForSeconds((Random.Range(MinLoadTime, MaxLoadTime)));
        SceneManager.LoadScene(sceneName);
    }
}

public enum IsPuased
{
    Paused, Playing
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

