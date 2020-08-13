using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class IngameSFXManager : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public AudioSource[] SFX;

    private AudioSource Mammoth_Attack;
    private AudioSource Mammoth_Damaged;
    private AudioSource Babarian_Attack;
    private AudioSource Babarian_Dodge;
    private AudioSource Babarian_Aim;

    private int _max_SFX_count;

    private float SFX_Vol;
    // Start is called before the first frame update
    void Start()
    {
        AudioMixer.GetFloat("SFX_Vol", out SFX_Vol);
        _max_SFX_count = SFX.Length;
        
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage_StoneAge" :
                Mammoth_Attack = SFX[0];
                Mammoth_Damaged = SFX[1];
                Babarian_Attack = SFX[2];
                Babarian_Dodge = SFX[3];
                Babarian_Aim = SFX[4];
                break;
            case "Stage_MiddleAge" :
                break;
            case "Stage_ModernAge" :
                break;
            case "Stage_SciFi" :
                break;
            default :
                break;
        }
    }

    public void VolChangeSFX(float value)
    {
        SFX_Vol = value;
        if (value == -30f)
        {
            AudioMixer.SetFloat("SFX_Vol", -100f);
        }
        else
        {
            AudioMixer.SetFloat("SFX_Vol", value);
        }
    }

    public float GetSFXVol()
    {
        return SFX_Vol;
    }

    public void PlayStoneAgeSFX(StoneAge_SFX type)
    {
        switch (type)
        {
            case StoneAge_SFX.Mammoth_Attack :
                Mammoth_Attack.Play();
               break;
            case StoneAge_SFX.Mammoth_Damaged :
                Mammoth_Damaged.Play();
                break;
            case StoneAge_SFX.Babarian_Attack :
                Babarian_Attack.Play();
                break;
            case StoneAge_SFX.Babarian_Dodge :
                Babarian_Dodge.Play();
                break;
            case StoneAge_SFX.Babarian_Aim :
                Babarian_Aim.Play();
                break;
            default :
                Debug.Log("IngameSFXManger - PlayStoneAgeSFX : Unexpected Value Excpetion");
                break;
        }
    }
}

public enum StoneAge_SFX
{
    Mammoth_Attack, Mammoth_Damaged, Babarian_Attack, Babarian_Dodge, Babarian_Aim, NULL
}
