﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    private static SceneData instance;

    public string str;
    public MenuAnimationState MenuAnimation = MenuAnimationState.Enabled;
    public TextEffectEnable TextEffect = TextEffectEnable.NULL;
    
    private int cnt = 0;
    private bool[] checkList;
    private bool isAnimationOn;

    private float BGM_Vol = -15;
    private float SFX_Vol = -15;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        
        // 바이너리로 된 설정 파일 불러오기
    }

    private void Initialize()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
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

    public void SaveBGMVol(float value)
    {
        BGM_Vol = value;
    }

    public float LoadBGMVol()
    {
        return BGM_Vol;
    }

    public void SaveSFXVol(float value)
    {
        SFX_Vol = value;
    }

    public float LoadSFXVol()
    {
        return SFX_Vol;
    }

    public enum MenuAnimationState
    {
        Enabled, Disenabled
    }
}
