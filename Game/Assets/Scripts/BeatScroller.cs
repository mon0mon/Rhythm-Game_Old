using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;

    private bool hasStarted;
    private bool hasStoped = false;

    void Start()
    {
        beatTempo = beatTempo / 60f;
    }
    
    private void Update()
    {
        if (hasStarted && hasStoped) { } 
        else if (hasStarted && !hasStoped)
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }

    public void SetStart(bool check)
    {
        hasStarted = check;
    }

    public void SetStop(bool check)
    {
        hasStoped = check;
    }
}
