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
    private IngameSFXManager _SFXManager;

    private GameObject PressedButton;

    public SceneList Stage = SceneList.NULL;
    public SceneList NextScene = SceneList.NULL;
    public IsPuased GameStatus = IsPuased.Playing;
    public bool EnableLoadingScreen = true;
    public float MinLoadTime;
    public float MaxLoadTime;
    public float maxScore;
    public float score = 100;
    public float AddScoreAmount = 10;
    public int DodgeFailPenaltyMul = 3;
    public float Touch_Exploit_tolerance_Time = 0.3f;
    public float Touch_Exploit_Punish_Time = 0.8f;
    
    // Test
    public float TimeScale;
    public int TextEffectDelayFrame = 5;

    private bool startPlaying;
    private int hitCount = 0;
    private int dodgeCount = 0;
    private int missCount = 0;
    private int badCount = 0;
    private string sceneName;
    private bool isNotPlaying = false;
    private bool isSceneChange = false;
    private float tempBeat;
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
            _beatScroller.SetStart(true);
            // 음악 메소드인 IngameMusicManager 호출로 변경
            _ingameMusic.PlayBGM();
        }
        
        // 점수가 맥스를 넘겼을 경우 보스 사망 애니메이션 출력
        if (score >= 200)
        {
            _ingameAnimManager.GetAction(AnimState.BossDie);
        }

        // 음악이 끝났을 경우 종료 화면 출력
        if (!_ingameMusic.AudioSource.isPlaying && !gameEndTrigger && _ingameMusic.CheckTrigger())
        {
            CheckHitNotes();
            gameEndTrigger = true;
        }

        // 인게임 체력바가 0 밑으로 떨어질 경우 게임 오버
        if (score <= 0 && !gameEndTrigger)
        {
            CheckHitNotes();
            _ingameMusic.StopBGM();
            GameObject.Find("NoteHolder").GetComponent<BeatScroller>().SetStop(true);
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
                    Debug.Log("Hit on Time");
                    _ingameAnimManager.GetAction(AnimState.PlayerAttack);
                    _ingameSFX.PlayStoneAgeSFX(StoneAge_SFX.Babarian_Attack);
                    _ingameSFX.PlayStoneAgeSFX(StoneAge_SFX.Mammoth_Damaged);
                    hitCount++;
                    score += AddScoreAmount;
                    _ingameUI.OnBossHPChageListener();
                    if (_textEffect.TextEffect == TextEffectEnable.Enable)
                        StartCoroutine(PrintText(TextEffectDelayFrame, TextPrintType.Hit,
                            PressedButton.GetComponent<ButtonController>()));
                    // PressedButton.GetComponent<ButtonController>().SelectTextType();
                    break;
                case TouchInputType.Swipe :
                    _ingameAnimManager.GetAction(AnimState.PlayerDodge);
                    _ingameSFX.PlayStoneAgeSFX(StoneAge_SFX.Babarian_Dodge);
                    Debug.Log("Dodge On Time");
                    dodgeCount++;
                    if (_textEffect.TextEffect == TextEffectEnable.Enable)
                        StartCoroutine(PrintText(TextEffectDelayFrame, TextPrintType.Dodge,
                            PressedButton.GetComponent<ButtonController>()));
                    // PressedButton.GetComponent<ButtonController>().SelectTextType();
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
                    _ingameAnimManager.GetAction(AnimState.PlayerMiss);
                    if (_textEffect.TextEffect == TextEffectEnable.Enable)
                        StartCoroutine(PrintText(TextEffectDelayFrame, TextPrintType.Miss,
                            PressedButton.GetComponent<ButtonController>()));
                    // PressedButton.GetComponent<ButtonController>().SelectTextType(TextPrintType.Miss);
                    missCount++;
                    break;
                case TouchInputType.Swipe :
                    // 플레이어 피격 모션 출력
                    _ingameAnimManager.GetAction(AnimState.PlayerDamaged);
                    _ingameSFX.PlayStoneAgeSFX(StoneAge_SFX.Mammoth_Attack);
                    // 데미지를 출력
                    score -= (AddScoreAmount * DodgeFailPenaltyMul);
                    _ingameUI.OnBossHPChageListener();
                    badCount++;
                    if (_textEffect.TextEffect == TextEffectEnable.Enable)
                        StartCoroutine(PrintText(TextEffectDelayFrame, TextPrintType.Damaged,
                            PressedButton.GetComponent<ButtonController>()));
                    // PressedButton.GetComponent<ButtonController>().SelectTextType(TextPrintType.Damaged);
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
        float cntClear = (float)Math.Round(score / maxScore * 100);
        Debug.Log(cntClear);

        if (score <= 0) score = 0;
        
        // 맘모스와 원시인 그로기 상태 확정되면 그때 추가하기
        // 클리어 퍼센트에 따른 엔딩 분기 조절
        if (cntClear >= 100)
        {
            state = ResultState.BossDead;
            str = "Boss Dead";
            Debug.Log("Boss Dead");
        } else if (85 <= cntClear && cntClear < 100)
        {
            state = ResultState.BossGroggy;
            str = "Boss Groggy";
            Debug.Log("Boss Groggy");
        } else if (50 <= cntClear && cntClear < 85)
        {
            state = ResultState.BossRun;
            str = "Boss Run";
            Debug.Log("Boss Run");
        } else if (0 < cntClear && cntClear < 50)
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

        Debug.Log("Score : " + score);
        Debug.Log("Clear : " + Math.Round(((score / maxScore) * 100)) + "%");
        if (score >= maxScore) gameObject.GetComponent<IngameUIManager>().GetGameResult(state, str, score, 
            (float)Math.Round(((score / maxScore) * 100)), new int[4] {hitCount, dodgeCount, missCount, badCount});
        else gameObject.GetComponent<IngameUIManager>().GetGameResult(state, str, score, 
            (float)Math.Round(((score / maxScore) * 100)), new int[4] {hitCount, dodgeCount, missCount, badCount});
        StartCoroutine(Timer(endSceneOpenTime));
    }

    public void ResetVariables()
    {
        hitCount = 0;
        dodgeCount = 0;
        missCount = 0;
        badCount = 0;
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

    IEnumerator PrintText(int waitTime, TextPrintType tp, ButtonController btnController)
    {
        yield return new WaitForSeconds(waitTime * 0.016f);
        switch (tp)
        {
            case TextPrintType.Hit :
            case TextPrintType.Dodge :
                btnController.SelectTextType();
                break;
            default :
                btnController.SelectTextType(tp);
                break;
        }
    }
}

public enum ResultState
{
    BossDead, BossRun, BossGroggy, PlayerRun, PlayerFail, NULL
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

