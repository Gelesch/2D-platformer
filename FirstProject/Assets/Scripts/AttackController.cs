using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private bool _isAttack;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isAttack = true;
            _animator.SetTrigger("attack");
        }
    }
}
