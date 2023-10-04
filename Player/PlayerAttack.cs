using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttack : Player
{
    [Header("Ledge info")]
    [SerializeField] Vector2 offset1;
    [SerializeField] Vector2 offset2;

    Vector2 climbBegunPosition;
    Vector2 climbOverPosition;

    public bool isBlocking;

    private void Awake()
    {
        playerAttack = this;
    }
    private void Update()
    {
        if (!Check())
        {
            return;
        }
        CheckForLedge();
    }

    public void PlayerBlockDown()
    {
        if (isAttacking)
        {
            return;
        }
        isBlocking = true;
        animator.SetBool("isBlocking", true);
    }

    public void PlayerBlockUp()
    {
        isBlocking = false;
        animator.SetBool("isBlocking", false);
    }
    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            //climbBegunPosition = ledgePosition + offset1;
            //climbOverPosition = ledgePosition + offset2;

            if (transform.rotation.y == 0)
            {
                climbBegunPosition = ledgePosition + new Vector2(0.3f, 0.1f);
                climbOverPosition = ledgePosition + new Vector2(0.1f, 0.5f);

            }
            else
            {
                climbBegunPosition = ledgePosition + new Vector2(-0.3f, 0.1f);
                climbOverPosition = ledgePosition + new Vector2(0f, 0.5f);
            }
            canClimb = true;
            
        }
        if (canClimb)
        {
            if (takingDmg)
            {
                return;
            }
            transform.position = climbBegunPosition;
            animator.SetBool("CanClimb", true);
        }
            
    }
    private void LedgeClimbOver()
    {
        canMove = true;
        canClimb = false;
        transform.position = climbOverPosition;
        animator.SetBool("CanClimb", false);
        Invoke("AllowLedgeGrab", 0.5f);
    }
    private void AllowLedgeGrab() => canGrabLedge = true;

    public void Die()
    {
        playerAttack.enabled = false;
    }
    public void SetisAttacking(string getisAttacking)
    {
        isAttacking = bool.Parse(getisAttacking);
    }
    public void PlayerAttackDown()
    {
        if (!Check())
        {
            return;
        }
        animator.SetBool("isRun", false);
        animator.SetBool("isJump", false);
        animator.SetTrigger("Attack");
    }
    public void PlayerAttackUp()
    {
        if (horizontalMove != 0 && !isAttacking)
        {
            animator.SetBool("isRun", true);
        }
    }
}
