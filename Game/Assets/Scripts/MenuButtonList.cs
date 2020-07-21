using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuButtonList : MonoBehaviour
{
    private MenuManger _menuManger;
    private Image _background;
    private Button _gameStart;
    private Button _configBtn;
    private Button _screen;
    private GameObject _configWindow;
    private MusicManager _MainMenuMusic;
    
    private string selected_Scene;
    private bool isConfigOn = false;
    private bool init_check = false;

    public SceneList SelectedScene = SceneList.NULL;
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
        _gameStart.interactable = true;
        _menuManger.NextScene = MenuManger.SceneList.StoneAge;
        SelectedScene = SceneList.StoneAge;
        ChangeButtonSprite();
    }
    
    public void OnClick_MiddleAge()
    {
        _gameStart.interactable = true;
        _menuManger.NextScene = MenuManger.SceneList.MiddleAge;
        SelectedScene = SceneList.MiddleAge;
        ChangeButtonSprite();
    }
    
    public void OnClick_ModernAge()
    {
        _gameStart.interactable = true;
        _menuManger.NextScene = MenuManger.SceneList.ModernAge;
        SelectedScene = SceneList.ModernAge;
        ChangeButtonSprite();
    }
    
    public void OnClick_SciFi()
    {
        _gameStart.interactable = true;
        _menuManger.NextScene = MenuManger.SceneList.SciFi;
        SelectedScene = SceneList.SciFi;
        ChangeButtonSprite();
    }

    public void OnClick_GameStart()
    {
        _menuManger.MoveNextScene();
        if (SelectedScene != SceneList.NULL)
        {
            GameObject.Find("MainMenuMusic").GetComponent<MusicManager>().StopMainMenuMusic();
        }
    }

    public void OnClick_Config()
    {
        if (!isConfigOn)
        {
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
        }
        else
        {
            ResetColor();
            ResetButtonSprite();
            GameObject.Find("Config_Window").transform.GetChild(0).gameObject.SetActive(false);
            ActiveButtons();
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
        Application.Quit();
        Debug.Log("Exit");
    }

    public void OnClick_Credit()
    {
        Debug.Log("Credit");
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
    }
}
