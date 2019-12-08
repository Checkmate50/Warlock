using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MovableObject
{
    // Note that a negative value for damage provides healing
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float maxSpeed;
    [SerializeField]
    protected string[] damageTargets;

    protected float speed;
    protected Character owner;
    protected LinkedList<int> bannedTargets;

    public float MaxSpeed {
        get { return maxSpeed; }
    }

    public virtual void initialize(GameController gc, Character owner, float forwardOffset, float horizontalOffset) {
        gameController = gc;
        bannedTargets = new LinkedList<int>();
        speed = maxSpeed;
        this.owner = owner;
        
        //transform.Translate(transform.up.normalized * forwardOffset + transform.up.normalized * horizontalOffset);
        banObject(owner.GetInstanceID());
    }

    // Update is called once per frame
    protected new virtual void Update() {
        base.Update();
        transform.Translate(transform.up.normalized * speed * Time.deltaTime, Space.World);
        Vector3 camCheck = gameController.MainCamera.WorldToViewportPoint(transform.position);
        if (Mathf.Clamp01(camCheck.x) != camCheck.x || Mathf.Clamp01(camCheck.y) != camCheck.y)
            Destroy(gameObject);
    }

    public virtual void banObject(int id) {
        bannedTargets.AddFirst(id);
    }

    protected virtual bool dealDamage(Character target) {
        return target.damage(damage);
    }

    protected Character getTargetClass(string target, GameObject other) {
        switch (target.ToLower()) {
            case "enemy":   return other.GetComponent<Enemy>();
            case "player":  return other.GetComponent<Player>();
            default: Debug.LogError("Invalid target string" + target); return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject other = collision.gameObject;
        Projectile asProj = other.GetComponent<Projectile>();
        if (asProj != null)
            return;
        Character asChar = other.GetComponent<Character>();
        if (asChar != null) {
            bool validTarget = false;
            // If this is a character this projectile is meant to damage and hasn't already hit, deal damage to it
            foreach (string s in damageTargets) {
                if (getTargetClass(s, other)) {
                    if (bannedTargets.Contains(other.gameObject.GetInstanceID()))
                        return;
                    else {
                        dealDamage(asChar);
                        validTarget = true;
                        break;
                    }
                }
            }
            if (!validTarget)
                return;
        }
        Destroy(gameObject);
    }
    
    
}
