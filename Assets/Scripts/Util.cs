using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Util
{
    public static float FastDistance(Vector3 v1, Vector3 v2) {
        Vector3 newV = new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        return newV.x * newV.x + newV.y * newV.y + newV.z * newV.z;
    }
    // Given a direction towards the target, the direction the target is moving, and the speed of the object and target
    // Returns the direction the object should travel to intercept the target
    public static Vector2 CalcInterceptDir(Vector2 current, Vector2 targetMove, float targetSpeed, float projSpeed) {
        if (targetSpeed == 0 || projSpeed == 0) {
            Debug.LogError("Invalid speed for interception, cannot be 0");
            return Vector2.zero;
        }
        float angle = Mathf.Acos(Vector2.Dot(current, targetMove)
             / (current.magnitude * targetMove.magnitude));
        float D = targetSpeed / projSpeed * Mathf.Sin(angle);
        // No solution, so we give up
        if (D > .995f)
            return current;
        float dAngle = Mathf.Asin(D) * 180 / Mathf.PI;
        if (Vector3.Cross(current, targetMove).z >= 0f)
            return Quaternion.Euler(0, 0, dAngle) * current;
        return -(Quaternion.Euler(0, 0, 180 - dAngle) * current);
    }
}
