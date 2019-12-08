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

    private Vector2 targetAccumulatedTransform;

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
        targetAccumulatedTransform = Vector2.zero;
    }
    protected override void prepAttack() {
        targetAccumulatedTransform += target.getVelocity();
    }
    protected override void attack() {
        Vector3 targetTransform = targetAccumulatedTransform / attackTime;
        Vector2 dir = target.transform.position - transform.position;
        if (targetTransform.magnitude > .1f)
            dir = Util.CalcInterceptDir(dir, targetTransform, target.Speed, projectile.MaxSpeed);
        createProjectile(projectile, dir, 0f, 0f);
    }
}
