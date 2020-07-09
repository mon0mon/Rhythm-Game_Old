using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class LoadingSceneManager : MonoBehaviour
{
    public static SceneList list;
    public SceneList scene;
    public bool AutoLoading = true;
    public int MinLoadingTime = 4;
    public int MaxLoadingTime = 10;

    private static LoadingSceneManager instance = null;

    private AsyncOperation async;
    private string str;
    private bool canOpen = true;
    private Random rand = new Random();
    private bool isSceneNameSet = false;
    private static string nextSceneName;

    public enum SceneList
    {
        LoadingScene, RhythmGame_Test_PC, TouchSwipe_Test_Mobile, NULL
    }
        
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        
        if (GameObject.Find("SaveData").GetComponent<SceneData>().CheckIsThisSceneNext())
        {
            str = GameObject.Find("SaveData").GetComponent<SceneData>().GetNextSceneName();
            Debug.Log("Start : " + str);
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
        Debug.Log("Load : " + str);
        // 다른 클래스에서 SceneName을 설정한 적이 없고, str 값이 없을 경우
        if (!isSceneNameSet && str == null)
        {
            // Enum에 있는 대로 다음 씬을 설정
            SelecteScene();
        }

        Debug.Log(str);
        async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(str); // 열고 싶은 씬
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            float progress = async.progress;
            Debug.Log("Progress : " + progress);

            yield return true;

            if (canOpen)
            {
                yield return new WaitForSeconds(RandomNumber(MinLoadingTime, MaxLoadingTime));
                async.allowSceneActivation = true;
            }
        }
    }

    public int RandomNumber(int a, int b)
    {
        int temp = rand.Next(a, b);
        Debug.Log("RandomNumber : " + temp);
        return temp;
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
            default :
                str = "Scenes/Loading_Scene";
                break;
        }
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
