using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TouchManager : MonoBehaviour
{
    private static TouchManager instance = null;
    private ButtonController btnController_Tap;
    private ButtonController btnController_Slide;
    
    private Vector2 touchStartPos;
    private bool isTap = false;
    private bool isSwiped = false;
    private int touchSide = 0;
    private bool trgTap = false;
    private bool trgSwip = false;
    public float time = 0;

    public TouchSpaceDivision TouchScreenDivision = TouchSpaceDivision.Single;
    public SwipeType SwipeType = SwipeType.NULL;
    public float Vertical_Sensitivity = 0.6f;
    public float Horizontal_Sensitivity = 0.6f;
    public float Free_Sensitivty = 0.6f;

    public GameObject btnTap;
    public GameObject btnSwipe;
    public GameObject btnTap_Right;
    public GameObject btnTap_Left;
    public GameObject btnSwipe_Right;
    public GameObject btnSwipe_Left;

    public ButtonPressed ButtonPressed;
    public bool DebugingOption = false;

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

        if (btnSwipe == null)
        {
            if (GameObject.Find("Button_Slide") != null)
            {
                btnSwipe = GameObject.Find("Button_Slide");
            }
        }
        
        isTap = false;
        isSwiped = false;
        trgTap = false;
        trgSwip = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        // 터치 조작 매커니즘
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = Input.GetTouch(0).deltaPosition;
                    isTap = true;
                    trgTap = true;
                    trgSwip = false;
                    // isTap = true;
                    // DisplayButtonPress();
                    time = Time.deltaTime;
                    break;

                case TouchPhase.Moved:
                    time += Time.deltaTime;

                    if (time >= 0.5f)
                    {
                        isTap = false;
                        trgTap = false;
                        isSwiped = false;
                        trgSwip = false;
                    }
                    else
                    {
                        switch (SwipeType)
                        {
                            case SwipeType.Vertical :
                                // Vertical Swipe Only
                                if (Mathf.Abs(Input.GetTouch(0).deltaPosition.y - touchStartPos.y) >= 3)
                                {
                                    trgSwip = true;
                                    isSwiped = true;
                                    isTap = false;
                                    trgTap = false;
                                    // DisplayButtonPress();
                                    // gameObject.transform.Translate(new Vector2(0, Input.GetTouch(0).deltaPosition.y) * Time.deltaTime * Vertical_Sensitivity);
                                }
                                break;
                            case SwipeType.Horizontal :
                                // Horizontal Swipe Only
                                if (Mathf.Abs(Input.GetTouch(0).deltaPosition.x - touchStartPos.x) >= 3)
                                {
                                    trgSwip = true;
                                    isSwiped = true;
                                    isTap = false;
                                    trgTap = false;
                                    // DisplayButtonPress();
                                    // gameObject.transform.Translate(new Vector2(Input.GetTouch(0).deltaPosition.x, 0) * Time.deltaTime * Horizontal_Sensitivity);
                                }
                                break;
                            case SwipeType.Free :
                                if (Mathf.Abs(Input.GetTouch(0).deltaPosition.y - touchStartPos.y) >= 3
                                    && Mathf.Abs(Input.GetTouch(0).deltaPosition.x - touchStartPos.x) >= 3) {
                                    trgSwip = true;
                                    isSwiped = true;
                                    isTap = false;
                                    trgTap = false;
                                    // DisplayButtonPress();
                                    // gameObject.transform.Translate(Input.GetTouch(0).deltaPosition * Time.deltaTime * Free_Sensitivty);
                                }
                                break;
                            default:
                                Debug.Log("Touch Manager : Swipe Type is NULL");
                                break;
                        }
                    }
                    break;
                
                case TouchPhase.Ended :
                    // 스와이프 트리커가 작동하지 않았을 때 (탭일 경우)
                    if (!trgSwip && trgTap)
                    {
                        // 터치 스크린의 좌우 구분 옵션이 켜있을 경우
                        if (TouchScreenDivision == TouchSpaceDivision.Double)
                        { 
                            ScreenTouchSideCalc(Input.GetTouch(0).position.x);
                            switch (TouchSide())
                            {
                                case 1:
                                    btnTap_Right.GetComponent<ButtonController>().ButtonPressedImage();
                                    ButtonPressed = ButtonPressed.Button_Tab_Right;
                                    if (DebugingOption) Debug.Log("ButtonPressed.Button_Tab_Right");
                                    break;
                                case -1:
                                    btnTap_Left.GetComponent<ButtonController>().ButtonPressedImage();
                                    ButtonPressed = ButtonPressed.Button_Tab_Left;
                                    if (DebugingOption) Debug.Log("ButtonPressed.Button_Tab_Left");
                                    break;
                            }
                        }
                        // 터치 스크린의 좌우 구분 옵션이 꺼져있을 경우
                        else
                        {
                            btnTap.GetComponent<ButtonController>().ButtonPressedImage();
                            ButtonPressed = ButtonPressed.Button_Tab;
                            if (DebugingOption) Debug.Log("ButtonPressed.Button_Tab");
                        }
                        
                        // DisplayButtonDefault();
                        isTap = false;
                        isSwiped = false;

                        trgTap = false;
                        trgSwip = false;
                    }
                    
                    // 스와이프 트리거가 작동 했을 경우 (스와이프일 경우)
                    else if (trgSwip && !trgTap)
                    {
                        // 터치 스크린의 좌우 구분 옵션이 켜있을 경우
                        if (TouchScreenDivision == TouchSpaceDivision.Double)
                        { 
                            ScreenTouchSideCalc(Input.GetTouch(0).position.x);
                            switch (TouchSide())
                            {
                                case 1:
                                    btnSwipe_Right.GetComponent<ButtonController>().ButtonPressedImage();
                                    ButtonPressed = ButtonPressed.Button_Swipe_Right;
                                    if (DebugingOption) Debug.Log("ButtonPressed.Button_Swipe_Right");
                                    break;
                                case -1:
                                    btnSwipe_Left.GetComponent<ButtonController>().ButtonPressedImage();
                                    ButtonPressed = ButtonPressed.Button_Swipe_Left;
                                    if (DebugingOption) Debug.Log("ButtonPressed.Button_Swipe_Left");
                                    break;
                            }
                        }
                        else
                        {
                            btnSwipe.GetComponent<ButtonController>().ButtonPressedImage();
                            ButtonPressed = ButtonPressed.Button_Swipe;
                            ResetButtonTapSprite();
                            if (DebugingOption) Debug.Log("ButtonPressed.Button_Swipe");
                        }
                        // DisplayButtonDefault();
                        isTap = false;
                        isSwiped = false;
                        
                        trgTap = false;
                        trgSwip = false;
                    }

                    time = 0;
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

    private void ScreenTouchSideCalc(float num)
    {
        if (num > Screen.width / 2)
        {
            touchSide = 1;    // 우측 화면 터치
            return;
        }
        else
        {
            touchSide = -1;    // 좌측 화면 터치
            return;
        }
    }

    // private void DisplayButtonPress()
    // {
    //     if (!isSwiped)
    //     {
    //         if (TouchScreenDivision == TouchSpaceDivision.Double)
    //         { 
    //             ScreenTouchSideCalc(Input.GetTouch(0).position.x);
    //             switch (TouchSide())
    //             {
    //                 case 1:
    //                     btnTap_Right.GetComponent<ButtonController>().ButtonPressedImage();
    //                     ButtonPressed = ButtonPressed.Button_Tab_Right;
    //                     Debug.Log("ButtonPressed.Button_Tab_Right");
    //                     break;
    //                 case -1:
    //                     btnTap_Left.GetComponent<ButtonController>().ButtonPressedImage();
    //                     ButtonPressed = ButtonPressed.Button_Tab_Left;
    //                     Debug.Log("ButtonPressed.Button_Tab_Left");
    //                     break;
    //             }
    //         }
    //         else
    //         {
    //             btnTap.GetComponent<ButtonController>().ButtonPressedImage();
    //             ButtonPressed = ButtonPressed.Button_Tab;
    //             Debug.Log("ButtonPressed.Button_Tab");
    //         }
    //     }
    //     else
    //     {
    //         if (TouchScreenDivision == TouchSpaceDivision.Double)
    //         { 
    //             ScreenTouchSideCalc(Input.GetTouch(0).position.x);
    //             switch (TouchSide())
    //             {
    //                 case 1:
    //                     btnSwipe_Right.GetComponent<ButtonController>().ButtonPressedImage();
    //                     ButtonPressed = ButtonPressed.Button_Swipe_Right;
    //                     Debug.Log("ButtonPressed.Button_Swipe_Right");
    //                     break;
    //                 case -1:
    //                     btnSwipe_Left.GetComponent<ButtonController>().ButtonPressedImage();
    //                     ButtonPressed = ButtonPressed.Button_Swipe_Left;
    //                     Debug.Log("ButtonPressed.Button_Swipe_Left");
    //                     break;
    //             }
    //         }
    //         else
    //         {
    //             btnSwipe.GetComponent<ButtonController>().ButtonPressedImage();
    //             ButtonPressed = ButtonPressed.Button_Swipe;
    //             ResetButtonTapSprite();
    //             Debug.Log("ButtonPressed.Button_Swipe");
    //         }
    //     }
    // }

    private void DisplayButtonDefault()
    {
        switch (ButtonPressed)
        {
            case ButtonPressed.Button_Tab:
                btnTap.GetComponent<ButtonController>().ButtonDefaultImage();
                break;
            case ButtonPressed.Button_Tab_Right:
                btnTap_Right.GetComponent<ButtonController>().ButtonDefaultImage();
                break;
            case ButtonPressed.Button_Tab_Left:
                btnTap_Left.GetComponent<ButtonController>().ButtonDefaultImage();
                break;
            case ButtonPressed.Button_Swipe:
                btnSwipe.GetComponent<ButtonController>().ButtonDefaultImage();
                break;
            case ButtonPressed.Button_Swipe_Right:
                btnSwipe_Right.GetComponent<ButtonController>().ButtonDefaultImage();
                break;
            case ButtonPressed.Button_Swipe_Left:
                btnSwipe_Left.GetComponent<ButtonController>().ButtonDefaultImage();
                break;
        }
        
        ButtonPressed = ButtonPressed.NULL;
    }

    private void ResetButtonTapSprite()
    {
        if (btnTap != null) btnTap.GetComponent<ButtonController>().ButtonDefaultImage();
        if (btnTap_Right != null) btnTap_Right.GetComponent<ButtonController>().ButtonDefaultImage();
        if (btnTap_Left != null) btnTap_Left.GetComponent<ButtonController>().ButtonDefaultImage();
    }
}

public enum ButtonPressed
{
    Button_Tab,
    Button_Swipe,
    Button_Tab_Right,
    Button_Tab_Left,
    Button_Swipe_Right,
    Button_Swipe_Left,
    NULL
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
