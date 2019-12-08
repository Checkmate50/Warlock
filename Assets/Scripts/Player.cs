using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private string up;
    [SerializeField]
    private string down;
    [SerializeField]
    private string left;
    [SerializeField]
    private string right;

    [SerializeField]
    private Projectile magicMissile;
    [SerializeField]
    private int mmCooldown;
    private int mmcd = 0;
    private bool mmCast = false;

    private Vector2 attackDirection = Vector2.zero;

    protected override void Update() {
        base.Update();
        Vector3 mousePos = gameController.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        attackDirection = mousePos - transform.position;
        checkInputs();
        checkMove();
        checkAttack();
        updateCooldowns();
    }

    private void checkInputs() {
        if (Input.GetMouseButtonDown(0))
            mmCast = true;
        if (Input.GetMouseButtonUp(0))
            mmCast = false;
    }

    private void checkMove() {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        moveDir = new Vector3(moveHorizontal, moveVertical, 0.0f);
    }

    private void checkAttack() {
        if (mmcd == 0 && mmCast) {
            createProjectile(magicMissile, attackDirection, 20.0f, 0.0f);
            mmcd = mmCooldown;
        }
    }

    private void updateCooldowns() {
        if (mmcd > 0)
            mmcd -= 1;
    }
}
