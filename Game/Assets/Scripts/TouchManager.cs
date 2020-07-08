using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private Vector2 touchStartPos;
    private bool isSwiped = false;

    public SwipeType SwipeType;
    public float Vertical_Sensitivity = 0.6f;
    public float Horizontal_Sensitivity = 0.6f;
    
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
                    break;
                
                case TouchPhase.Moved:
                    switch (SwipeType)
                    {
                        case SwipeType.Vertical :
                            // Vertical Swipe Only
                            if (Mathf.Abs(Input.GetTouch(0).deltaPosition.y - touchStartPos.y) >= 5)
                            {
                                isSwiped = true;
                                Debug.Log("Moved : " + touch.position);
                                gameObject.transform.Translate(new Vector2(0, Input.GetTouch(0).deltaPosition.y) * Time.deltaTime * Vertical_Sensitivity);
                            }
                            break;
                        case SwipeType.Horizontal :
                            // Horizontal Swipe Only
                            if (Mathf.Abs(Input.GetTouch(0).deltaPosition.x - touchStartPos.x) >= 5)
                            {
                                isSwiped = true;
                                Debug.Log("Moved : " + touch.position);
                                gameObject.transform.Translate(new Vector2(Input.GetTouch(0).deltaPosition.x, 0) * Time.deltaTime * Horizontal_Sensitivity);
                            }
                            break;
                    }
                    break;
                
                case TouchPhase.Ended :
                    if (!isSwiped)
                    {
                        Debug.Log("Touch Tap Active");
                    }
                    else
                    {
                        Debug.Log("Swipe Active");
                        isSwiped = false;
                    }
                    // Debug.Log("Ended : " + touch.position);
                    break;
            }
        }
    }
}

public enum SwipeType
{
    Horizontal,
    Vertical
}
