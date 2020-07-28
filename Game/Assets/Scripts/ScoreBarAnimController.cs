using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBarAnimController : MonoBehaviour
{
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    public void PlayOnChangeScoreBar()
    {
        _animator.SetTrigger("TrgChange");
    }
}
