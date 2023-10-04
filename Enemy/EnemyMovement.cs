using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Enemy
{
    EnemyAttack attack;
    EnemyHealth health;

    public LayerMask layerMask,layerMaskPlayer;
    public Transform groundDetection,playerDetection;

    public int movingR = 1;
    public float speed = 75f;
    public bool canMove = true;
    public RaycastHit2D groundInfo;

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f, layerMask);
        Debug.DrawRay(groundDetection.position, Vector2.down * 1f, Color.blue);

        RaycastHit2D forwardInfo = Physics2D.Raycast(playerDetection.position, Vector2.left * movingR, 0.2f, layerMaskPlayer);
        Debug.DrawRay(playerDetection.position, Vector2.left * movingR * 0.2f, Color.blue);

        if (health.enemyStun || !health.isAlive)
        {
            return;
        }

        if (attack.enemyDetected && !groundInfo)
        {
            Debug.Log("1");
            direction = (attack.target.position - transform.position).normalized;
            if (direction.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            animator.SetBool("isRun", false);
            //FliptoPlayer();
        }

        if (!groundInfo)
        {
            Flip();
        }

        if (forwardInfo && forwardInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Flip();
        }
        if (forwardInfo && forwardInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            canMove = false;
            animator.SetBool("isRun", false);
        }

        if (!canMove || attack.enemyDetected || health.takingHit)
        {
            return;
        }

        animator.SetBool("isRun", true);
        rb.velocity = new Vector2(speed * Time.deltaTime * Vector2.left.x * movingR, rb.velocity.y);
    }
    public void SetAttack(EnemyAttack enemyAttackScript)
    {
        attack = enemyAttackScript;
        health.SetEnemyAttack(enemyAttackScript);
    }
    void Flip()
    {
        if (attack.enemyDetected)
        {
            return;
        }
        if (movingR == 1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            movingR = -1;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingR = 1;
        }
    }
}
