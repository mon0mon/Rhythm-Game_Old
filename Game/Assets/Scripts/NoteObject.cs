using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NoteObject : MonoBehaviour
{
    private GameManager _GM;
    private CircleCollider2D _collider2D;
    private GameObject PressedButton;
    private Ingame_Warnning_Indicator_Controller _player_Warnning_Controller;
    private Ingame_Warnning_Indicator_Controller _npc_Warnning_Controller;
    
    private bool isDeleted = false;
    private bool canBePressed;
    
    public TouchInputType TouchInputType;
    public TouchPosition TouchPosition = TouchPosition.NULL;

    // Start is called before the first frame update
    void Start()
    {
        _GM = GameObject.Find("Manager").GetComponent<GameManager>();
        _player_Warnning_Controller = GameObject.Find("Player_Warnning_Indicator").GetComponent<Ingame_Warnning_Indicator_Controller>();
        _npc_Warnning_Controller = GameObject.Find("NPC_Warnning_Indicator").GetComponent<Ingame_Warnning_Indicator_Controller>();
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
            canBePressed = true;
            GameManager.Instance.SetPressedButton(other.gameObject);
        } else if (other.CompareTag("DeadZone"))
        {
            Debug.Log("Destroied");
            Destroy(gameObject);
        } else if (other.CompareTag("Trigger"))
        {
            // 원시인 공격 준비 동작 및 맘모스 공격 준비 동작
            switch (other.name)
            {
                case "Trigger_Tab" :
                    Debug.Log("Trigger_Tab");
                    _GM.GetComponent<Ingame_Charactor_Animation_Manager>().Actor_Player.SetAttackReady();
                    // _player_Warnning_Controller.OnEnableSign();
                    break;
                case "Trigger_Swipe" :
                    Debug.Log("Trigger_Swipe");
                    _GM.GetComponent<Ingame_Charactor_Animation_Manager>().Actor_NonPlayer.SetAttackReady();
                    _npc_Warnning_Controller.OnEnableSign();
                    break;
                default :
                    Debug.LogWarning("NoteObject - OnTriggerEnter2D - Trigger Handling : Uncategorized Exception");
                    break;
            }
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
                case "Trigger_Tab" :
                    _player_Warnning_Controller.OnEnableSign();
                    break;
                case "Trigger_Swipe" :
                    Debug.Log("Trigger_Swipe");
                    Debug.Log("Trigger_Swip TriggerAttack");
                    _GM.GetComponent<Ingame_Charactor_Animation_Manager>().Actor_NonPlayer.SetDefault();
                    _GM.GetComponent<Ingame_Charactor_Animation_Manager>().Actor_NonPlayer.TriggerAttack();
                    break;
                default :
                    Debug.LogWarning("NoteObject - OnTriggerEnter2D - Trigger Handling : Uncategorized Exception");
                    break;
            }
        } 
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
