using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MovableObject
{
    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected float baseSpeed;
    protected float speed;
    protected int health;
    protected Vector3 moveDir;
    public Vector3 MoveDirection {
        get { return moveDir; }
    }
    public float Speed {
        get { return speed; }
    }
    public int Health {
        get { return health; }
    }

    public virtual void initialize(GameController gc) {
        gameController = gc;
    }

    protected new virtual void Start() {
        base.Start();
        speed = baseSpeed;
        health = maxHealth;
    }

    protected new virtual void Update() {
        base.Update();
        rb.velocity = getVelocity();
    }

    public Vector2 getVelocity() {
        return moveDir.normalized * speed * Time.deltaTime * 60.0f;
    }

    // Note that negative damage is the same as healing
    // Returns true if this character was destroyed
    public bool damage(int amount) {
        health -= amount;
        if (health > maxHealth)
            health = maxHealth;
        if (health <= 0) {
            death();
            return true;
        }
        return false;
    }

    protected virtual void death() {
        Enemy temp = gameObject.GetComponent<Enemy>();
        if (temp)
            gameController.enemyDeath(temp);
        Player temp2 = gameObject.GetComponent<Player>();
        if (temp2)
            gameController.playerDeath(temp2);
        Destroy(gameObject);
    }

    protected virtual void createProjectile(Projectile proj, Vector2 direction, float forOffset, float horOffset) {
        Vector2 dir = new Vector2(direction.y, direction.x);
        Projectile result = Instantiate(proj, transform.position, Quaternion.LookRotation(Vector3.forward, direction.normalized));
        result.initialize(gameController, this, forOffset, horOffset);
    }
}
