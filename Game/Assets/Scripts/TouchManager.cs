using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private static TouchManager instance = null;
    private ButtonController btnController_Tap;
    private ButtonController btnController_Slide;
    
    private Vector2 touchStartPos;
    private bool isTap = false;
    private bool isSwiped = false;
    private int touchSide;

    public TouchSpaceDivision TouchScreenDivision = TouchSpaceDivision.Single;
    public SwipeType SwipeType = SwipeType.NULL;
    public float Vertical_Sensitivity = 0.6f;
    public float Horizontal_Sensitivity = 0.6f;
    public float Free_Sensitivty = 0.6f;

    public GameObject btnTap;
    public GameObject btnSlide;
    public GameObject btnTap_Right;
    public GameObject btnTap_Left;
    public GameObject btnSwipe_Right;
    public GameObject btnSwipe_Left;

    void Start()
    {
        Initialize();
    }

    public static TouchManager Instance => instance;

    private void Initialize()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

        if (btnTap == null)
        {
            btnTap = GameObject.Find("Button_Tap");
        }

        if (btnSlide == null)
        {
            if (GameObject.Find("Button_Slide") != null)
            {
                btnSlide = GameObject.Find("Button_Slide");
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        // gameObject.transform.Translate(Input.GetTouch(0).deltaPosition * Time.deltaTime * 1.0f);
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = Input.GetTouch(0).deltaPosition;
                    btnTap.GetComponent<ButtonController>().ButtonPressedImage();
                    isTap = true;
                    break;
                
                case TouchPhase.Moved:
                    btnTap.GetComponent<ButtonController>().ButtonDefaultImage();
                    switch (SwipeType)
                    {
                        case SwipeType.Vertical :
                            // Vertical Swipe Only
                            if (Mathf.Abs(Input.GetTouch(0).deltaPosition.y - touchStartPos.y) >= 3)
                            {
                                isSwiped = true;
                                btnSlide.GetComponent<ButtonController>().ButtonPressedImage();
                                gameObject.transform.Translate(new Vector2(0, Input.GetTouch(0).deltaPosition.y) * Time.deltaTime * Vertical_Sensitivity);
                            }
                            break;
                        case SwipeType.Horizontal :
                            // Horizontal Swipe Only
                            if (Mathf.Abs(Input.GetTouch(0).deltaPosition.x - touchStartPos.x) >= 3)
                            {
                                isSwiped = true;
                                btnSlide.GetComponent<ButtonController>().ButtonPressedImage();
                                gameObject.transform.Translate(new Vector2(Input.GetTouch(0).deltaPosition.x, 0) * Time.deltaTime * Horizontal_Sensitivity);
                            }
                            break;
                        case SwipeType.Free :
                            if (Mathf.Abs(Input.GetTouch(0).deltaPosition.y - touchStartPos.y) >= 3
                                && Mathf.Abs(Input.GetTouch(0).deltaPosition.x - touchStartPos.x) >= 3) {
                                isSwiped = true;
                                btnSlide.GetComponent<ButtonController>().ButtonPressedImage();
                                gameObject.transform.Translate(Input.GetTouch(0).deltaPosition * Time.deltaTime * Free_Sensitivty);
                            }
                            break;
                        default:
                            Debug.Log("Touch Manager : Swipe Type is NULL");
                            break;
                    }
                    break;
                
                case TouchPhase.Ended :
                    if (!isSwiped)
                    {
                        btnTap.GetComponent<ButtonController>().ButtonDefaultImage();
                        Debug.Log("Touch Tap Active");
                        // isTap = true;
                        // StartCoroutine(Timer((float)0.15));
                        isTap = false;
                    }
                    else
                    {
                        btnSlide.GetComponent<ButtonController>().ButtonDefaultImage();
                        Debug.Log("Swipe Active");
                        isTap = false;
                        isSwiped = false;
                    }
                    
                    
                    if (Input.GetTouch(0).position.x > Screen.width / 2)
                    {
                        touchSide = 1;    // 우측 화면 터치
                    }
                    else
                    {
                        touchSide = 0;    // 좌측 화면 터치
                    }
                    break;
            }
        }
    }

    public bool CheckHit()
    {
        return isTap;
    }
    
    public bool CheckSwipe()
    {
        return isSwiped;
    }

    public int TouchSide()
    {
        return touchSide;
    }
}

public enum SwipeType
{
    Horizontal,
    Vertical,
    Free,
    NULL
}

public enum TouchSpaceDivision
{
    Single,
    Double
}
