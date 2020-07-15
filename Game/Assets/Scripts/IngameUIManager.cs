using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class IngameUIManager : MonoBehaviour
{
    private IngameMusicManager _ingameMusic;
    private GameManager _GM;
    private GameObject _configWindow;
        
    private Slider _progress;
    private Image _blackScreen;
    private Image _endScene;

    private bool isEnd = false;
    private bool isConfigOn = false;

    public float EndSceneOpenTime = 1.5f;
    public float TextEffectLiveTime = 0.6f;
    public GameObject TextEffect_Hit;
    public GameObject TextEffect_Dodge;
    public Transform TextEffectTransform_Hit;
    public Transform TextEffectTransform_Dodge;

    // Start is called before the first frame update
    void Start()
    {
        _ingameMusic = GameObject.Find("BGM").GetComponent<IngameMusicManager>();
        _progress = GameObject.Find("ProgressBar").GetComponent<Slider>();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _endScene = GameObject.Find("EndScene").GetComponent<Image>();
        _GM = GameObject.Find("Manager").GetComponent<GameManager>();
        _configWindow = GameObject.Find("Settings").transform.Find("ConfigWindow").gameObject;
        TextEffectTransform_Hit = GameObject.Find("TextEffect_Hit_Position").GetComponent<Transform>();
        TextEffectTransform_Dodge = GameObject.Find("TextEffect_Dodge_Position").GetComponent<Transform>();

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
        Debug.Log("ReturnToMainMenu");
    }

    public void PrintTextEffect(TextPrintType type)
    {
        switch (type)
        {
            case TextPrintType.Hit :
                StartCoroutine(TextEffect(TextEffectLiveTime, 
                    Instantiate(TextEffect_Hit, TextEffectTransform_Hit.position, TextEffectTransform_Hit.rotation.normalized, 
                        GameObject.Find("TextEffect").GetComponent<Transform>()),
                    type));
                break;
            case TextPrintType.Dodge :
                StartCoroutine(TextEffect(TextEffectLiveTime, 
                    Instantiate(TextEffect_Dodge, TextEffectTransform_Dodge.position, TextEffectTransform_Dodge.rotation.normalized, 
                        GameObject.Find("TextEffect").GetComponent<Transform>()),
                    type));
                break;
            default :
                break;
        }
    }

    IEnumerator TextEffect(float waitTime, GameObject obj, TextPrintType type)
    {
        Debug.Log("TextEffect");
        while (true)
        {
            switch (type)
            {
                case TextPrintType.Hit :
                    obj.transform.position = new Vector3(TextEffectTransform_Hit.position.x, TextEffectTransform_Hit.position.y + Time.deltaTime * 2.0f, 0);
                    break;
                case TextPrintType.Dodge :
                    obj.transform.position = new Vector3(TextEffectTransform_Dodge.position.x, TextEffectTransform_Dodge.position.y + Time.deltaTime * 2.0f, 0);
                    break;
                default :
                    break;
            }
            StartCoroutine(Timer(waitTime));
            yield break;
        }
        Destroy(obj);
        Debug.Log("End");
    }

    IEnumerator Timer(float waitTime)
    {
        Debug.Log("Timer");
        yield return new WaitForSeconds(waitTime);
    }
}
