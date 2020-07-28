using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_TextEffect_Controller : MonoBehaviour
{
    public TextEffectType Type;

    private Ingame_TextEffect_Manager _textEffectManager;
    private Transform _pivot;
    private Animator _animator;
    
    private bool isEnable = false;
    private bool isPaused;
    private float initTime;
    private float lifeTime;
    private float speed = 2.0f;
    private bool isAnimPlay = false;

    void Start()
    {
        _textEffectManager = GameObject.Find("Manager").GetComponent<Ingame_TextEffect_Manager>();

        speed = _textEffectManager.GetTextEffectSpeed();
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (isEnable && lifeTime <= initTime)
            {
                initTime += Time.deltaTime;
                Debug.Log("Left Time : " + (lifeTime - initTime));
                
                if (!isAnimPlay)
                {
                    _animator.SetTrigger("TrgAction");
                    // switch (Type)
                    // {
                    //     case TextEffectType.TextEffect_Hit :
                    //         _animator.SetTrigger("TrgHit");
                    //         break;
                    //     case TextEffectType.TextEffect_Dodge :
                    //         _animator.SetTrigger("TrgDodge");
                    //         break;
                    //     case TextEffectType.TextEffect_Miss :
                    //         _animator.SetTrigger("TrgMiss");
                    //         break;
                    //     case TextEffectType.TextEffect_Damaged :
                    //         _animator.SetTrigger("TrgDamaged");
                    //         break;
                    //     default :
                    //         Debug.Log("Ingame_TextEffect_Controller - Updata - Is Animation Switch : Unexpected Value Excpetion");
                    //         break;
                    // }
                    isAnimPlay = true;
                }
            }
            else
            {
                ResetTextEffect();
            }
        }
    }

    public void EnableTextEffect(float activeTime)
    {
        if (!isEnable)
        {
            InitPosition();
            isEnable = true;
            initTime = Time.deltaTime;
            lifeTime = initTime + activeTime;
        }
        else
        {
            InitPosition();
            initTime = Time.deltaTime;
        }
    }

    public void InitPosition()
    {
        switch (Type)
        {
            case TextEffectType.TextEffect_Miss :
            case TextEffectType.TextEffect_Hit :
                _pivot = _textEffectManager.Get_Hit_Pivot();
                break;
            case TextEffectType.TextEffect_Damaged :
            case TextEffectType.TextEffect_Dodge :
                _pivot = _textEffectManager.Get_Dodge_Pivot();
                break;
            default:
                break;
        }
        this.transform.localPosition = _pivot.localPosition;
    }

    public void ResetTextEffect()
    {
        switch (Type)
        {
            case TextEffectType.TextEffect_Hit :
                this.transform.localPosition = new Vector3(-2000, 0, 0);
                break;
            case TextEffectType.TextEffect_Miss :
                this.transform.localPosition = new Vector3(-2600,0, 0);
                break;
            case TextEffectType.TextEffect_Dodge :
                this.transform.localPosition = new Vector3(-2200,0, 0);
                break;
            case TextEffectType.TextEffect_Damaged :
                this.transform.localPosition = new Vector3(-2400,0, 0);
                break;
            default:
                break;
        }
        isEnable = false;
    }

    public void SetTextEffectStatus(bool check)
    {
        isPaused = check;
    }
}

public enum TextEffectType
{
    TextEffect_Hit, TextEffect_Miss, TextEffect_Dodge, TextEffect_Damaged, 
    TNULL
}
