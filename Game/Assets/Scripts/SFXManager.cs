using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public AudioSource[] SFX;
    public AudioClip ButtonClickSound;
    public AudioClip LoadingSound;

    private int _max_SFX_count;
    private int index = 0;

    private float SFX_Vol;
    // Start is called before the first frame update
    void Start()
    {
        AudioMixer.GetFloat("SFX_Vol", out SFX_Vol);
        _max_SFX_count = SFX.Length;
    }

    public void VolChangeSFX(float value)
    {
        SFX_Vol = value;
        AudioMixer.SetFloat("SFX_Vol", value);
    }

    public float GetSFXVol()
    {
        return SFX_Vol;
    }

    public void PlayButtonClickSFX()
    {
        SFX[index].clip = ButtonClickSound;
        SFX[index].Play();
        IndexCounter();
    }

    public void PlayLoadingSFX()
    {
        SFX[index].clip = LoadingSound;
        SFX[index].Play();
        IndexCounter();
    }

    public void IndexCounter()
    {
        index++;
        if (index >= _max_SFX_count)
        {
            index = 0;
        }
    }
}
