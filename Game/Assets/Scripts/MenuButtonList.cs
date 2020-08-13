using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Main_Scene에서 사용하는 스크립트
public class MenuButtonList : MonoBehaviour
{
    private MenuManger _menuManger;
    private Image _background;
    private Button _gameStart;
    private Button _configBtn;
    private Button _screen;
    private GameObject _configWindow;
    private MusicManager _MainMenuMusic;
    private GameObject ButtonCheckImage;
    private GameObject Lock_StoneAge;
    private GameObject Lock_MiddleAge;
    private GameObject Lock_ModernAge;
    private GameObject Lock_SciFi;
    
    private string selected_Scene;
    private bool isConfigOn = false;
    private bool init_check = false;
    private ButtonSelected _selectedButton = ButtonSelected.NULL;
    private SFXManager _MenuSFX;

    public SceneList SelectedScene = SceneList.NULL;
    public bool StoneAge_Enable = true;
    public bool MiddleAge_Enable = true;
    public bool ModernAge_Enable = true;
    public bool SciFi_Enable = true;
    public Sprite Default_Barbarian;
    public Sprite Default_Knight;
    public Sprite Default_Worker;
    public Sprite Default_Spaceship;
    public Sprite Highlight_Barbarian;
    public Sprite Highlight_Knight;
    public Sprite Highlight_Worker;
    public Sprite Highlight_Spaceship;
    
    public enum SceneList
    {
        // 게임 플레이 스테이지
        StoneAge, MiddleAge, ModernAge, SciFi,
        // 예외처리
        NULL
    }

    // Start is called before the first frame update
    void Start()
    {
        _menuManger = GameObject.Find("Menu_Manager").GetComponent<MenuManger>();
        if (GameObject.Find("Background") != null)
        {
            _background = GameObject.Find("Background").GetComponent<Image>();
        }

        if (GameObject.Find("Game_Start") != null)
        {
            _gameStart = GameObject.Find("Game_Start").GetComponent<Button>();
            _gameStart.interactable = false;
        }

        if (GameObject.Find("ConfigButton") != null)
        {
            _configBtn = GameObject.Find("ConfigButton").GetComponent<Button>();
        }

        if (GameObject.Find("Config_Window") != null)
        {
            _configWindow = GameObject.Find("Config_Window");
        }

        if (GameObject.Find("Screen") != null)
        {
            _screen = GameObject.Find("Screen").GetComponent<Button>();
        }
        
        _selectedButton = ButtonSelected.NULL;
        if (GameObject.Find("Config_Window") != null)
        {
            ButtonCheckImage = GameObject.Find("Config_Window").transform.Find("Check_Image").gameObject;
        }

        if (GameObject.Find("MainMenuSFX").GetComponent<SFXManager>() != null)
        {
            _MenuSFX = GameObject.Find("MainMenuSFX").GetComponent<SFXManager>();
        }

        // 메인 화면인지 체크하고 변수 초기화
        if (GameObject.Find("Stone_Age") != null || GameObject.Find("Middle_Age") != null ||
            GameObject.Find("Modern_Age") != null || GameObject.Find("Sci-Fi_Age") != null)
        {
            if (GameObject.Find("Stone_Age").transform.Find("Lock_StoneAge") != null)
            {
                Lock_StoneAge = GameObject.Find("Stone_Age").transform.Find("Lock_StoneAge").gameObject;
                if (!StoneAge_Enable)
                {
                    Lock_StoneAge.SetActive(true);
                }
            }
        
            if (GameObject.Find("Middle_Age").transform.Find("Lock_MiddleAge") != null)
            {
                Lock_MiddleAge = GameObject.Find("Middle_Age").transform.Find("Lock_MiddleAge").gameObject;
                if (!MiddleAge_Enable)
                {
                    Lock_MiddleAge.SetActive(true);
                }
            }
        
            if (GameObject.Find("Modern_Age").transform.Find("Lock_ModernAge") != null)
            {
                Lock_ModernAge = GameObject.Find("Modern_Age").transform.Find("Lock_ModernAge").gameObject;
                if (!ModernAge_Enable)
                {
                    Lock_ModernAge.SetActive(true);
                }
            }
        
            if (GameObject.Find("Sci-Fi_Age").transform.Find("Lock_SciFi") != null)
            {
                Lock_SciFi = GameObject.Find("Sci-Fi_Age").transform.Find("Lock_SciFi").gameObject;
                if (!SciFi_Enable)
                {
                    Lock_SciFi.SetActive(true);
                }
            }
        }
    }

    void Update()
    {
        if (SelectedScene == SceneList.NULL)
        {
            _gameStart.interactable = false;
        }
    }

    public void OnClick_StoneAge()
    {
        if (StoneAge_Enable)
        {
            _gameStart.interactable = true;
            _menuManger.NextScene = MenuManger.SceneList.StoneAge;
            SelectedScene = SceneList.StoneAge;
            ChangeButtonSprite();
            _MenuSFX.PlayButtonClickSFX();
        }
    }
    
    public void OnClick_MiddleAge()
    {
        if (MiddleAge_Enable)
        {
            _gameStart.interactable = true;
            _menuManger.NextScene = MenuManger.SceneList.MiddleAge;
            SelectedScene = SceneList.MiddleAge;
            ChangeButtonSprite();
            _MenuSFX.PlayButtonClickSFX();
        }
    }
    
    public void OnClick_ModernAge()
    {
        if (ModernAge_Enable)
        {
            _gameStart.interactable = true;
            _menuManger.NextScene = MenuManger.SceneList.ModernAge;
            SelectedScene = SceneList.ModernAge;
            ChangeButtonSprite();
            _MenuSFX.PlayButtonClickSFX();
        }
    }
    
    public void OnClick_SciFi()
    {
        if (SciFi_Enable)
        {
            _gameStart.interactable = true;
            _menuManger.NextScene = MenuManger.SceneList.SciFi;
            SelectedScene = SceneList.SciFi;
            ChangeButtonSprite();
            _MenuSFX.PlayButtonClickSFX();
        }
    }

    // Start_Scene에서 사용하는 버튼
    public void OnClick_Start()
    {
        GameObject.Find("Start").GetComponent<Button>().interactable = false;
        _menuManger.MoveNextScene();
    }

    // Main_Scene에서 사용하는 버튼
    public void OnClick_GameStart()
    {
        if (_selectedButton == ButtonSelected.OnClickGameStart)
        {
            GameObject.Find("Game_Start").GetComponent<Button>().interactable = false;
            _menuManger.MoveNextScene();
            _MenuSFX.PlayLoadingSFX();
            if (SelectedScene != SceneList.NULL)
            {
                GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().StopMainMenuMusic();
            }
            return;
        }
        else
        {
            _selectedButton = ButtonSelected.OnClickGameStart;
            _MenuSFX.PlayButtonClickSFX();
            ButtonCheckImage.transform.localPosition = new Vector3(0.0f, -370.0f, 0.0f);
            ButtonCheckImage.SetActive(true);
        }
    }

    public void OnClick_Config()
    {
        if (!isConfigOn)
        {
            _MenuSFX.PlayLoadingSFX();
            SetButtonColorDeactive(); 
            GameObject.Find("Config_Window").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Background_Animation_Toggle").GetComponent<Toggle>().isOn = GameObject.Find("SaveData")
                .transform.GetComponentInChildren<AnimationManager>().isMenuAnimationOn;
            if (!init_check)
            {
                GameObject.Find("BGM_Slider").GetComponent<Slider>().value = SceneData.Instance.LoadBGMVol();
                GameObject.Find("SFX_Slider").GetComponent<Slider>().value = SceneData.Instance.LoadSFXVol();
                init_check = true;
            }
            DeactiveButtons();
            ButtonCheckImage.SetActive(false);
            _selectedButton = ButtonSelected.NULL;
        }
        else
        {
            _MenuSFX.PlayLoadingSFX();
            ResetColor();
            ResetButtonSprite();
            ButtonCheckImage.SetActive(false);
            GameObject.Find("Config_Window").transform.GetChild(0).gameObject.SetActive(false);
            ActiveButtons();
            ButtonCheckImage.SetActive(false);
            _selectedButton = ButtonSelected.NULL;
        }
        isConfigOn = !isConfigOn;
    }

    public void OnClick_Screen()
    {
        ResetColor();
        ResetButtonSprite();
        if (isConfigOn)
        {
            OnClick_Config();
        }
        SelectedScene = SceneList.NULL;
        
        // 선택된 체크 이미지 삭제
        ButtonCheckImage.SetActive(false);
        _selectedButton = ButtonSelected.NULL;
    }

    public void OnBGMVolSlider()
    {
        GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().VolChangeBGM(GameObject.Find("BGM_Slider").GetComponent<Slider>().value);
    }

    public void OnSFXVolSlider()
    {
        GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().VolChangeSFX(GameObject.Find("SFX_Slider").GetComponent<Slider>().value);
    }

    public void OnClick_Exit()
    {
        if (_selectedButton == ButtonSelected.OnClickExit)
        {
            Debug.Log("Exit");
            Application.Quit();
            _MenuSFX.PlayLoadingSFX();
            return;
        }
        else
        {
            _selectedButton = ButtonSelected.OnClickExit;
            ButtonCheckImage.transform.position = GameObject.Find("GameExit").transform.position;
            ButtonCheckImage.SetActive(true);
            _MenuSFX.PlayButtonClickSFX();
            return;
        }
    }
    
    public void OnClick_Credit()
    {
        if (_selectedButton == ButtonSelected.OnClickCredit)
        {
            Debug.Log("Credit");
            return;
        } 
        else
        {
            _selectedButton = ButtonSelected.OnClickCredit;
            ButtonCheckImage.transform.position = GameObject.Find("Credit").transform.position;
            ButtonCheckImage.SetActive(true);
            _MenuSFX.PlayButtonClickSFX();
            return;
        }
    }

    public void OnToggle_Animation()
    {
        _menuManger.MenuAnimation(GameObject.Find("Background_Animation_Toggle").GetComponent<Toggle>().isOn);
    }

    private void ChangeButtonSprite()
    {
        ResetButtonSprite();
        _background.color = Color.gray;
        switch (SelectedScene)
        {
            case SceneList.StoneAge :
                GameObject.Find("Stone_Age").GetComponent<Image>().sprite
                    = Highlight_Barbarian;
                break;
            case SceneList.MiddleAge :
                GameObject.Find("Middle_Age").GetComponent<Image>().sprite
                    = Highlight_Knight;
                break;
            case SceneList.ModernAge :
                GameObject.Find("Modern_Age").GetComponent<Image>().sprite
                    = Highlight_Worker;
                break;
            case SceneList.SciFi :
                GameObject.Find("Sci-Fi_Age").GetComponent<Image>().sprite
                    = Highlight_Spaceship;
                break;
            default:
                Debug.Log("MenuButtonList : Unexcepted Value");
                break;
        }
    }

    private void SetButtonColorDeactive()
    {
        GameObject.Find("Background").GetComponent<Image>().color = Color.gray;
        _gameStart.GetComponent<Image>().color = Color.gray;
    }

    private void ResetColor()
    {
        _background.color = Color.white;
        _gameStart.GetComponent<Image>().color = Color.white;
        _configBtn.GetComponent<Image>().color = Color.white;
    }

    private void ResetButtonSprite()
    {
        GameObject.Find("Stone_Age").GetComponent<Image>().sprite
            = Default_Barbarian;
        GameObject.Find("Middle_Age").GetComponent<Image>().sprite
            = Default_Knight;
        GameObject.Find("Modern_Age").GetComponent<Image>().sprite
            = Default_Worker;
        GameObject.Find("Sci-Fi_Age").GetComponent<Image>().sprite
            = Default_Spaceship;
    }

    private void DeactiveButtons()
    {
        GameObject.Find("Stone_Age").GetComponent<Button>().interactable = false;
        GameObject.Find("Middle_Age").GetComponent<Button>().interactable = false;
        GameObject.Find("Modern_Age").GetComponent<Button>().interactable = false;
        GameObject.Find("Sci-Fi_Age").GetComponent<Button>().interactable = false;
    }
    
    private void ActiveButtons()
    {
        GameObject.Find("Stone_Age").GetComponent<Button>().interactable = true;
        GameObject.Find("Middle_Age").GetComponent<Button>().interactable = true;
        GameObject.Find("Modern_Age").GetComponent<Button>().interactable = true;
        GameObject.Find("Sci-Fi_Age").GetComponent<Button>().interactable = true;
        _selectedButton = ButtonSelected.NULL;
        ButtonCheckImage.SetActive(false);
    }
}

public enum ButtonSelected
{
    OnClickCredit, OnClickExit, OnClickRestart, OnClickMainMenu, OnClickGameStart,
    NULL
}
