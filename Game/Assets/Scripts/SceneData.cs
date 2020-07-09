using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    private static SceneData instance;

    private string str;
    private int cnt = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static SceneData Instance => instance;

    public string GetNextSceneName()
    {
        return str;
    }
    
    public void SetNextSceneName(string temp)
    {
        str = temp;
    }

    public bool CheckIsThisSceneNext()
    {
        cnt++;

        if (cnt % 2 == 0)
        {
            return true;
        }

        return false;
    }
}
