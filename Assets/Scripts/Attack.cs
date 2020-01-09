using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MovableObject
{
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected string[] damageTargets;

    protected LinkedList<int> bannedTargets;

    protected Character owner;
    public virtual void initialize(GameController gc, Character owner, float forwardOffset, float horizontalOffset)
    {
        bannedTargets = new LinkedList<int>();
        gameController = gc;
        this.owner = owner;
        //transform.Translate(transform.up.normalized * forwardOffset + transform.up.normalized * horizontalOffset);
    }

    public virtual void banTarget(int id)
    {
        bannedTargets.AddFirst(id);
    }

    public static bool dealDamage(Character target, int damage)
    {
        return target.applyDamage(damage);
    }
    protected virtual bool dealDamage(Character target)
    {
        return dealDamage(target, damage);
    }

    protected Character getTargetClass(string target, Character other)
    {
        switch (target.ToLower())
        {
            case "enemy": return other.GetComponent<Enemy>();
            case "player": return other.GetComponent<Player>();
            default: Debug.LogError("Invalid target string" + target); return null;
        }
    }

    protected virtual void checkCharacterCollision(Character other)
    {
        return;
    }
    protected virtual void checkBannedCollision(Character other)
    {
        return;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Projectile asProj = other.GetComponent<Projectile>();
        if (asProj != null)
            return;
        Character asChar = other.GetComponent<Character>();
        if (asChar != null)
        {
            bool validTarget = false;
            // If this is a character this projectile is meant to damage and hasn't already hit, deal damage to it
            foreach (string s in damageTargets)
            {
                if (getTargetClass(s, asChar))
                {
                    if (bannedTargets.Contains(other.gameObject.GetInstanceID()))
                    {
                        checkBannedCollision(asChar);
                        continue;
                    }
                    else
                    {
                        dealDamage(asChar);
                        validTarget = true;
                        checkCharacterCollision(asChar);
                        continue;
                    }
                }
            }
            if (!validTarget)
                return;
        }
    }
}
