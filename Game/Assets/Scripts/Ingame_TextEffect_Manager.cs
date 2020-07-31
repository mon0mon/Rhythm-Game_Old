using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_TextEffect_Manager : MonoBehaviour
{
    public TextEffectEnable TextEffect;
    public GameObject TextEffect_Hit;
    public GameObject TextEffect_Dodge;
    public GameObject TextEffect_Miss;
    public GameObject TextEffect_Damaged;

    private bool isPaused = false;
    private Ingame_TextEffect_Controller _hitController;
    private Ingame_TextEffect_Controller _dodgeController;
    private Ingame_TextEffect_Controller _missController;
    private Ingame_TextEffect_Controller _damagedController;
    
    private void Start()
    {
        if (SceneData.Instance.TextEffect != TextEffectEnable.NULL && GameObject.Find("SaveData") != null) TextEffect = SceneData.Instance.TextEffect;
        
        _hitController = TextEffect_Hit.GetComponent<Ingame_TextEffect_Controller>();
        _dodgeController = TextEffect_Dodge.GetComponent<Ingame_TextEffect_Controller>();
        _missController = TextEffect_Miss.GetComponent<Ingame_TextEffect_Controller>();
        _damagedController = TextEffect_Damaged.GetComponent<Ingame_TextEffect_Controller>();

        StartCoroutine(LateStart());
    }

    public void PrintHitEffect()
    {
        _hitController.EnableTextEffect();
    }

    public void PrintDodgeEffect()
    {
        _dodgeController.EnableTextEffect();
    }

    public void PrintMissEffect()
    {
        _missController.EnableTextEffect();
    }
    
    public void PrintDamagedEffect()
    {
        _damagedController.EnableTextEffect();
    }

    public void ToggleStatusTextEffect()
    {
        isPaused = !isPaused;
        
        _hitController.SetTextEffectStatus(isPaused);
        _dodgeController.SetTextEffectStatus(isPaused);
        _missController.SetTextEffectStatus(isPaused);
        _damagedController.SetTextEffectStatus(isPaused);
    }

    IEnumerator LateStart()
    {
        yield return  new WaitForSeconds(0.01f);
        if (SceneData.Instance.TextEffect == TextEffectEnable.NULL)
        {
            TextEffect = TextEffectEnable.Enable;
            SceneData.Instance.TextEffect = TextEffect;
        }
        else
        {
            Debug.Log(SceneData.Instance.TextEffect);
            TextEffect = SceneData.Instance.TextEffect;
        }
    }
}

public enum TextEffectEnable
{
    Enable, Disenable, NULL
}
