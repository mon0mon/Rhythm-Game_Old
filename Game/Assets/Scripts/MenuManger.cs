using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MenuManger : MonoBehaviour
{
    private static MenuManger instance = null;
    private SceneData _sceneData;
    private LoadingSceneManager _loading;
    private SFXManager _SFXManager;
    
    public SceneList NextScene = SceneList.NULL;
    public bool EnableLoadingScreen = false;
    public float MinLoadTime = 1.5f;
    public float MaxLoadTime = 2.0f;
    public bool EnableStartTransition = true;
    
    private string sceneName;
    private bool loadingTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        StartCoroutine(LateEnable(0.01f));
    }

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
        _loading = gameObject.GetComponent<LoadingSceneManager>();
        _sceneData = GameObject.Find("SaveData").GetComponent<SceneData>();
        _sceneData.ClearSceneInfo();
        _SFXManager = GameObject.Find("MainMenuSFX").GetComponent<SFXManager>();
        StartCoroutine(LateStart(0.01f));
        StopAllCoroutines();

        if (EnableStartTransition)
        { 
            SceneAnimationManager.Instance.StartTransition();
        }
        
        ResetVariables();
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

    public void MoveNextScene()
    {
        // MainMenuAnimation 종료
        _SFXManager.PlayLoadingSFX();
        _sceneData.transform.GetComponentInChildren<AnimationManager>().enabled = false;
        loadingTrigger = true;
        if (EnableLoadingScreen)
        {
            _loading.enabled = true;
            SelecteScene();
            SceneData.Instance.SaveBGMVol(GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().GetBGMVol());
            SceneData.Instance.SaveSFXVol(GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().GetSFXVol());
            _sceneData.SetNextSceneName(sceneName);
        }
        else
        {
            SelecteScene();
            StartCoroutine(EndTansition());
        }
    }
    
    private void ResetVariables()
    {
        GameObject.Find("SaveData").GetComponent<SceneData>().SetNextSceneName(null);
    }

    public void MenuAnimation(bool check)
    {
        SceneData.Instance.SetMenuAnimationState(check);
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

    IEnumerator EndTansition()
    {
        SceneAnimationManager.Instance.EndTransition();
        yield return new WaitForSeconds((Random.Range(MinLoadTime, MaxLoadTime)));
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LateStart(float waitTime)
    {
        _sceneData.transform.GetComponentInChildren<AnimationManager>().enabled = true;
        yield return new WaitForSeconds(waitTime);
    }

    IEnumerator LateEnable(float waitTime)
    {
        new WaitForSeconds(waitTime);
        GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().CheckMusic(loadingTrigger);
        yield return null;
    }
}