using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    private IngameMusicManager _ingameMusic; 
        
    private Slider _progress;
    private Image _blackScreen;
    private Image _endScene;
    private GameObject _configWindow;

    private bool isEnd = false;

    public float EndSceneOpenTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        _ingameMusic = GameObject.Find("BGM").GetComponent<IngameMusicManager>();
        _progress = GameObject.Find("ProgressBar").GetComponent<Slider>();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _endScene = GameObject.Find("EndScene").GetComponent<Image>();
        _configWindow = GameObject.Find("ConfigWindow");

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
        _endScene.enabled = true;
    }

    public void EnableConfigWindow()
    {
        _blackScreen.enabled = true;
        _configWindow.SetActive(true);
    }
}
