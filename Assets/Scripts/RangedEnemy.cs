using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    protected Projectile projectile;
    [SerializeField]
    protected float closeDistance;
    private float closeDistanceSquared;

    private Vector2 targetTransform;
    private Vector2 targetPrevTransform;

    protected override void Start() {
        base.Start();
        closeDistanceSquared = closeDistance * closeDistance;
    }

    protected override void move() {
        if (Util.FastDistance(transform.position, target.transform.position) > closeDistanceSquared)
            moveDir = target.transform.position - transform.position;
        else
            moveDir = Vector3.zero;
    }
    protected override void startAttack() {
        base.startAttack();
        targetTransform = Vector2.zero;
        targetPrevTransform = target.transform.position;
    }
    protected override void prepAttack() {
        attackDir = target.transform.position - transform.position;
        targetTransform += (Vector2)target.transform.position - targetPrevTransform;
        targetPrevTransform = target.transform.position;
    }
    protected override void attack() {
        // Magic variable to control how much the attacker leads the player, lower is more
        float leading = 50f;
        // Note that we don't account for attackTime since our weighting mostly clears it
        float foundSpeed = targetTransform.magnitude * leading / attackTime;
        Vector2 dir = target.transform.position - transform.position;
        // If the player moved for over 2/3 the prepping time, lead the player
        if (foundSpeed > target.Speed * 2f / 3f)
            dir = Util.CalcInterceptDir(dir, targetTransform, 
                foundSpeed, projectile.MaxSpeed);
        createProjectile(projectile, dir, 0f, 0f);
    }
}
