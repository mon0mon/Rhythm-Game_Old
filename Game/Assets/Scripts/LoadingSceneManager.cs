using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class LoadingSceneManager : MonoBehaviour
{
    public static SceneList list;
    public SceneList scene = SceneList.RhythmGame_Test_PC;
    public bool AutoLoading = false;

    private static LoadingSceneManager instance = null;

    private AsyncOperation async;
    private string str;
    private bool canOpen = true;
    private Random rand = new Random();
    private int num;

    public enum SceneList
    {
        LoadingScene, RhythmGame_Test_PC, TouchSwipe_Test_Mobile
    }
        
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
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

        instance = this;
        list = scene;

    }

    public static LoadingSceneManager Instance => instance;

    // 로딩
    IEnumerator Load()
    {
        SelecteScene();
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
                yield return new WaitForSeconds(RandomNumber(5, 10));
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
        }
    }
}
