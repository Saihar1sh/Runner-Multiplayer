using System;
using Arixen.ScriptSmith;
using UnityEngine;

public class CylinderObstacle : MonoBehaviour
{
    private int _fallTriggerHash = Animator.StringToHash("FallTrigger");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)        //player layer
        {
            TriggerFall();
        }
    }

    public void TriggerFall()
    {
        _animator.SetTrigger(_fallTriggerHash);
    }
    private void OnDisable()
    {
        Debug.Log("disable");
        _animator.ResetTrigger(_fallTriggerHash);
    }
}
