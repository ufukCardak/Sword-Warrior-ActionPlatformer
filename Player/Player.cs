using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    protected static Rigidbody2D rb;
    protected static Animator animator;
    protected static PlayerHealth playerHealth;
    protected static PlayerAttack playerAttack;
    protected static PlayerDash playerDash;
    protected static PlayerMovement playerMovement;
    protected static PlayerEXP playerExp;

    protected static bool canMove = true;

    protected static bool canGrabLedge = true;
    protected static bool isDashing, ledgeDetected, takingDmg, canClimb, isAttacking,takedDmg;

    protected static int horizontalMove, currentCoin,damage;
    protected bool Check()
    {
        if (!canMove || isDashing || takedDmg || playerAttack.isBlocking || !playerHealth.isAlive)
        {
            return false;
        }
        return true;
    }
}
