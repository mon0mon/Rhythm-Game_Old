using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NoteObject : MonoBehaviour
{
    private GameManager _gameManager;
    
    private bool isDeleted = false;
    private bool canBePressed;
    
    public TouchInputType TouchInputType;
    public TouchPosition TouchPosition = TouchPosition.NULL;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
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
                            GameManager.Instance.NoteHit();
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
                                        GameManager.Instance.NoteHit();
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
                                        GameManager.Instance.NoteHit();
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
                            GameManager.Instance.NoteHit();
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
                                        GameManager.Instance.NoteHit();
                                        GameManager.Instance.NoteHit();
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
                                        GameManager.Instance.NoteHit();
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
        if (other.tag == "Activator")
        {
            canBePressed = true;
        } else if (other.CompareTag("DeadZone"))
        {
            Debug.Log("Destroied");
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator") && !isDeleted)
        {
            _gameManager.NoteMissed();
            canBePressed = false;
            isDeleted = true;
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
    Swipe
}
