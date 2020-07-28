using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingame_Warnning_Indicator_Controller : MonoBehaviour
{
    public float IndicatorLiveTime = 0.2f;

    private Image _image;
    private Animator _animator;

    private void Start()
    {
        _image = gameObject.GetComponent<Image>();
        if (gameObject.GetComponent<Animator>() != null)
        { 
            _animator = gameObject.GetComponent<Animator>();
        }
    }

    public void OnEnableSign()
    {
        _image.enabled = true;
        StartCoroutine(DisenableImage(IndicatorLiveTime));
    }

    public void PlayInitAnim()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("TrgInit");
        }
    }

    public void PlayHitAnim()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("TrgHit");
        }
    }

    IEnumerator DisenableImage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _image.enabled = false;
    }
}
