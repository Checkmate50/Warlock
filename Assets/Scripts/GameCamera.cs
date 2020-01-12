using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    MovableObject target = null;

    protected virtual void Update()
    {
        if (target != null)
        {
            Vector3 pos = target.transform.position;
            transform.position = new Vector3(pos.x, pos.y, -10);
        }

    }

    public void addTarget(MovableObject t)
    {
        target = t;
    }

    public void removeTarget(MovableObject t)
    {
        target = null;
    }
}
