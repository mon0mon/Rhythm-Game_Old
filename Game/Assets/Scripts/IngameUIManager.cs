﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class IngameUIManager : MonoBehaviour
{
    private GameObject _saveData;
    private IngameMusicManager _ingameMusic;
    private GameManager _GM;
    private GameObject _configWindow;
    private GameObject _endScene;
    private Ingame_TextEffect_Manager _textEffect;
    private IngameMusicManager _BGM;
    private IngameSFXManager _SFX;
    private Slider _BGM_Slider;
    private Slider _SFX_Slider;
        
    private Slider _progress;
    private Image _blackScreen;
    private Toggle _toggleTextEffect;

    private bool isEnd = false;
    private bool isConfigOn = false;
    private bool textEffectTrigger = true;
    private float temp;

    public float EndSceneOpenTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        _saveData = GameObject.Find("SavaData");
        _ingameMusic = GameObject.Find("BGM").GetComponent<IngameMusicManager>();
        _progress = GameObject.Find("ProgressBar").GetComponent<Slider>();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _endScene = GameObject.Find("Ending").transform.Find("EndScene").gameObject;
        _GM = GameObject.Find("Manager").GetComponent<GameManager>();
        _configWindow = GameObject.Find("Settings").transform.Find("ConfigWindow").gameObject;
        _textEffect = gameObject.GetComponent<Ingame_TextEffect_Manager>();
        _toggleTextEffect = _configWindow.transform.Find("TextEffect_Toggle").gameObject.GetComponent<Toggle>();
        _BGM = GameObject.Find("BGM").GetComponent<IngameMusicManager>();
        _SFX = GameObject.Find("SFX").GetComponent<IngameSFXManager>();
        
        _BGM_Slider = _configWindow.transform.Find("BGM_Slider").GetComponent<Slider>();
        _SFX_Slider = _configWindow.transform.Find("SFX_Slider").GetComponent<Slider>();
        
        _BGM_Slider.value = GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().GetBGMVol();
        _SFX_Slider.value = GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().GetSFXVol();

        _progress.minValue = 0.0f;
        _progress.maxValue = _ingameMusic.GetAudioLength();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnd)
        {
            if (_ingameMusic.GetPlayTime() <= _progress.maxValue - 0.1f)
            {
                _progress.value = _ingameMusic.GetPlayTime();
            }
            else
            {
                isEnd = true;
            }
        }
        else
        {
            _progress.value = _progress.maxValue;
        }
    }

    public void EnableEndScene()
    {
        _blackScreen.enabled = true;
        _endScene.SetActive(true);
    }

    public void EnableConfigWindow()
    {
        if (!isConfigOn)
        {
            _blackScreen.enabled = true;
            _configWindow.SetActive(true);
            _ingameMusic.PauseBGM();
            _GM.GamePause();
        }
        else
        {
            _blackScreen.enabled = false;
            _configWindow.SetActive(false);
            _ingameMusic.UnPauseBGM();
            _GM.GameUnPause();
        }

        _textEffect.ToggleStatusTextEffect();
        isConfigOn = !isConfigOn;
    }

    public void RestartLevel()
    {
        _GM.NextScene = SceneList.StoneAge;
        _GM.EnableLoadingScreen = false;
        _GM.MoveNextScene();
    }

    public void RetrunToMainMenu()
    {
        _GM.NextScene = SceneList.Main_Scene;
        _GM.EnableLoadingScreen = false;
        _GM.MoveNextScene();
    }

    public void PrintTextEffect(TextPrintType type)
    {
        switch (type)
        {
            case TextPrintType.Hit :
                _textEffect.PrintHitEffect();
                break;
            case TextPrintType.Dodge :
                _textEffect.PrintDodgeEffect();
                break;
            case TextPrintType.Miss :
                _textEffect.PrintMissEffect();
                break;
            case TextPrintType.Damaged :
                _textEffect.PrintDamagedEffect();
                break;
            default :
                break;
        }
    }

    public void OnToggleTextEffect()
    {
        textEffectTrigger = _toggleTextEffect.isOn;
        if (!textEffectTrigger)
        {
            _textEffect.ResetPosition();
        }
        _GM.ToggleTextEffect(textEffectTrigger);
    }
    
    public void OnBGMVolSlider()
    {
        _BGM.VolChangeBGM(_configWindow.transform.Find("BGM_Slider").GetComponent<Slider>().value);
    }

    public void OnSFXVolSlider()
    {
        _SFX.VolChangeSFX(_configWindow.transform.Find("SFX_Slider").GetComponent<Slider>().value);
    }
}
