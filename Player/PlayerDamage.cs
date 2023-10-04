using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : Player
{
    [SerializeField] int basicDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            int randomDamage = Random.Range((basicDamage + damage) / 2, basicDamage + damage);
            if (collision.GetComponent<EnemyHealth>().enemyStun)
            {
                randomDamage += Random.Range(5, 16);
            }
            DamageText.Create(collision.transform.position, randomDamage);
            collision.GetComponent<EnemyHealth>().TakeDamage(randomDamage, transform);
        }
        else if (collision.gameObject.tag == "Crate")
        {
            collision.GetComponent<CrateDestroy>().DestroyCrate();
        }
    }
}
