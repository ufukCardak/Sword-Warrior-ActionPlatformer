using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Enemy
{
    EnemyMovement movement;
    EnemyHealth health;

    public Transform target;
    public bool enemyDetected,attackControl;
    private void Awake()
    {
        movement = GetComponentInParent<EnemyMovement>();
        health = GetComponentInParent<EnemyHealth>();
        animator = GetComponentInParent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
    }
    private void Start()
    {
        movement.SetAttack(this);
    }
    private void FixedUpdate()
    {
        if (!movement.groundInfo)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (health.enemyStun || !health.isAlive)
        {
            return;
        }
        if (movement.canMove && enemyDetected && movement.groundInfo && !health.takingHit)
        {
            animator.ResetTrigger("Attack");
            direction = (target.position - transform.parent.position).normalized;
            movement.movingR = PlayerDirection();
            animator.SetBool("isRun", true);
            rb.velocity = new Vector2(movement.speed * Time.deltaTime * direction.x, rb.velocity.y);
            if (direction.x < 0)
            {
                transform.parent.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.parent.transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            target = collision.transform;
            if (health.takingHit)
            {
                return;
            }
            enemyDetected = true;
            //movement.canMove = false;
            animator.SetBool("isRun", false);
            rb.velocity = Vector2.zero;
            StartCoroutine("RandomAttack");
            //animator.SetTrigger("Attack");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartAttack(false);
            movement.canMove = true;
            StopCoroutine("RandomAttack");
            if (animator.speed == 1)
            {
                animator.SetBool("isRun", true);
                animator.ResetTrigger("Attack");
            }
        }
    }
    public void ResetAttack()
    {
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
    }
    void StartAttack(bool isAttacking)
    {
        if (animator.speed == 1)
        {
            animator.SetBool("Attack1", isAttacking);
            animator.SetBool("Attack2", isAttacking);
            animator.SetBool("Attack3", isAttacking);
        }
    }
    IEnumerator RandomAttack()
    {
        float waitNumber = Random.Range(0.4f,0.7f);
        yield return new WaitForSeconds(waitNumber);
        StartAttack(true);
        //animator.SetTrigger("Attack");
    }

}
