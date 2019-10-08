using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSpawner : MonoBehaviour
{
    public GameObject meshPrefab;
    public GameManager gm;
    GameObject mesh;
    public MeshManager mm;

    public bool showShear;
    public bool showBend;
    public int initDimN;
    public int dimNX;
    public int dimNY;
    public int dimMin;
    public int dimMax;
    public GameObject clothLocation;
    public bool meshSpawned;
    public float edgeDel;
    bool meshChanged;
    public Vector3 orientation;

    Vector3 startPosition
    {
        get
        {
            return clothLocation.transform.position;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        dimNX = initDimN;
        dimNY = initDimN;

        showShear = false;
        showBend = false;
        mesh = Instantiate(meshPrefab);
        mm = mesh.GetComponent<MeshManager>();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.sm.simOn) //Cloth Simulation Currently Off, can manipulate quantities
        {
            if (Input.anyKeyDown)
            {
                int newDimX = dimNX;
                int newDimY = dimNY;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (newDimY > dimMin)
                    {
                        newDimY--;
                        newDimX--;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    if (newDimY < dimMax)
                    {
                        newDimY++;
                        newDimX++;
                    }
                }
                meshChanged = meshChanged || (newDimX != dimNX || newDimY != dimNY);
                dimNX = newDimX;
                dimNY = newDimY;

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    showShear = !showShear;
                    meshChanged = true;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    showBend = !showBend;
                    meshChanged = true;
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    if(orientation == Vector3.right)
                    {
                        orientation = Vector3.up;
                    }
                    else if(orientation == Vector3.up)
                    {
                        orientation = Vector3.forward;
                    }
                    else
                    {
                        orientation = Vector3.right;
                    }
                    meshChanged = true;
                }
                if (meshChanged)
                {
                    UpdateMesh();
                }
            }

            if(mm.initPos != startPosition)
            {
                mm.initPos = startPosition;
                mm.ShiftMesh();
            }
        }
        else //Cloth Simulation Currently On
        {
        }
    }

    void UpdateMesh()
    {
        mm.shearOn = showShear;
        mm.bendOn = showBend;
        if(orientation == Vector3.right)
        {
            mm.axis1 = Vector3.up;
            mm.axis2 = Vector3.forward;
        }
        else if(orientation == Vector3.up)
        {
            mm.axis1 = Vector3.right;
            mm.axis2 = Vector3.forward;
        }
        else
        {
            mm.axis1 = Vector3.right;
            mm.axis2 = Vector3.up;
        }
        mm.delta = edgeDel;
        mm.dimX = dimNX;
        mm.dimY = dimNY;
        mm.initPos = startPosition;

        mm.ReMesh();
        meshChanged = false;
    }
}
