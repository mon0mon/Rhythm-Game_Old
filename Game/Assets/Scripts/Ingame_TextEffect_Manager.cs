using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_TextEffect_Manager : MonoBehaviour
{
    public TextEffectEnable TextEffect;
    public GameObject[] TextEffect_Hit;
    public GameObject[] TextEffect_Dodge;
    public GameObject[] TextEffect_Miss;
    public GameObject[] TextEffect_Damaged;
    public Transform TextEffect_Hit_Pivot;
    public Transform TextEffect_Dodge_Pivot;
    public float TextEffectLiveTime = 1.5f;
    public float TextEffectSpeed = 30.0f;

    private int _index_Hit = 0;
    private int _index_Dodge = 0;
    private int _index_Miss = 0;
    private int _index_Damaged = 0;
    private bool isPaused = false;
    private Ingame_TextEffect_Controller[] _hitController;
    private Ingame_TextEffect_Controller[] _dodgeController;
    private Ingame_TextEffect_Controller[] _missController;
    private Ingame_TextEffect_Controller[] _damagedController;
    
    private void Start()
    {
        if (SceneData.Instance.TextEffect != TextEffectEnable.NULL) TextEffect = SceneData.Instance.TextEffect;
        
        if (TextEffect_Hit.Length != 0)
        {
            _hitController = new Ingame_TextEffect_Controller[TextEffect_Hit.Length];
            for (int i = 0; i < TextEffect_Hit.Length; i++)
            {
                TextEffect_Hit[i].transform.localPosition = new Vector3(-2000, 0, 0);
                _hitController[i] = TextEffect_Hit[i].GetComponent<Ingame_TextEffect_Controller>();
            }
        }
        
        if (TextEffect_Dodge.Length != 0)
        {
            _dodgeController = new Ingame_TextEffect_Controller[TextEffect_Dodge.Length];
            for (int i = 0; i < TextEffect_Dodge.Length; i++)
            {
                TextEffect_Dodge[i].transform.localPosition = new Vector3(-2200, 0, 0);
                _dodgeController[i] = TextEffect_Dodge[i].GetComponent<Ingame_TextEffect_Controller>();
            }
        }
        
        if (TextEffect_Miss.Length != 0)
        {
            _missController = new Ingame_TextEffect_Controller[TextEffect_Miss.Length];
            for (int i = 0; i < TextEffect_Miss.Length; i++)
            {
                TextEffect_Miss[i].transform.localPosition = new Vector3(-2600, 0, 0);
                _missController[i] = TextEffect_Miss[i].GetComponent<Ingame_TextEffect_Controller>();
            }
        }
        
        if (TextEffect_Damaged.Length != 0)
        {
            _damagedController = new Ingame_TextEffect_Controller[TextEffect_Damaged.Length];
            for (int i = 0; i < TextEffect_Damaged.Length; i++)
            {
                TextEffect_Damaged[i].transform.localPosition = new Vector3(-2400, 0, 0);
                _damagedController[i] = TextEffect_Damaged[i].GetComponent<Ingame_TextEffect_Controller>();
            }
        }

        StartCoroutine(LateStart());
    }

    public void PrintHitEffect()
    {
        if (_index_Hit == TextEffect_Hit.Length) ResetIndex(IndexType.TextEffect_Hit);

        _hitController[_index_Hit].EnableTextEffect(TextEffectLiveTime);
        _index_Hit++;
    }

    public void PrintDodgeEffect()
    {
        if (_index_Dodge == TextEffect_Dodge.Length) ResetIndex(IndexType.TextEffect_Dodge);

        _dodgeController[_index_Dodge].EnableTextEffect(TextEffectLiveTime);
        _index_Dodge++;
    }

    public void PrintMissEffect()
    {
        if (_index_Miss == TextEffect_Miss.Length) ResetIndex(IndexType.TextEffect_Miss);

        _missController[_index_Miss].EnableTextEffect(TextEffectLiveTime);
        _index_Miss++;
    }
    
    public void PrintDamagedEffect()
    {
        if (_index_Damaged == TextEffect_Damaged.Length) ResetIndex(IndexType.TextEffect_Damaged);

        _damagedController[_index_Damaged].EnableTextEffect(TextEffectLiveTime);
        _index_Damaged++;
    }

    public void ResetIndex(IndexType type)
    {
        switch (type)
        {
            case IndexType.TextEffect_Hit:
                _index_Hit = 0;
                break;
            case IndexType.TextEffect_Dodge:
                _index_Dodge = 0;
                break;
            case IndexType.TextEffect_Miss:
                _index_Miss = 0;
                break;
            case IndexType.TextEffect_Damaged:
                _index_Damaged = 0;
                break;
        }
    }

    public Transform Get_Hit_Pivot()
    {
        return TextEffect_Hit_Pivot;
    }
    
    public Transform Get_Dodge_Pivot()
    {
        return TextEffect_Dodge_Pivot;
    }
    
    public void ResetPosition()
    {
        for (int i = 0; i < TextEffect_Hit.Length; i++)
        {
            TextEffect_Hit[i].transform.localPosition = new Vector3(-2000, 0, 0);
        }
        
        for (int i = 0; i < TextEffect_Dodge.Length; i++)
        {
            TextEffect_Dodge[i].transform.localPosition = new Vector3(-2200, 0, 0);
        }

        for (int i = 0; i < TextEffect_Miss.Length; i++)
        {
            TextEffect_Miss[i].transform.localPosition = new Vector3(-2600, 0, 0);
        }

        for (int i = 0; i < TextEffect_Damaged.Length; i++)
        {
            TextEffect_Damaged[i].transform.localPosition = new Vector3(-2400, 0, 0);
        }
    }

    public void ToggleStatusTextEffect()
    {
        isPaused = !isPaused;
        
        for (int i = 0; i < TextEffect_Hit.Length; i++)
        {
            _hitController[i].SetTextEffectStatus(isPaused);
        }
        
        for (int i = 0; i < TextEffect_Dodge.Length; i++)
        {
            _dodgeController[i].SetTextEffectStatus(isPaused);
        }
        
        for (int i = 0; i < TextEffect_Miss.Length; i++)
        {
            _missController[i].SetTextEffectStatus(isPaused);
        }
        
        for (int i = 0; i < TextEffect_Damaged.Length; i++)
        {
            _damagedController[i].SetTextEffectStatus(isPaused);
        }
    }

    public float GetTextEffectSpeed()
    {
        return TextEffectSpeed;
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

public enum IndexType
{
    TextEffect_Hit, TextEffect_Dodge, TextEffect_Miss, TextEffect_Damaged
}
