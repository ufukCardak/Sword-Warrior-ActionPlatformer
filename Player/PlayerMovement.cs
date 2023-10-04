using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Player
{
    private BoxCollider2D boxCollider;

    [SerializeField] private float speed = 125f;

    [SerializeField] private bool isGrounded, isJumping;
    [SerializeField] private Transform feetPos;
    public LayerMask whatIsGround;

    [SerializeField] private float jumpVelocity = 4.5f;
    private float jumpTimeCounter;
    private float jumpTime = 0.35f;
    private void Awake()
    {
        playerMovement = this;
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (!Check())
        {
            return;
        }
        isGrounded = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, whatIsGround);
        CheckGround();
        if (isGrounded && !isJumping)
        {
            animator.SetBool("isFall", false);
            animator.SetBool("isJump", false);
            animator.SetBool("isGrounded", true);
            takingDmg = false;
            canGrabLedge = true;
            rb.gravityScale = 1;
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }


        if (rb.velocity.y < 0 && !isGrounded && !isAttacking)
        {
            animator.SetBool("isJump", false);

            if (horizontalMove != 0)
            {
                animator.SetBool("isRun", true);
            }
            else
            {
                animator.SetBool("isFall", true);
            }
        }

        if (isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                CheckGround();
                rb.velocity = Vector2.up * jumpVelocity;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (!Check())
        {
            return;
        }
        
        rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rb.velocity.y);
    }
    void CheckGround()
    {
        if (rb.velocity.y < 0 && !isGrounded)
        {
            animator.SetBool("isFall", true);
        }
    }
    private void Jump()
    {
        if (!Check())
        {
            return;
        }
        if (isGrounded)
        {
            rb.velocity = Vector2.up * jumpVelocity;

            isJumping = true;
            jumpTimeCounter = jumpTime;

            animator.SetBool("isGrounded", false);
            animator.SetBool("isRun", false);
            animator.SetBool("isJump", true);
        }
    }
    public void PlayerMoveDownLeft()
    {
        if (!Check())
        {
            return;
        }
        if (canClimb)
        {
            animator.SetBool("CanClimb", false);
            canClimb = false;
        }
        Quaternion target = Quaternion.Euler(0, 0, 0);
        transform.rotation = target;
        if (isAttacking)
        {
            return;
        }
        animator.SetBool("isRun", true);
        horizontalMove = -1;
    }
    public void PlayerMoveDownRight()
    {
        if (!Check())
        {
            return;
        }
        if (canClimb)
        {
            animator.SetBool("CanClimb", false);
            canClimb = false;
        }
        Quaternion target = Quaternion.Euler(0, -180, 0);
        if (transform.rotation.y == 0)
        {
            transform.rotation = target;
        }

        if (isAttacking)
        {
            return;
        }
        animator.SetBool("isRun", true);
        horizontalMove = 1;
    }
    public void PlayerMoveUp()
    {
        animator.SetBool("isRun", false);

        horizontalMove = 0;
    }
    public void PlayerJumpDown()
    {
        if (!Check())
        {
            return;
        }
        Jump();
    }
    public void PlayerJumpUp()
    {
        isJumping = false;
    }
}


