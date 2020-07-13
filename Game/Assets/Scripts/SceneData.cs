using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    private static SceneData instance;

    public string str;
    public bool isAnimationOn = true;
    
    private int cnt = 0;
    private int num = 0;
    private bool[] checkList;
    
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

    public void ClearSceneInfo()
    {
        str = null;
    }

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
    
    // 메뉴 애니메이션 설정
    public void SetMenuAnimation(bool check)
    {
        if (check)
        {
            checkList = new bool[2];
            bool test = true;

            if (GameObject.Find("Bird") != null)
            {
                GameObject.Find("Bird").GetComponent<Animator>().enabled = true;
                checkList[num] = false;
            }

            if (GameObject.Find("Title") != null)
            {
                num++;
                GameObject.Find("Title").GetComponent<Animator>().enabled = true;
                checkList[num] = false;
            }

            for (int i = 0; i < checkList.Length; i++)
            {
                if (checkList[i] != false)
                {
                    test = false;
                }
            }

            if (test == false)
            {
                return;
            }
            else
            {
                isAnimationOn = check;
            }
        }
        else
        {
            checkList = new bool[2];
            bool test = true;

            if (GameObject.Find("Bird") != null)
            {
                GameObject.Find("Bird").GetComponent<Animator>().enabled = false;
                checkList[num] = false;
            }

            if (GameObject.Find("Title") != null)
            {
                num++;
                GameObject.Find("Title").GetComponent<Animator>().enabled = false;
                checkList[num] = false;
            }

            for (int i = 0; i < checkList.Length; i++)
            {
                if (checkList[i] != false)
                {
                    test = false;
                }
            }

            if (test == false)
            {
                return;
            }
            else
            {
                isAnimationOn = check;
            }
        }
    }
}
