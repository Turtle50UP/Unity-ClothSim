using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* MeshManager: Handles Traits of the Mesh
 */
public class MeshManager : MonoBehaviour
{
    public int dimX;
    public int dimY;
    public float delta;
    public Vector3 axis1;
    public Vector3 axis2;
    public GameObject vertexPrefab;
    public GameObject edgePrefab;
    public GameObject shearPrefab;
    public GameObject bendPrefab;
    public bool shearOn;
    public bool bendOn;
    public Vector3 initPos;

    public int totalVertices
    {
        get
        {
            return dimX * dimY;
        }
    }

    public Mesh mesh;
    List<GameObject> vertices;
    List<GameObject> edges;
    List<GameObject> shearEdges;
    List<GameObject> bendEdges;

    private void Awake()
    {
        vertices = new List<GameObject>();
        edges = new List<GameObject>();
        shearEdges = new List<GameObject>();
        bendEdges = new List<GameObject>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    /* Mesh Alteration Functions
     */
    void DestroyMesh()
    {
        for (int i = vertices.Count - 1; i >= 0; i--)
        {
            GameObject v = vertices[i];
            vertices.Remove(v);
            Destroy(v);
        }
        for (int i = edges.Count - 1; i >= 0; i--)
        {
            GameObject e = edges[i];
            edges.Remove(e);
            Destroy(e);
        }
        for (int i = shearEdges.Count - 1; i >= 0; i--)
        {
            GameObject e = shearEdges[i];
            shearEdges.Remove(e);
            Destroy(e);
        }
        for (int i = bendEdges.Count - 1; i >= 0; i--)
        {
            GameObject e = bendEdges[i];
            bendEdges.Remove(e);
            Destroy(e);
        }
    }

    public void ReMesh()
    {
        DestroyMesh();
        MakeMesh();
    }

    GameObject MakeEdge(GameObject v1, GameObject v2, GameObject prefab)
    {
        GameObject e = Instantiate(prefab);
        UpdateEdge(e, v1, v2);
        return e;
    }

    Vector3 Centroid(EdgeManager em)
    {
        return (em.v2.transform.position + em.v1.transform.position) / 2;
    }

    void UpdateEdge(GameObject e)
    {
        EdgeManager em = e.GetComponent<EdgeManager>();
        Vector3 mid = Centroid(em);
        Vector3 axis = em.v2.transform.position - em.v1.transform.position;
        float mag = axis.magnitude;
        axis = axis.normalized;
        Quaternion q = Quaternion.FromToRotation(Vector3.up, axis);
        e.transform.localScale = new Vector3(1, mag, 1);
        e.transform.position = mid;
        e.transform.rotation = q * this.transform.rotation;
    }

    void UpdateEdge(GameObject e, GameObject v1, GameObject v2)
    {
        EdgeManager em = e.GetComponent<EdgeManager>();
        em.v1 = v1;
        em.v2 = v2;
        Vector3 mid = Centroid(em);
        Vector3 axis = em.v2.transform.position - em.v1.transform.position;
        float mag = axis.magnitude;
        axis = axis.normalized;
        Quaternion q = Quaternion.FromToRotation(Vector3.up, axis);
        e.transform.localScale = new Vector3(1, mag, 1);
        e.transform.position = mid;
        e.transform.rotation = q * this.transform.rotation;
    }

    void MakeMesh()
    {
        for (int i = 0; i < totalVertices; i++)
        {
            int numX = i % dimX;
            int numY = i / dimX;
            Vector3 vpos = initPos + (axis1 * (numX * delta)) + (axis2 * (numY * delta));
            GameObject v = Instantiate(vertexPrefab);
            v.transform.position = vpos;

            vertices.Add(v);
        }
        for(int i = 0; i < totalVertices; i++)
        {
            int numX = i % dimX;
            int numY = i / dimX;
            int j;
            GameObject v1 = vertices[i];
            GameObject v2;
            if (numX + 1 < dimX)
            {
                j = i + 1;
                v2 = vertices[j];
                edges.Add(MakeEdge(v1, v2, edgePrefab));
            }
            if (numY + 1 < dimY)
            {
                j = i + dimX;
                v2 = vertices[j];
                edges.Add(MakeEdge(v1, v2, edgePrefab));
                if (shearOn)
                {
                    if (numX + 1 < dimX)
                    {
                        j = i + dimX + 1;
                        v2 = vertices[j];
                        shearEdges.Add(MakeEdge(v1, v2, shearPrefab));
                    }
                    if (numX - 1 >= 0)
                    {
                        j = i + dimX - 1;
                        v2 = vertices[j];
                        shearEdges.Add(MakeEdge(v1, v2, shearPrefab));
                    }
                }
            }
            if (bendOn)
            {
                if (numX + 2 < dimX)
                {
                    j = i + 2;
                    v2 = vertices[j];
                    bendEdges.Add(MakeEdge(v1, v2, bendPrefab));
                }
                if (numY + 2 < dimY)
                {
                    j = i + (2 * dimX);
                    v2 = vertices[j];
                    bendEdges.Add(MakeEdge(v1, v2, bendPrefab));
                }
            }
        }
    }

    public void ShiftMesh()
    {
        for (int i = 0; i < totalVertices; i++)
        {
            int numX = i % dimX;
            int numY = i / dimX;
            Vector3 vpos = initPos + (axis1 * (numX * delta)) + (axis2 * (numY * delta));
            GameObject v = vertices[i];
            v.transform.position = vpos;
        }
        foreach (GameObject e in edges)
        {
            UpdateEdge(e);
        }
        foreach (GameObject e in bendEdges)
        {
            UpdateEdge(e);
        }
        foreach (GameObject e in shearEdges)
        {
            UpdateEdge(e);
        }
    }

    public void MakeRep()
    {
        mesh = new Mesh(vertices, dimX, dimY, delta);
        Debug.Log("Rep Made");
    }

    public void DeleteRep()
    {
        mesh = null;
    }

    public void UpdateRepPhys()
    {
        for(int i = 0; i < vertices.Count; i++)
        {
            GameObject uv = vertices[i];
            Vertex v = mesh.vertices[i];
            uv.transform.position = v.position;
        }
        foreach (GameObject e in edges)
        {
            UpdateEdge(e);
        }
        foreach (GameObject e in bendEdges)
        {
            UpdateEdge(e);
        }
        foreach (GameObject e in shearEdges)
        {
            UpdateEdge(e);
        }
    }
}
