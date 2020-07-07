using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private Vector2 touchStartPos;
    
    // Update is called once per frame
    void Update()
    {
        // gameObject.transform.Translate(Input.GetTouch(0).deltaPosition * Time.deltaTime * 1.0f);
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began :
                    // Debug.Log("Begin : " + touch.position);
                    touchStartPos = Input.GetTouch(0).deltaPosition;
                    break;
                case TouchPhase.Moved :
                    // if (Mathf.Abs(Input.GetTouch(0).deltaPosition.normalized.y) >= 0.93)
                    // {
                    //     gameObject.transform.Translate(Input.GetTouch(0).deltaPosition * Time.deltaTime * 1.0f);
                    // }
                    if (Mathf.Abs(Input.GetTouch(0).deltaPosition.y - touchStartPos.y) >= 5)
                    {
                        gameObject.transform.Translate(Input.GetTouch(0).deltaPosition * Time.deltaTime * 1.0f);
                    }
                    // Debug.Log("Moved : " + touch.position);
                    break;
                case TouchPhase.Ended :
                    // Debug.Log("Ended : " + touch.position);
                    break;
            }
        }
    }
}
