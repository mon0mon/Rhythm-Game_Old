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
    
    public SceneList NextScene = SceneList.NULL;
    public bool EnableLoadingScreen = false;
    public float MinLoadTime = 1.5f;
    public float MaxLoadTime = 2.0f;
    public bool EnableStartTransition = true;
    
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
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
        _sceneData.transform.GetComponentInChildren<AnimationManager>().enabled = false;
        if (EnableLoadingScreen)
        {
            _loading.enabled = true;
            SelecteScene();
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
            case SceneList.MiddleAge :
                sceneName = "Scenes/Start_Scene";
                break;
            case SceneList.ModernAge :
                sceneName = "Scenes/Start_Scene";
                break;
            case SceneList.SciFi :
                sceneName = "Scenes/Start_Scene";
                break;
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
}