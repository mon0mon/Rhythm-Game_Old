using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private static TouchManager instance = null;
    private ButtonController btnController_Tap;
    private ButtonController btnController_Slide;
    
    private Vector2 touchStartPos;
    private bool isTap;
    private bool isSwiped = false;

    public SwipeType SwipeType;
    public float Vertical_Sensitivity = 0.6f;
    public float Horizontal_Sensitivity = 0.6f;

    public GameObject btnTap;
    public GameObject btnSlide;

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
                    // Debug.Log("Begin : " + touch.position);
                    touchStartPos = Input.GetTouch(0).deltaPosition;
                    btnTap.GetComponent<ButtonController>().ButtonPressedImage();
                    break;
                
                case TouchPhase.Moved:
                    switch (SwipeType)
                    {
                        case SwipeType.Vertical :
                            // Vertical Swipe Only
                            if (Mathf.Abs(Input.GetTouch(0).deltaPosition.y - touchStartPos.y) >= 5)
                            {
                                isSwiped = true;
                                btnSlide.GetComponent<ButtonController>().ButtonPressedImage();
                                gameObject.transform.Translate(new Vector2(0, Input.GetTouch(0).deltaPosition.y) * Time.deltaTime * Vertical_Sensitivity);
                            }
                            break;
                        case SwipeType.Horizontal :
                            // Horizontal Swipe Only
                            if (Mathf.Abs(Input.GetTouch(0).deltaPosition.x - touchStartPos.x) >= 5)
                            {
                                isSwiped = true;
                                btnSlide.GetComponent<ButtonController>().ButtonPressedImage();
                                gameObject.transform.Translate(new Vector2(Input.GetTouch(0).deltaPosition.x, 0) * Time.deltaTime * Horizontal_Sensitivity);
                            }
                            break;
                        default:
                            Debug.Log("Touch Manager : Swip Type is NULL");
                            break;
                    }
                    break;
                
                case TouchPhase.Ended :
                    if (!isSwiped)
                    {
                        btnTap.GetComponent<ButtonController>().ButtonDefaultImage();
                        Debug.Log("Touch Tap Active");
                    }
                    else
                    {
                        btnSlide.GetComponent<ButtonController>().ButtonDefaultImage();
                        Debug.Log("Swipe Active");
                        isSwiped = false;
                    }
                    break;
            }
        }
    }

    public bool CheckHit()
    {
        throw new System.NotImplementedException();
    }
}

public enum SwipeType
{
    Horizontal,
    Vertical,
    NULL
}
