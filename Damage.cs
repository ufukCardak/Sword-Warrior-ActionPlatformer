using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private int dmg;
    bool stun = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && transform.parent.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            EnemyHealth enemyHealth = transform.parent.GetComponent<EnemyHealth>();

            if (collision.GetComponent<PlayerAttack>().isBlocking)
            {
                enemyHealth.EnemyStun();
                playerHealth.EnemyStunBegun();
                stun = true;
            }
            else
            {
                if (stun)
                {
                    stun = false;
                    return;
                }

                //int randomDamage = Random.Range((dmg * enemyHealth.playerLevel) / 2, dmg * enemyHealth.playerLevel);
                // int damage = randomDamage - playerHealth.playerDefans;

                int randomDamage = Random.Range(dmg + enemyHealth.playerLevel + 5, dmg + enemyHealth.playerLevel + 10);

                DamageText.Create(collision.transform.position, randomDamage);
                playerHealth.TakeDamage(randomDamage, transform);
            }  
        }
    }
}
