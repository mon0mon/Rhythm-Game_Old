using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Sprite defaultImage;
    public Sprite pressedImage;
    public Sprite TextHit;
    public Sprite TextDodge;
    public TextPrintType TextType;
    public TouchInputType InputType;

    private GameObject _Manager;

    private void Start()
    {
        _Manager = GameObject.Find("Manager");
    }

    public void ButtonPressedImage()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = pressedImage;
    }

    public void ButtonDefaultImage()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = defaultImage;
    }
    
    public void SelectTextType()
    {
        switch (TextType)
        {
            case TextPrintType.Hit :
                _Manager.GetComponent<IngameUIManager>().PrintTextEffect(TextPrintType.Hit);
                break;
            case TextPrintType.Dodge :
                _Manager.GetComponent<IngameUIManager>().PrintTextEffect(TextPrintType.Dodge);
                break;
            case TextPrintType.NULL :
                break;
        }
    }
    
    public void SelectTextType(TextPrintType tp)
    {
        switch (tp)
        {
            case TextPrintType.Miss :
                _Manager.GetComponent<IngameUIManager>().PrintTextEffect(TextPrintType.Miss);
                break;
            case TextPrintType.Damaged :
                _Manager.GetComponent<IngameUIManager>().PrintTextEffect(TextPrintType.Damaged);
                break;
            case TextPrintType.NULL :
                break;
        }
    }
}

public enum NoteHitType
{
    Hit, NotHit
}

public enum TextPrintType
{
    Hit, 
    Dodge,
    Miss,
    Damaged,
    NULL
}
