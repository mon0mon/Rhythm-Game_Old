using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class IngameSFXManager : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public AudioSource[] SFX;

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
        
    }

    public void PlayLoadingSFX()
    {
        
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
