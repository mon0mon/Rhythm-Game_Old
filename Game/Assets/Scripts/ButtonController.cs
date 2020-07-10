using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Sprite defaultImage;
    public Sprite pressedImage;
    
    private void Start()
    {
        
    }

    public void ButtonPressedImage()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = pressedImage;
    }

    public void ButtonDefaultImage()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = defaultImage;
    }
}
