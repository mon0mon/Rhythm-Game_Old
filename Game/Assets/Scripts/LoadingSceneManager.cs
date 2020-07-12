using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadingSceneManager : MonoBehaviour
{
    public static SceneList list;
    public bool AutoLoading = true;
    public float MinLoadingTime = 4;
    public float MaxLoadingTime = 10;
    public float TransitionTime = 1f;
    public bool StartTransitionOn = true;

    private static LoadingSceneManager instance = null;

    private SceneList scene = SceneList.LoadingScene;
    private AsyncOperation async;
    private string str;
    private bool canOpen = true;
    private bool isSceneNameSet = false;
    private static string nextSceneName;

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
        
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        
        if (GameObject.Find("SaveData").GetComponent<SceneData>().CheckIsThisSceneNext())
        {
            str = GameObject.Find("SaveData").GetComponent<SceneData>().GetNextSceneName();
        }
        
        if (StartTransitionOn)
        {
            // SceneAnimationManager.Instance.StartTransition();
            // new WaitForSeconds(TransitionTime);
            StartCoroutine(TransitionAnimation(SceneTransition.Start));
        }
        
        if (AutoLoading)
        {
            StartCoroutine("Load");
        }
    }

    private void Initialize()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        // DontDestroyOnLoad(gameObject);

        instance = this;
        list = scene;
        isSceneNameSet = false;
    }

    public static LoadingSceneManager Instance => instance;

    public void StartLoad()
    {
        StartCoroutine("Load");
    }

    // 로딩
    IEnumerator Load()
    {
        // 다른 클래스에서 SceneName을 설정한 적이 없고, str 값이 없을 경우
        if (!isSceneNameSet && str == null)
        {
            // Enum에 있는 대로 다음 씬을 설정
            SelecteScene();
        }
        async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(str); // 열고 싶은 씬
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            float progress = async.progress;

            yield return true;

            if (canOpen)
            {
                float num = RandomNumber(MinLoadingTime, MaxLoadingTime);
                yield return new WaitForSeconds(num);
                StartCoroutine(TransitionAnimation(SceneTransition.End));
                yield return new WaitForSeconds(2f);
                // SceneAnimationManager.Instance.EndTransition();
                // new WaitForSeconds(TransitionTime);
                async.allowSceneActivation = true;
            }
        }
    }

    public IEnumerator TransitionAnimation(SceneTransition st)
    {
        switch (st)
        {
            case SceneTransition.Start:
                SceneAnimationManager.Instance.StartTransition();
                new WaitForSeconds(TransitionTime);
                break;
            case SceneTransition.End:
                SceneAnimationManager.Instance.EndTransition();
                new WaitForSeconds(TransitionTime);
                break;
        }

        yield return null;
    }

    public float RandomNumber(float a, float b)
    {
        return Random.Range(a, b);
    }

    private void SelecteScene()
    {
        switch (list)
        {
            case SceneList.LoadingScene:
                str = "Scenes/Loading_Scene";
                break;
            case SceneList.TouchSwipe_Test_Mobile:
                str = "Scenes/TouchSwipe_Test_Mobile";
                break;
            case SceneList.RhythmGame_Test_PC:
                str = "Scenes/RhythmGame_Test_PC";
                break;
            case SceneList.StoneAge:
                str = "Scenes/Stage_StoneAge";
                break;
            case SceneList.Touch_Test:
                str = "Scenes/TouchTset";
                break;
            default :
                str = "Scenes/Loading_Scene";
                break;
        }
    }

    public enum SceneTransition
    {
        Start, End
    }

    public void setTimer(int minTime, int maxTime)
    {
        MinLoadingTime = minTime;
        MaxLoadingTime = maxTime;
    }

    public void resetTimer()
    {
        MinLoadingTime = 4;
        MaxLoadingTime = 10;
    }

    public void setSceneName(String temp)
    {
        str = temp;
        isSceneNameSet = true;
    }

    public void resetSceneName()
    {
        str = null;
        isSceneNameSet = false;
    }
}
