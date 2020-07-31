using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_TextEffect_Controller : MonoBehaviour
{
    public TextEffectType Type;

    private Ingame_TextEffect_Manager _textEffectManager;
    private Animator _animator;
    
    private bool isPaused;

    void Start()
    {
        _textEffectManager = GameObject.Find("Manager").GetComponent<Ingame_TextEffect_Manager>();

        _animator = gameObject.GetComponent<Animator>();
    }

    public void EnableTextEffect()
    {
        _animator.SetTrigger("TrgAction");
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
