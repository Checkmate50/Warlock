using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    [SerializeField]
    protected int moveTime;
    [SerializeField]
    protected int attackTime;
    [SerializeField]
    protected int restTime;
    [SerializeField]
    protected float aggroRadius;

    protected float aggroRadiusSquared; 
    protected int attackCD = 0;
    protected int attackAnimCD = 0;
    protected int restCD = 0;
    protected Character target = null;
    protected Vector3 movement = Vector3.zero;
    protected Vector2 attackDir = Vector3.zero;

    protected enum State { IDLE, MOVE, ATTACK, REST };
    protected State curState;

    protected virtual void setMoveSprite()
    {
        float moveAngle = Mathf.Atan2(moveDir.y, moveDir.x);
        if (moveAngle < Mathf.PI / 4f && moveAngle > -Mathf.PI / 4f)
            setSprite(moveRight);
        else if (moveAngle < Mathf.PI * 3f / 4f && moveAngle > Mathf.PI / 4f)
            setSprite(moveUp);
        else if (moveAngle > -Mathf.PI * 3f / 4f && moveAngle < -Mathf.PI / 4f)
            setSprite(moveDown);
        else
            setSprite(moveLeft);
    }
    protected virtual void setAttackSprite()
    {
        float attackAngle = Mathf.Atan2(attackDir.y, attackDir.x);
        if (attackAngle < Mathf.PI / 4f && attackAngle > -Mathf.PI / 4f)
            setSprite(moveRight);
        else if (attackAngle < Mathf.PI * 3f / 4f && attackAngle > Mathf.PI / 4f)
            setSprite(moveUp);
        else if (attackAngle > -Mathf.PI * 3f / 4f && attackAngle < -Mathf.PI / 4f)
            setSprite(moveDown);
        else
            setSprite(moveLeft);
    }

    protected abstract void attack();
    protected abstract void move();
    protected virtual void prepAttack() { }
    protected virtual void startMove() { restartMoving(); }
    protected virtual void startAttack() {
        attackDir = moveDir;
        stopMoving();
    }

    protected override void Start() {
        base.Start();
        aggroRadiusSquared = aggroRadius * aggroRadius;
    }

    protected override void Update() {
        base.Update();
        if (curState == State.IDLE)
            checkTarget();
        if (curState == State.MOVE)
            checkMove();
        if (curState == State.ATTACK)
            checkAttack();
        if (curState == State.REST)
            checkRest();
    }

    protected void restartMoving()
    {
        speed = baseSpeed;
    }

    protected void stopMoving()
    {
        speed = 0;
    }

    protected void checkTarget() {
        foreach (GameObject o in gameController.Players) {
            Player c = o.GetComponent<Player>();
            if (Util.FastDistance(c.transform.position, transform.position) < aggroRadiusSquared) {
                target = c;
                if (attackCD == 0)
                    attackCD = attackTime;
                curState = State.MOVE;
                return;
            }
        }
    }

    protected void checkMove() {
        if (attackCD == 0) {
            attackAnimCD = attackTime;
            curState = State.ATTACK;
            startAttack();
        } else {
            attackCD -= 1;
            setMoveSprite();
            move();
        }
        
    }

    protected void checkAttack() {
        if (attackAnimCD == 0) {
            attack();
            restCD = restTime;
            moveDir = attackDir;
            curState = State.REST;
        } else {
            attackAnimCD -= 1;
            setAttackSprite();
            prepAttack();
        }
    }

    protected void checkRest() {
        if (restCD == 0) {
            startMove();
            attackCD = moveTime;
            curState = State.MOVE;
        } else
            restCD -= 1;
    }
}
