using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    private Ingame_TextEffect_Manager _textEffect;
    private IngameUIManager _ingameUI;
    private Ingame_Charactor_Animation_Manager _ingameAnimManager;

    private GameObject PressedButton;

    public SceneList NextScene = SceneList.NULL;
    public IsPuased GameStatus = IsPuased.Playing;
    public bool EnableLoadingScreen = true;
    public float MinLoadTime;
    public float MaxLoadTime;
    public float maxBossHP;
    public float bossHP = 280;
    public float atkDamage = 10;
    // Test
    public float TimeScale;

    private bool startPlaying;
    private int hitCount = 0;
    private int dodgeCount = 0;
    private string sceneName;
    private bool isNotPlaying = false;
    private bool isSceneChange = false;
    private float tempBeat;
    private float point;
    private bool gameEndTrigger = false;
    private float endSceneOpenTime = 1.5f;
    private bool isConfigOn = false;

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
        _textEffect = gameObject.GetComponent<Ingame_TextEffect_Manager>();
        _ingameUI = GameObject.Find("Manager").GetComponent<IngameUIManager>();
        _ingameAnimManager = gameObject.GetComponent<Ingame_Charactor_Animation_Manager>();

        _sceneData.ClearSceneInfo();
        SetTimeScale();
        
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

        if (!_ingameMusic.AudioSource.isPlaying && !gameEndTrigger && _ingameMusic.CheckTrigger())
        {
            CheckHitNotes();
            gameEndTrigger = true;
        }
    }

    // 노트가 적중할 경우 NoteObject에서 호출
    public void NoteHit(TouchInputType type)
    {
        // 게임을 플레이 하고 있을 경우에만 카운트
        if (!isNotPlaying && PressedButton != null && !isConfigOn)
        {
            switch (type)
            {
                case TouchInputType.Tab :
                    if (_textEffect.TextEffect == TextEffectEnable.Enable) PressedButton.GetComponent<ButtonController>().SelectTextType();
                    _ingameAnimManager.GetAction(AnimState.PlayerAttack);
                    Debug.Log("Hit on Time");
                    hitCount++;
                    bossHP -= atkDamage;
                    point += atkDamage;
                    _ingameUI.OnBossHPChageListener();
                    break;
                case TouchInputType.Swipe :
                    if (_textEffect.TextEffect == TextEffectEnable.Enable) PressedButton.GetComponent<ButtonController>().SelectTextType();
                    _ingameAnimManager.GetAction(AnimState.PlayerDodge);
                    Debug.Log("Dodge On Time");
                    dodgeCount++;
                    break;
            }
        }
    }

    // 노트가 미스할 경우 NoteObject에서 호출
    public void NoteMissed(TouchInputType inputType)
    {
        // 게임을 플레이 하고 있을 경우에만 호출
        if (!isNotPlaying)
        {
            switch (inputType)
            {
                case TouchInputType.Tab :
                    // 미스를 출력
                    Debug.Log("TextPrintType.Miss");
                    if (_textEffect.TextEffect == TextEffectEnable.Enable) PressedButton.GetComponent<ButtonController>().SelectTextType(TextPrintType.Miss);
                    break;
                case TouchInputType.Swipe :
                    // 데스를 출력
                    if (bossHP < maxBossHP)
                    {
                        if (bossHP >= maxBossHP - atkDamage)
                        {
                            bossHP = maxBossHP;
                        }
                        else
                        {
                            bossHP += atkDamage;
                        }
                        point -= atkDamage;
                    }
                    Debug.Log("TextPrintType.GotDamaged");
                    _ingameAnimManager.GetAction(AnimState.PlayerDamaged);
                    if (_textEffect.TextEffect == TextEffectEnable.Enable) PressedButton.GetComponent<ButtonController>().SelectTextType(TextPrintType.Damaged);
                    _ingameUI.OnBossHPChageListener();
                    break;
                case TouchInputType.NULL :
                    break;
                
            }
        }
    }

    // 일정 수 이상 노트를 적중 했을 경우 동작하는 메소드
    public void CheckHitNotes()
    {
        String str = null;
        ResultState state = ResultState.NULL;
        
        if (bossHP == maxBossHP - (maxBossHP * 1.0))
        {
            state = ResultState.BossDead;
            str = "Boss Dead";
            Debug.Log("Boss Dead");
        } else if (maxBossHP - (maxBossHP * 0.99) <= bossHP && bossHP < maxBossHP - (maxBossHP * 0.70))
        {
            state = ResultState.BossRun;
            str = "Boss Run";
            Debug.Log("Boss Run");
        } else if (maxBossHP - (maxBossHP * 0.40) <= bossHP && bossHP <= maxBossHP - (maxBossHP * 0.70))
        {
            state = ResultState.PlayerRun;
            str = "Player Run";
            Debug.Log("Player Run");
        }
        else
        {
            state = ResultState.PlayerFail;
            str = "Player Failed";
            Debug.Log("Player Failed");
        }
        
        Debug.Log("Boss HP : " + bossHP);
        Debug.Log("Clear : " + Math.Round((point / maxBossHP) * 100));
        gameObject.GetComponent<IngameUIManager>().GetGameResult(state, str, bossHP, (float)Math.Round((point / maxBossHP) * 100));
        StartCoroutine(Timer(endSceneOpenTime));
    }

    public void ResetVariables()
    {
        hitCount = 0;
        dodgeCount = 0;
        point = 0;
        sceneName= null;
        isNotPlaying = false;

        maxBossHP = bossHP;
        
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
            SceneData.Instance.SaveBGMVol(_ingameMusic.GetBGMVol());
            SceneData.Instance.SaveSFXVol(_ingameSFX.GetSFXVol());
            _sceneData.SetNextSceneName(sceneName);
        }
        else
        {
            SelecteScene();
            SceneData.Instance.SaveBGMVol(_ingameMusic.GetBGMVol());
            SceneData.Instance.SaveSFXVol(_ingameSFX.GetSFXVol());
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
        isConfigOn = true;
        tempBeat = GameObject.Find("NoteHolder").GetComponent<BeatScroller>().beatTempo;
        GameObject.Find("NoteHolder").GetComponent<BeatScroller>().beatTempo = 0;
        _ingameMusic.PauseBGM();
    }

    public void GameUnPause()
    {
        GameStatus = IsPuased.Playing;
        isConfigOn = false;
        GameObject.Find("NoteHolder").GetComponent<BeatScroller>().beatTempo = tempBeat;
        _ingameMusic.UnPauseBGM();
    }

    public void SetPressedButton(GameObject obj)
    {
        PressedButton = obj;
    }

    public void ToggleTextEffect(bool check)
    {
        if (check)
        {
            _textEffect.TextEffect = TextEffectEnable.Enable;
            SceneData.Instance.TextEffect = TextEffectEnable.Enable;
        }
        else
        {
            _textEffect.TextEffect = TextEffectEnable.Disenable;
            SceneData.Instance.TextEffect = TextEffectEnable.Disenable;
        }
    }

    private void SetTimeScale()
    {
        Time.timeScale = TimeScale;
        _ingameMusic.AudioSource.pitch = TimeScale;
    }

    IEnumerator EndTansition()
    {
        SceneAnimationManager.Instance.EndTransition();
        yield return new WaitForSeconds((Random.Range(MinLoadTime, MaxLoadTime)));
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<IngameUIManager>().EnableEndScene();
    }
}

public enum ResultState
{
    BossDead, BossRun, PlayerRun, PlayerFail, NULL
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

