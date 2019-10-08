using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Vertex v1;
    public Vertex v2;
    public float l;

    public Edge(Vertex vtx1, Vertex vtx2, float length)
    {
        v1 = vtx1;
        v2 = vtx2;
        l = length;
    }

    ~Edge()
    {
        v1 = null;
        v2 = null;
    }
}
