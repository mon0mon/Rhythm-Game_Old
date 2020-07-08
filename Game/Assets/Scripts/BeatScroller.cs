using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;

    private bool hasStarted;

    void Start()
    {
        beatTempo = beatTempo / 60f;
    }
    
    private void Update()
    {
        if (hasStarted)
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }

    public void setStart(bool check)
    {
        hasStarted = check;
    }
}
