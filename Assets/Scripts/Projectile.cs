using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attack
{
    // Note that a negative value for damage provides healing
    [SerializeField]
    protected float maxSpeed;

    protected float speed;

    public float MaxSpeed {
        get { return maxSpeed; }
    }

    public override void initialize(GameController gc, Character owner, float forwardOffset, float horizontalOffset) {
        base.initialize(gc, owner, forwardOffset, horizontalOffset);
        speed = maxSpeed;
        banTarget(owner.GetInstanceID());
    }

    // Update is called once per frame
    protected new virtual void Update() {
        base.Update();
        transform.Translate(transform.up.normalized * speed * Time.deltaTime, Space.World);
        Vector3 camCheck = gameController.MainCamera.WorldToViewportPoint(transform.position);
        if (Mathf.Clamp01(camCheck.x) != camCheck.x || Mathf.Clamp01(camCheck.y) != camCheck.y)
            Destroy(gameObject);
    }

    protected override void checkCharacterCollision(Character other)
    {
        Destroy(gameObject);
    }
}
