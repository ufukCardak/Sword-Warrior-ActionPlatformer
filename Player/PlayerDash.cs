using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : Player
{
    bool canDash = true;

    [SerializeField] float dashingPower = 6f;
    float dashingTime = 0.2f;
    float dashingCooldown = 1f;
    private void Awake()
    {
        playerDash = this;
    }
    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        if (transform.rotation.y == 0)
            rb.velocity = new Vector2(-1 * dashingPower, 0);
        else
            rb.velocity = new Vector2(1 * dashingPower, 0);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = 1f;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    public void PlayerDownDash()
    {
        if (!Check())
        {
            return;
        }
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }
}
