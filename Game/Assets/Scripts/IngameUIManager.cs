using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Vector3 TextEffectObject_Hit = new Vector3(-5, (float)-3.5);
    public Vector3 TextEffectObject_Dodge = new Vector3(0, (float)-3.5);

    public float EndSceneOpenTime = 1.5f;
    public float TextEffectLiveTime = 0.6f;
    public GameObject TextEffect_Hit;
    public GameObject TextEffect_Dodge;

    // Start is called before the first frame update
    void Start()
    {
        _ingameMusic = GameObject.Find("BGM").GetComponent<IngameMusicManager>();
        _progress = GameObject.Find("ProgressBar").GetComponent<Slider>();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _endScene = GameObject.Find("EndScene").GetComponent<Image>();
        _GM = GameObject.Find("Manager").GetComponent<GameManager>();
        _configWindow = GameObject.Find("Settings").transform.Find("ConfigWindow").gameObject;

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
                StartCoroutine(TextEffect(TextEffectLiveTime, Instantiate(TextEffect_Hit)));
                break;
            case TextPrintType.Dodge :
                StartCoroutine(TextEffect(TextEffectLiveTime, Instantiate(TextEffect_Dodge)));
                break;
            default :
                break;
        }
    }

    IEnumerator TextEffect(float waitTime, GameObject obj)
    {
        float temp = 0.0f;

        while (temp < waitTime)
        {
            temp = Time.deltaTime;
            obj.transform.Translate(obj.transform.position.x, obj.transform.position.y + Time.deltaTime, obj.transform.position.z);
            yield return null;
        }
        Destroy(obj);
    }
}
