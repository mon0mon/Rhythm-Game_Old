using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class IngameSFXManager : MonoBehaviour
{
    public AudioMixer AudioMixer;

    private float SFX_Vol;
    // Start is called before the first frame update
    void Start()
    {
        AudioMixer.GetFloat("SFX_Vol", out SFX_Vol);
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
}
