using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonList : MonoBehaviour
{
    private MenuManger _menuManger;
    private Image _background;
    private string selected_Scene;

    public SceneList SelectedScene;
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
    }

    public void OnClick_StoneAge()
    {
        Debug.Log("OnClick_StoneAge");
        _menuManger.NextScene = MenuManger.SceneList.StoneAge;
        SelectedScene = SceneList.StoneAge;
        ChangeButtonSprite();
    }
    
    public void OnClick_MiddleAge()
    {
        Debug.Log("OnClick_MiddleAge");
        _menuManger.NextScene = MenuManger.SceneList.MiddleAge;
        SelectedScene = SceneList.MiddleAge;
        ChangeButtonSprite();
    }
    
    public void OnClick_ModernAge()
    {
        Debug.Log("OnClick_ModernAge");
        _menuManger.NextScene = MenuManger.SceneList.ModernAge;
        SelectedScene = SceneList.ModernAge;
        ChangeButtonSprite();
    }
    
    public void OnClick_SciFi()
    {
        Debug.Log("OnClick_SciFi");
        _menuManger.NextScene = MenuManger.SceneList.SciFi;
        SelectedScene = SceneList.SciFi;
        ChangeButtonSprite();
    }

    public void OnClick_GameStart()
    {
        Debug.Log("OnClick_GameStart");
        _menuManger.MoveNextScene();
    }

    private void ChangeButtonSprite()
    {
        ResetButtonSprite();
        // _background.color = new Color(176, 176, 176);
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
}
