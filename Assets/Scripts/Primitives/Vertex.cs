using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public Vector3 prevPos;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 force;
    public Vector3 acc;
    float perturb = 0.00001f;
    public Vector3 prevVel;
    public Vector3 prevForce;

    public Vertex(GameObject unityVtx)
    {
        position = unityVtx.transform.position;
        prevPos = position + Random.insideUnitSphere * perturb * 100;
        velocity = Random.insideUnitSphere * perturb;
        prevVel = velocity;
        force = Vector3.zero;
        prevForce = force;
        acc = Vector3.zero;
    }
}
