using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh
{
    public List<Vertex> vertices;
    public List<Edge> edges;
    public List<Edge> stretchEdges;
    public List<Edge> bendEdges;

    public Mesh(List<GameObject> unityVts, int dimX, int dimY, float edgeLen)
    {
        edgeLen *= 1f;
        vertices = new List<Vertex>();
        edges = new List<Edge>();
        stretchEdges = new List<Edge>();
        bendEdges = new List<Edge>();
        InitVertices(unityVts);
        InitEdges(dimX, dimY, edgeLen);
        Debug.Log("Vertices Number: " + vertices.Count.ToString());
        Debug.Log("Edges Number: " + edges.Count.ToString());
        Debug.Log("Stretch Edges: " + stretchEdges.Count.ToString());
        Debug.Log("Bend Edges: " + bendEdges.Count.ToString());
    }

    ~Mesh()
    {
        Vertex v;
        Edge e;
        for (int i = edges.Count - 1; i >= 0; i--)
        {
            e = edges[i];
            edges.Remove(e);
        }
        for (int i = stretchEdges.Count - 1; i >= 0; i--)
        {
            e = stretchEdges[i];
            stretchEdges.Remove(e);
        }
        for (int i = bendEdges.Count - 1; i >= 0; i--)
        {
            e = bendEdges[i];
            bendEdges.Remove(e);
        }
        e = null;
        for (int i = vertices.Count - 1; i >= 0; i--)
        {
            v = vertices[i];
            vertices.Remove(v);
        }
        v = null;
    }

    public void InitVertices(List<GameObject> unityVts)
    {
        for(int i = 0; i < unityVts.Count; i++)
        {
            Vertex v = new Vertex(unityVts[i]);
            vertices.Add(v);
        }
    }

    public Edge MakeEdge(Vertex v1, int v2idx, float length)
    {
        Vertex v2 = vertices[v2idx];
        Edge e = new Edge(v1, v2, length);
        return e;
    }

    public void InitEdges(int nx, int ny, float edgeLen)
    {
        float stretchLen = edgeLen * Mathf.Sqrt(2f);
        float bendLen = edgeLen * 2f;
        for(int i = 0; i < vertices.Count; i++)
        {
            int j;
            Vertex thisVert = vertices[i];
            int vx = i % nx;
            int vy = i / ny;
            if(vx + 1 < nx)
            {
                j = i + 1;
                edges.Add(MakeEdge(thisVert, j, edgeLen));
            }
            if (vy + 1 < ny)
            {
                j = i + nx;
                edges.Add(MakeEdge(thisVert, j, edgeLen));
                if (vx + 1 < nx)
                {
                    j = i + nx + 1;
                    stretchEdges.Add(MakeEdge(thisVert, j, stretchLen));
                }
                if (vx - 1 >= 0)
                {
                    j = i + nx - 1;
                    stretchEdges.Add(MakeEdge(thisVert, j, stretchLen));
                }
            }
            if(vx + 2 < nx)
            {
                j = i + 2;
                bendEdges.Add(MakeEdge(thisVert, j, bendLen));
            }
            if(vy + 2 < ny)
            {
                j = i + (2 * nx);
                bendEdges.Add(MakeEdge(thisVert, j, bendLen));
            }
        }
    }


}
