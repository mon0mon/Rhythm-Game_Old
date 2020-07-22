using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingame_Warnning_Indicator_Controller : MonoBehaviour
{
    public Sprite AttackSign;
    public Sprite DodgeSign;
    public float IndicatorLiveTime = 0.2f;

    private Image _image;

    private void Start()
    {
        _image = gameObject.GetComponent<Image>();
    }

    public void OnEnableAttackSign()
    {
        _image.sprite = AttackSign;
        _image.enabled = true;
        StartCoroutine(DisenableImage(IndicatorLiveTime));
    }

    public void OnEnableDodgeSign()
    {
        _image.sprite = DodgeSign;
        _image.enabled = true;
        StartCoroutine(DisenableImage(IndicatorLiveTime));
    }

    IEnumerator DisenableImage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _image.enabled = false;
    }
}
