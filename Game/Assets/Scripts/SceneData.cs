using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    private static SceneData instance;

    public string str;
    public MenuAnimationState MenuAnimation = MenuAnimationState.Enabled;
    
    private int cnt = 0;
    private bool[] checkList;
    private bool isAnimationOn;
    
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
        
        Initialize();
    }

    private void Initialize()
    {
        switch (MenuAnimation)
        {
            case MenuAnimationState.Enabled:
                isAnimationOn = true;
                break;
            case MenuAnimationState.Disenabled:
                isAnimationOn = false;
                break;
        }
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
    public void SetMenuAnimationState(bool check)
    {
        switch (check)
        {
            case true :
                MenuAnimation = MenuAnimationState.Enabled;
                GameObject.Find("AnimationManager").GetComponent<AnimationManager>().EnableMenuAnimation();
                break;
            case false :
                MenuAnimation = MenuAnimationState.Disenabled;
                GameObject.Find("AnimationManager").GetComponent<AnimationManager>().DienableMenuAnimation();
                break;
        }
        isAnimationOn = check;
    }

    public bool GetMenuAnimationState()
    {
        return isAnimationOn;
    }
    
    public enum MenuAnimationState
    {
        Enabled, Disenabled
    }
}
