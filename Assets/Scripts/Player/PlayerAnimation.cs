using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    
    private readonly int moveHash = Animator.StringToHash("IsMoving");
    private readonly int isJumpHash = Animator.StringToHash("IsJumping");
    private readonly int jumpTriggerHash = Animator.StringToHash("jumpTrigger");
    private readonly int isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int isRollHash = Animator.StringToHash("IsRoll");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void UpdateMovement(bool isMoving)
    {
        animator.SetBool(moveHash, isMoving);
    }

    public void PlayJump()
    {
        animator.SetTrigger(jumpTriggerHash);
        animator.SetBool(isJumpHash, true);
        animator.SetBool(isGroundedHash, false);
    }

    public void PlayGrounded()
    {
        animator.SetBool(isJumpHash, false);
        animator.SetBool(isGroundedHash, true);
    }

    public void PlayRoll()
    {
        animator.SetBool(isRollHash, true);
        StartCoroutine(EndRoll());
    }

    private IEnumerator EndRoll()
    {
        yield return new WaitForSeconds(0.8f);
        animator.SetBool(isRollHash, false);
    }
}
