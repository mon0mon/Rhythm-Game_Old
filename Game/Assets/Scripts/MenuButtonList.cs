using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonList : MonoBehaviour
{
    private MenuManger _menuManger;
    private Image _background;
    private Button _gameStart;
    private Button _configBtn;
    private Button _screen;
    private Image _configWindow;
    
    private string selected_Scene;
    private bool isConfigOn = false;

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
            _configWindow = GameObject.Find("Config_Window").GetComponent<Image>();
        }

        if (GameObject.Find("Screen") != null)
        {
            _screen = GameObject.Find("Screen").GetComponent<Button>();
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
    }

    public void OnClick_Config()
    {
        if (!isConfigOn)
        {
            SetButtonColorDeactive();
            _configWindow.enabled = true;
            DeactiveButtons();
            isConfigOn = true;
        }
        else
        {
            ResetColor();
            _configWindow.enabled = false;
            isConfigOn = false;
            ActiveButtons();
        }
    }

    public void OnClick_Screen()
    {
        Debug.Log("OnClick_Screen");
        ResetColor();
        ResetButtonSprite();
        _configWindow.enabled = false;
        SelectedScene = SceneList.NULL;
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
        _background.color = Color.gray;
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
