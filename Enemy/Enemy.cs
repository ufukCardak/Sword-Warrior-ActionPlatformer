using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Vector2 direction;

    protected void FliptoPlayer(Transform rotateGameObject)
    {
        if (direction.x < 0)
        {
            rotateGameObject.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            rotateGameObject.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    protected int PlayerDirection()
    {
        int movingRorL = 0;

        if (direction.x < 0)
        {
            movingRorL = 1;
        }
        else
        {
            movingRorL = -1;
        }

        return movingRorL;
    }
}

