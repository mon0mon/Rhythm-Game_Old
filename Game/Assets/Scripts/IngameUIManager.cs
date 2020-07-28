using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    private Slider _Score_Indicator;
    private Text _SongInfo;
        
    private Slider _progress;
    private Image _blackScreen;
    private Toggle _toggleTextEffect;
    private Dropdown _timeDropDown;
    private GameObject ButtonCheckImage;

    private bool isEnd = false;
    private bool isConfigOn = false;
    private bool textEffectTrigger = true;
    private float temp;
    private bool btnTriggerOn;

    private ResultState resultState;
    private string bossStatus;
    private float score;
    private float clearPercentage;
    private bool isEnableBackground;
    private ButtonSelected _selectedButton = ButtonSelected.NULL;

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
        _Score_Indicator = GameObject.Find("Boss_HP_Indicator").GetComponent<Slider>();

        _BGM_Slider = _configWindow.transform.Find("BGM_Slider").GetComponent<Slider>();
        _SFX_Slider = _configWindow.transform.Find("SFX_Slider").GetComponent<Slider>();
        
        _BGM_Slider.value = GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().GetBGMVol();
        _SFX_Slider.value = GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().GetSFXVol();

        _SongInfo = GameObject.Find("Song_Info").GetComponent<Text>();

        _timeDropDown = _configWindow.transform.Find("Time").gameObject.GetComponent<Dropdown>();
        _timeDropDown.value = 3;

        _progress.minValue = 0.0f;
        _progress.maxValue = _ingameMusic.GetAudioLength();
        
        _Score_Indicator.maxValue = 200;
        _Score_Indicator.minValue = 0;
        _Score_Indicator.value = 100;

        if (_configWindow != null)
        {
            ButtonCheckImage = _configWindow.transform.Find("Check_Image").gameObject;
        }

        if (SceneData.Instance.TextEffect != TextEffectEnable.NULL)
        {
            switch (SceneData.Instance.TextEffect)
            {
                case TextEffectEnable.Enable :
                    _toggleTextEffect.isOn = true;
                    break;
                case TextEffectEnable.Disenable :
                    _toggleTextEffect.isOn = false;
                    break;
            }
        }
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

        // ResultState 상태에 따라 이미지 변경
        _endScene.transform.Find("ResultDisplay").GetComponent<Text>().text = bossStatus;
        _endScene.transform.Find("Clear_Percentage_Display").GetComponent<Text>().text = "Clear : " + clearPercentage.ToString() + "%";
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
            ButtonCheckImage.SetActive(false);
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
        if (!btnTriggerOn)
        {
            if (_selectedButton == ButtonSelected.OnClickRestart)
            {
                Debug.Log("Restart");
                _GM.NextScene = SceneList.StoneAge;
                _GM.EnableLoadingScreen = false;
                _GM.MoveNextScene();
                btnTriggerOn = true;
            } 
            else
            {
                _selectedButton = ButtonSelected.OnClickRestart;
                ButtonCheckImage.transform.position = GameObject.Find("Restart").transform.position;
                ButtonCheckImage.SetActive(true);
            }
        }
    }

    public void RetrunToMainMenu()
    {
        if (!btnTriggerOn)
        {
            if (_selectedButton == ButtonSelected.OnClickMainMenu)
            {
                Debug.Log("Restart");
                _GM.NextScene = SceneList.Main_Scene;
                _GM.EnableLoadingScreen = false;
                _GM.MoveNextScene();
                btnTriggerOn = true;
            } 
            else
            {
                _selectedButton = ButtonSelected.OnClickMainMenu;
                ButtonCheckImage.transform.position = GameObject.Find("ToMainMenu").transform.position;
                ButtonCheckImage.SetActive(true);
            }
        }
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
        switch (textEffectTrigger)
        {
            case true :
                _textEffect.TextEffect = TextEffectEnable.Enable;
                break;
            case false :
                _textEffect.TextEffect = TextEffectEnable.Disenable;
                break;
        }
    }
    
    public void OnBGMVolSlider()
    {
        _BGM.VolChangeBGM(_configWindow.transform.Find("BGM_Slider").GetComponent<Slider>().value);
    }

    public void OnSFXVolSlider()
    {
        _SFX.VolChangeSFX(_configWindow.transform.Find("SFX_Slider").GetComponent<Slider>().value);
    }

    public void GetGameResult(ResultState state, string bossStatus, float score, float clearPercentage)
    {
        this.resultState = state;
        this.bossStatus = bossStatus;
        this.score = score;
        this.clearPercentage = clearPercentage;
    }

    public void OnBossHPChageListener()
    {
        _Score_Indicator.value = _GM.score;
    }

    public void OnToggleBackgroundImg()
    {
        isEnableBackground = GameObject.Find("Background_Toggle").GetComponent<Toggle>().isOn;
        GameObject.Find("BaseCanvas").GetComponent<Image>().enabled = isEnableBackground;
    }

    public void SetSongInfo(string str)
    {
        _SongInfo.text = str;
    }

    public void OnChangeTimeListener()
    {
        _timeDropDown.onValueChanged.AddListener(delegate { DropDownValueChanged(_timeDropDown); });
    }

    void DropDownValueChanged(Dropdown change)
    {
        float time = 1f;
        
        switch (change.value)
        {
            case 0 :
                _GM.TimeScale = 0f;
                time = 0f;
                _ingameMusic.AudioMixer.SetFloat("BGM_Speed", 0f);
                break;
            case 1 :
                _GM.TimeScale = 0.25f;
                time = 0.25f;
                _ingameMusic.AudioMixer.SetFloat("BGM_Speed", 0.25f);
                break;
            case 2 :
                _GM.TimeScale = 0.5f;
                time = 0.5f;
                _ingameMusic.AudioMixer.SetFloat("BGM_Speed", 0.5f);
                break;
            case 3 :
                _GM.TimeScale = 1f;
                time = 1f;
                _ingameMusic.AudioMixer.SetFloat("BGM_Speed", 1f);
                break;
            case 4 :
                _GM.TimeScale = 2f;
                time = 2f;
                _ingameMusic.AudioMixer.SetFloat("BGM_Speed", 2f);
                break;
            case 5 :
                _GM.TimeScale = 3f;
                time = 3f;
                _ingameMusic.AudioMixer.SetFloat("BGM_Speed", 3f);
                break;
        }

        Time.timeScale = time;
    }
}
