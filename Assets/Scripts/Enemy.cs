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
    private int attackCD = 0;
    private int attackAnimCD = 0;
    private int restCD = 0;
    protected Character target = null;
    protected Vector3 movement = Vector3.zero;

    private enum State { IDLE, MOVE, ATTACK, REST };
    private State curState;

    protected abstract void move();
    protected abstract void attack();
    protected virtual void prepAttack() { }
    protected virtual void startMove() { }
    protected virtual void startAttack() { }

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
            moveDir = Vector3.zero;
        } else {
            move();
            attackCD -= 1;
        }
        
    }

    protected void checkAttack() {
        if (attackAnimCD == 0) {
            attack();
            restCD = restTime;
            curState = State.REST;
        } else {
            prepAttack();
            attackAnimCD -= 1;
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
