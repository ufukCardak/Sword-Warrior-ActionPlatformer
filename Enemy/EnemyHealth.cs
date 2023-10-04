using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Enemy
{
    EnemyMovement movement;
    EnemyAttack attack;
    ChangeColor changeColor;

    public int enemyTier;
    public bool isAlive = true;
    public bool takingHit,isBlocking,enemyStun;
    private Slider enemyHeaalthSlider;
    private BoxCollider2D boxCollider;
    public int knockback;

    public int playerLevel;

    private void Awake()
    {
        changeColor = GetComponent<ChangeColor>();
        movement = GetComponent<EnemyMovement>();
        enemyHeaalthSlider = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        playerLevel = transform.parent.GetComponent<PlayerLevel>().playerLevel;
        if (enemyTier == 2)
        {
            enemyTier = Random.Range(2, 4);
        }
        enemyHeaalthSlider.maxValue = (30 * playerLevel * enemyTier);
        enemyHeaalthSlider.value = enemyHeaalthSlider.maxValue;
        RandomColorStart();
    }

    public void SetEnemyAttack(EnemyAttack enemyAttack)
    {
        attack = enemyAttack;
    }

    void RandomColorStart()
    {
        string colorString;
        int randomColor = Random.Range(1, 6);
        switch (randomColor)
        {
            case 1:
                colorString = "FFFFFF";
                break;
            case 2:
                colorString = "00DBFF";
                break;
            case 3:
                colorString = "000CFF";
                break;
            case 4:
                colorString = "FFF000";
                break;
            case 5:
                colorString = "FF0000";
                break;

            default:
                colorString = "FFFFFF";
                break;
        }
        changeColor.SetEnemyWeaponColor(colorString);
    }

    public void EnemyStun()
    {
        attack.ResetAttack();
        StartCoroutine(EnemyWaitStun());
    }
    public void TakeDamage(int dmg,Transform player)
    {
        enemyHeaalthSlider.value -= dmg;
        enemyStun = false;
        if (enemyHeaalthSlider.value > 0)
        {
            StartCoroutine(StopRigidbody(player.parent.transform));
        }
        if (!attack.enemyDetected)
        {
            attack.target = player.parent.transform;
            attack.enemyDetected = true;
            if (takingHit)
            {
                return;
            }
           
            if (movement.movingR == 1)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                movement.movingR = -1;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movement.movingR = 1;
            }
            movement.canMove = true;

        }
        if (enemyHeaalthSlider.value <= 0)
        {
            isAlive = false;
            int exp = Random.Range(10 * playerLevel, 30 * playerLevel) * enemyTier;
            player.parent.GetComponent<PlayerEXP>().AddExpCoin(exp, Random.Range(10 * playerLevel, 20 * playerLevel) * enemyTier);

            gameObject.layer = LayerMask.NameToLayer("Default");
            changeColor.StartColor();
            enemyHeaalthSlider.transform.gameObject.SetActive(false);
            boxCollider.isTrigger = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            StartCoroutine(changeColor.DyingAnim(0.25f));
            
            EnemyDead();
            Destroy(gameObject, 1.5f);
        }
    }
    void EnemyDead()
    {
        GetComponent<EnemyHealth>().enabled = false;
        GetComponent<EnemyMovement>().enabled = false;
        animator.speed = 0f;
    }
    IEnumerator StopRigidbody(Transform player)
    {
        takingHit = true;
        movement.canMove = false;
        direction = (player.position - transform.position).normalized;

        if (direction.x < 0)
        {
            rb.AddForce(new Vector2(knockback, knockback));
        }
        else
        {
            rb.AddForce(new Vector2(-knockback, knockback));
        }
        rb.velocity = Vector2.zero;

        animator.speed = 0f;
        changeColor.SetColor("000000");
        yield return new WaitForSeconds(0.25f);
        changeColor.SetColor("FFFFFF");
        movement.canMove = true;
        takingHit = false;
        animator.speed = 1;
    }

    IEnumerator EnemyWaitStun()
    {
        movement.canMove = false;
        rb.velocity = Vector2.zero;
        changeColor.SetColor("000000");
        animator.speed = 0f;
        enemyStun = true;
        yield return new WaitForSeconds(1.5f);
        enemyStun = false;
        movement.canMove = true;
        rb.velocity = Vector2.zero;
        changeColor.SetColor("FFFFFF");
        animator.speed = 1;

    }
}
