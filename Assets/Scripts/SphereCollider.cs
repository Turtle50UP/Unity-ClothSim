using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollider : MonoBehaviour
{
    float error = 0.0000001f;
    public float radius
    {
        get
        {
            return this.transform.localScale.x;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsColliding(Vector3 collidingPoint)
    {
        Vector3 collisionVec = collidingPoint - this.transform.position;
        float collDist = collisionVec.magnitude;
        return collDist < radius;
    }

    public bool IsCollidingSurface(Vector3 collidingPoint)
    {
        Vector3 collisionVec = collidingPoint - this.transform.position;
        float collDist = collisionVec.magnitude;
        return collDist < radius + error && collDist > radius - error;
    }

    public Vector3 CollNormal(Vector3 collidingPoint)
    {
        return (collidingPoint - this.transform.position).normalized;
    }

    public Vector3 GetTanPos(Vector3 collidingPoint)
    {
        Vector3 sphPtVec = CollNormal(collidingPoint) * radius;
        return this.transform.position + sphPtVec;
    }
}
