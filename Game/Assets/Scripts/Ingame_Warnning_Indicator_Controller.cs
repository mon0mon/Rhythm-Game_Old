using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingame_Warnning_Indicator_Controller : MonoBehaviour
{
    public float IndicatorLiveTime = 0.2f;

    private Image _image;

    private void Start()
    {
        _image = gameObject.GetComponent<Image>();
    }

    public void OnEnableSign()
    {
        _image.enabled = true;
        StartCoroutine(DisenableImage(IndicatorLiveTime));
    }

    IEnumerator DisenableImage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _image.enabled = false;
    }
}
