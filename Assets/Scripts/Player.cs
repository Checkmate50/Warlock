using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
        checkInputs();
        checkMove();
        checkAttack();
        updateCooldowns();
    }

    class InputState {
        public string key_name;
        public bool state = false;
        public bool pressed = false;
        public bool unpressed = false;

        public InputState(string key_name) => this.key_name = key_name;

        public void update() {
            pressed = Input.GetKeyDown(key_name);
            unpressed = Input.GetKeyUp(key_name);
            state = pressed || (state && !unpressed);
        }

    }

    Dictionary<string, InputState> inputs = 
        new List<string>() {"up", "down", "left", "right", "w", "a", "s", "d"}
        .ConvertAll((key_name) => new InputState(key_name))
        .ToDictionary(input => input.key_name, input => input);

    
    private void checkInputs() {
        foreach (var input in inputs) { input.Value.update(); }
    }

    private void checkAttack() {
        if (mmcd == 0 && mmCast) {
            createProjectile(magicMissile, attackDirection, 20.0f, 0.0f);
            mmcd = mmCooldown;
        }
    }

    private void checkMove() {
        //unity_movement_smoothing();
        //pseudo_physics_smoothing();
        kinematic_smoothing();
    }

    // ------ Unity's smoothing through Input.GetAxis ------
    // kinda mediocre
    float movementSpeed = 10f;
    private void unity_movement_smoothing() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float offset = movementSpeed * Time.deltaTime;
        transform.position += new Vector3(horizontalInput*offset, verticalInput*offset, 0);
    }

    Vector2 curr_vel = Vector2.zero;
    Vector2 curr_accel = Vector2.zero;
    // if mu0 = input_accel / max_accel is in [-1, 1],
    // then acceleration converges to max_accel (it's an alternating geometric series) 
    //float mu0= 0.8f;
    float input_accel = 400;
    float max_accel = 500;

    // if mu1 = max_accel*deltaTime / max_vel is in [-1, 1],
    // then velocity converges to max_vel
    // mu1 is inversely proportional to max velocity and 'slipperiness'
    float mu1= 0.7f;
    
    private void kinematic_smoothing() {
        Vector2 input_a =
            new Vector2(
                inputs["d"].state ? input_accel : inputs["a"].state ? -input_accel : 0,
                inputs["w"].state ? input_accel : inputs["s"].state ? -input_accel : 0);
        curr_accel += input_a - (input_accel/max_accel)*curr_accel;
        curr_vel += curr_accel*Time.deltaTime - mu1*curr_vel;
        transform.position += (Vector3) curr_vel * Time.deltaTime;
        // Debug.Log("curr_accel = " + curr_accel.ToString());
        // Debug.Log("cur_vel = " + curr_vel.ToString());
        // Debug.Log("framerate = " + Application.targetFrameRate.ToString());
    }

    private void updateCooldowns() {
        if (mmcd > 0) {
            mmcd -= 1;
        }
    }
}
