using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NoteObject : MonoBehaviour
{
    private GameObject SaveData;
    private GameManager _GM;
    private CircleCollider2D _collider2D;
    private GameObject PressedButton;
    private Ingame_Warnning_Indicator_Controller _player_Warnning_Controller;
    private Ingame_Warnning_Indicator_Controller _npc_Warnning_Controller;
    private TouchManager _touchManager;
    private IngameSFXManager _SFXManager;

    private bool isDeleted = false;
    private bool canBePressed;
    private bool detectExploitInput = false;
    private int checkCount = 0;
    private float checkTime = 0;
    
    public TouchInputType TouchInputType;
    public TouchPosition TouchPosition = TouchPosition.NULL;

    // Start is called before the first frame update
    void Start()
    {
        SaveData = GameObject.Find("SavaData");
        _GM = GameObject.Find("Manager").GetComponent<GameManager>();
        _player_Warnning_Controller = GameObject.Find("Player_Warnning_Indicator").GetComponent<Ingame_Warnning_Indicator_Controller>();
        _npc_Warnning_Controller = GameObject.Find("NPC_Warnning_Indicator").GetComponent<Ingame_Warnning_Indicator_Controller>();
        _touchManager = TouchManager.Instance;
        _SFXManager = GameObject.Find("SFX").GetComponent<IngameSFXManager>();
        
        detectExploitInput = false;
        checkCount = 0;
        checkTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (TouchInputType)
        {
            case TouchInputType.Tab:
                if (TouchManager.Instance.CheckHit())
                {
                    // 터치할 수 있는 화면 영역 설정이 없을 경우
                    if (TouchPosition == TouchPosition.NULL)
                    {
                        if (canBePressed)
                        {
                            DestroyImmediate(gameObject);
                            GameManager.Instance.NoteHit(TouchInputType);
                        }
                    }
                    // 터치할 수 있는 화면 영역 설징이 되어있을 경우
                    else
                    {
                        switch (TouchManager.Instance.TouchSide())
                        {
                            case -1 :
                                if (TouchPosition == TouchPosition.Left)
                                {
                                    if (canBePressed)
                                    {
                                        DestroyImmediate(gameObject);
                                        GameManager.Instance.NoteHit(TouchInputType);
                                        Debug.Log("NoteObject : TouchPosition _ Tab _ Left");
                                    }
                                }
                                break;
                            case 1 :
                                if (TouchPosition == TouchPosition.Right)
                                {
                                    if (canBePressed)
                                    {
                                        DestroyImmediate(gameObject);
                                        GameManager.Instance.NoteHit(TouchInputType);
                                        Debug.Log("NoteObject : TouchPosition _ Tab _ Right");
                                    }
                                }
                                break;
                            default :
                                Debug.Log("NoteObejct : Unexcpected Value");
                                break;
                        }
                    }
                }
                break;
            case TouchInputType.Swipe:
                if (TouchManager.Instance.CheckSwipe())
                {
                    // 터치할 수 있는 화면 영역 설정이 없을 경우
                    if (TouchPosition == TouchPosition.NULL)
                    {
                        if (canBePressed)
                        {
                            DestroyImmediate(gameObject);
                            GameManager.Instance.NoteHit(TouchInputType);
                        }
                    }
                    // 터치할 수 있는 화면 영역 설징이 되어있을 경우
                    else
                    {
                        switch (TouchManager.Instance.TouchSide())
                        {
                            case -1 :
                                if (TouchPosition == TouchPosition.Left)
                                {
                                    if (canBePressed)
                                    {
                                        DestroyImmediate(gameObject);
                                        GameManager.Instance.NoteHit(TouchInputType);
                                        Debug.Log("NoteObject : TouchPosition _ Swipe _ Left");
                                    }
                                }
                                break;
                            case 1 :
                                if (TouchPosition == TouchPosition.Right)
                                {
                                    if (canBePressed)
                                    {
                                        DestroyImmediate(gameObject);
                                        GameManager.Instance.NoteHit(TouchInputType);
                                        Debug.Log("NoteObject : TouchPosition _ Swipe _ Right");
                                    }
                                }
                                break;
                        }
                    }
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            // 마구잡이로 터치해서 악용하는 입력이 없을 경우 정상적으로 노트 입력 가능
            if (!detectExploitInput)
            {
                canBePressed = true;
            }
            GameManager.Instance.SetPressedButton(other.gameObject);
        } else if (other.CompareTag("DeadZone"))
        {
            Destroy(gameObject);
        } else if (other.CompareTag("Trigger"))
        {
            // 원시인 공격 준비 동작 및 맘모스 공격 준비 동작
            switch (other.name)
            {
                case "Trigger_Tap" :
                    _GM.GetComponent<Ingame_Charactor_Animation_Manager>().Actor_Player.SetAttackReady();
                    _player_Warnning_Controller.PlayInitAnim();
                    _SFXManager.PlayStoneAgeSFX(StoneAge_SFX.Babarian_Aim);
                    break;
                case "Trigger_Swipe" :
                    _GM.GetComponent<Ingame_Charactor_Animation_Manager>().Actor_NonPlayer.SetAttackReady();
                    _npc_Warnning_Controller.OnEnableSign();
                    break;
                default :
                    Debug.LogWarning("NoteObject - OnTriggerEnter2D - Trigger Handling : Uncategorized Exception");
                    break;
            }
        } 
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // 터치 악용 트리거가 감지 되었을 경우
        if (other.CompareTag("Dectect_Exploit_Input") && (_touchManager.CheckHit() || _touchManager.CheckSwipe()))
        {
            Debug.Log("IsChecking Exploit");
            // 사용자 입력 오차를 감안한 시간을 예외로 둠
            switch (TouchInputType)
            {
                case TouchInputType.Tab :
                    Debug.Log("checkCount : " + checkCount);
                    checkCount += 1;
                    if (checkCount >= 2)
                    {
                        Debug.Log("Exploit Found");
                        checkCount = 0;
                        StartCoroutine(DisenableInput(_GM.Touch_Exploit_Punish_Time));
                    }
                    break;
                case TouchInputType.Swipe :
                    checkTime += Time.deltaTime;
                    Debug.Log("checkTime : " + checkTime);
                    if (checkTime >= _GM.Touch_Exploit_tolerance_Time)
                    {
                        Debug.Log("Exploit Found");
                        StartCoroutine(DisenableInput(_GM.Touch_Exploit_Punish_Time));
                    }
                    break;
                default:
                    Debug.LogWarning("NoteObject - OnTriggerStay2D - TouchInputType Handling : Unexpected Value Exception");
                    break;
            }
            // 오차 감안 시간을 넘어갈 경우, 일정 시간동안 입력이 불가능하게 만듦
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator") && !isDeleted)
        {
            _GM.NoteMissed(TouchInputType);
            canBePressed = false;
            isDeleted = true;
            GameManager.Instance.SetPressedButton(null);
        } else if (other.CompareTag("Trigger"))
        {
            switch (other.name)
            {
                case "Trigger_Tap" :
                    // _player_Warnning_Controller.OnEnableSign();
                    break;
                case "Trigger_Swipe" :
                    _GM.GetComponent<Ingame_Charactor_Animation_Manager>().Actor_NonPlayer.SetDefault();
                    _GM.GetComponent<Ingame_Charactor_Animation_Manager>().Actor_NonPlayer.TriggerAttack();
                    break;
                default :
                    Debug.LogWarning("NoteObject - OnTriggerEnter2D - Trigger Handling : Uncategorized Exception");
                    break;
            }
        } else if (other.CompareTag("Dectect_Exploit_Input"))
        {
            checkCount = 0;
            checkTime = 0;
        }
    }

    IEnumerator DisenableInput(float waitTime)
    {
        detectExploitInput = true;
        yield return new WaitForSeconds(waitTime);
        detectExploitInput = false;
    }
}

public enum TouchPosition
{
    Right,
    Left,
    NULL
}

public enum TouchInputType 
{
    Tab,
    Swipe,
    NULL
}
