using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_Charactor_Animation_Controller : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
    }

    public void TriggerDamage()
    {
        _animator.SetTrigger("Damaged");
    }

    public void TriggerAttack()
    {
        _animator.SetBool("isReady", false);
        _animator.SetTrigger("Attack");
    }

    public void TriggerDodge()
    {
        _animator.SetTrigger("Dodge");
    }

    public void SetAttackReady()
    {
        _animator.SetBool("isReady", true);
    }

    public void SetDefault()
    {
        _animator.SetBool("isReady", false);
    }

    public void TriggerDie()
    {
        _animator.SetTrigger("isDead");
    }
}
