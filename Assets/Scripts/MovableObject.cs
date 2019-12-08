using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour
{
    protected GameController gameController;
    protected Rigidbody2D rb;

    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update() {}
}
