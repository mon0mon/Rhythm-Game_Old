using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource Music;

    private void Start()
    {
        Music.Play();
    }
}
