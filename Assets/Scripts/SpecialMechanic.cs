using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpecialMechanic
{
    private static LayerMask layerMask = LayerMask.GetMask("Platform");
    private static RaycastHit2D objectHit;
    public static bool Raycast(this Rigidbody2D rb, Vector2 direction, CapsuleCollider2D cap)
    {
        if (rb.isKinematic)
        {
            return false;
        }

        //float radius = 0.1f;
        float distance = 0.05f;

        objectHit = Physics2D.CapsuleCast(cap.bounds.center, cap.size, cap.direction, 0,  direction.normalized, distance, layerMask);
        //hit = Physics2D.CircleCast(rb.position, radius, direction, distance, layerMask);
        return objectHit.collider != null && objectHit.rigidbody != rb;
    }

    public static bool DotTest(this Transform trans, Transform other, Vector2 Direction)
    {
        Vector2 direction = other.position - trans.position;
        return Vector2.Dot(direction.normalized, Direction) > 0f;
    }
}
