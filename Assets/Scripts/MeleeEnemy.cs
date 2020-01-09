using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected float attackAngle;
    [SerializeField]
    protected int damage;

    protected float attackTriggerSquared;

    protected override void Start()
    {
        base.Start();
        attackTriggerSquared = attackRange * attackRange / 2f;
    }
    protected override void move()
    {
        moveDir = target.transform.position - transform.position;
        if (Util.FastDistance(transform.position, target.transform.position) < attackTriggerSquared)
            attackCD = 0;
    }

    protected bool tryAttack(float angle)
    {
        Vector2 rayAngle = Quaternion.Euler(0, 0, angle) * moveDir;
        foreach (RaycastHit2D result in Physics2D.RaycastAll(transform.position, rayAngle, attackRange)) {
            Player other = result.collider.GetComponent<Player>();
            if (other)
            {
                other.applyDamage(damage);
                return true;
            }   
        }
        return false;
    }
    protected override void attack()
    {
        Debug.Log("attack");
        for (float angle = -attackAngle; angle < attackAngle; angle += attackAngle / 20f)
            if (tryAttack(angle))
                break;
    }
}
