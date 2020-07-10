using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer theSR;
    
    public Sprite defaultImage;
    public Sprite pressedImage;
    
    private void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
    }

    public void ButtonPressedImage()
    {
        theSR.sprite = pressedImage;
    }

    public void ButtonDefaultImage()
    {
        theSR.sprite = defaultImage;
    }
}
