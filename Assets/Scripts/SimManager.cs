using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimManager : MonoBehaviour
{
    public GameManager gm;
    public bool simOn;
    const float delayT = .5f;
    bool simJustStopped;
    bool makeRep;
    float startT;

    public int selectedQuality;
    int maxQualities;
    public float mass;
    public float dm;
    public Vector2 massRange;
    public float structurek1;
    public float structurek2;
    public float sheark1;
    public float sheark2;
    public float bendk1;
    public float bendk2;
    public float dk;
    public Vector2 structurek1Range;
    public Vector2 structurek2Range;
    public Vector2 sheark1Range;
    public Vector2 sheark2Range;
    public Vector2 bendk1Range;
    public Vector2 bendk2Range;
    public float g;
    public float dg;
    public Vector2 gravityRange;
    public float dt;
    public float ddt;
    public Vector2 dtRange;
    public float kmu;
    public float smu;
    public float dmu;
    public Vector2 kmuRange;
    public Vector2 smuRange;

    const float staticThresh = 0.02f;

    public int selectedIntegrator;
    int integratorNum;

    public bool isEulerian;
    public int simLoops;
    public int simLoopsHi;
    public int simLoopsLo;
    public float weightBound;
    public float dw;
    public Vector2 weightRange;

    public SphereCollider[] sphereColliders;
    public BoxCollider[] boxColliders;

    // Start is called before the first frame update
    void Start()
    {
        integratorNum = 4;
        maxQualities = 13;
        selectedQuality = 0;
        selectedIntegrator = 1;
        simJustStopped = false;
        makeRep = false;
        isEulerian = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            simOn = !simOn;
            if (!simOn)
            {
                simJustStopped = true;
            }
            else
            {
                makeRep = true;
            }
        }

        if (simOn)
        {
            if (makeRep)
            {
                gm.ms.mm.MakeRep();
                makeRep = false;
                Debug.Log("Simulation Start");
            }
            if (isEulerian)
            {
                DoEulerian(gm.ms.mm.mesh);
            }
            else
            {
                DoLagrangian(gm.ms.mm.mesh);
            }
            gm.ms.mm.UpdateRepPhys();
        }
        else
        {
            if (simJustStopped)
            {
                gm.ms.mm.DeleteRep();
                gm.ms.mm.ShiftMesh();
                simJustStopped = false;
                Debug.Log("Simulation End");
            }
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    startT = Time.time;
                    IncrementQuantity();
                }
                if (Input.GetKeyDown(KeyCode.G))
                {
                    startT = Time.time;
                    DecrementQuantity();
                }

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if(selectedIntegrator < integratorNum - 1)
                    {
                        selectedIntegrator++;
                    }
                    else
                    {
                        selectedIntegrator = 0;
                    }
                }
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        if (selectedIntegrator > 0)
                        {
                            selectedIntegrator--;
                        }
                        else
                        {
                            selectedIntegrator = integratorNum - 1;
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    if (selectedQuality < maxQualities - 1)
                    {
                        selectedQuality++;
                    }
                    else
                    {
                        selectedQuality = 0;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    if (selectedQuality > 0)
                    {
                        selectedQuality--;
                    }
                    else
                    {
                        selectedQuality = maxQualities - 1;
                    }
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    isEulerian = !isEulerian;
                    if (isEulerian)
                    {
                        selectedIntegrator = 1;
                    }
                    else
                    {
                        selectedIntegrator = 2;
                    }
                }
            }
            if (Input.anyKey)
            {
                if (Input.GetKey(KeyCode.T) && (Time.time - startT) > delayT)
                {
                    IncrementQuantity();
                }
                if (Input.GetKey(KeyCode.G) && (Time.time - startT) > delayT)
                {
                    DecrementQuantity();
                }
            }
        }
    }

    void IncrementQuantity()
    {
        switch (selectedQuality)
        {
            case 0:
                if (mass < massRange.y)
                {
                    mass += dm;
                    mass = Mathf.Round(mass / dm) * dm;
                }
                break;
            case 1:
                if (structurek1 < structurek1Range.y)
                {
                    structurek1 += dk;
                    structurek1 = Mathf.Round(structurek1 / dk) * dk;
                }
                break;
            case 2:
                if (structurek2 < structurek2Range.y)
                {
                    structurek2 += dk;
                    structurek2 = Mathf.Round(structurek2 / dk) * dk;
                }
                break;
            case 3:
                if (sheark1 < sheark1Range.y)
                {
                    sheark1 += dk;
                    sheark1 = Mathf.Round(sheark1 / dk) * dk;
                }
                break;
            case 4:
                if (sheark2 < sheark2Range.y)
                {
                    sheark2 += dk;
                    sheark2 = Mathf.Round(sheark2 / dk) * dk;
                }
                break;
            case 5:
                if (bendk1 < bendk1Range.y)
                {
                    bendk1 += dk;
                    bendk1 = Mathf.Round(bendk1 / dk) * dk;
                }
                break;
            case 6:
                if (bendk2 < bendk2Range.y)
                {
                    bendk2 += dk;
                    bendk2 = Mathf.Round(bendk2 / dk) * dk;
                }
                break;
            case 7:
                if (g < gravityRange.y)
                {
                    g += dg;
                    g = Mathf.Round(g / dg) * dg;
                }
                break;
            case 8:
                if (dt < dtRange.y)
                {
                    dt += ddt;
                    dt = Mathf.Round(dt / ddt) * ddt;
                }
                break;
            case 9:
                if (kmu < kmuRange.y)
                {
                    kmu += dmu;
                    kmu = Mathf.Round(kmu / dmu) * dmu;
                }
                break;
            case 10:
                if (smu < smuRange.y)
                {
                    smu += dmu;
                    smu = Mathf.Round(smu / dmu) * dmu;
                }
                break;
            case 11:
                if (simLoops < simLoopsHi)
                {
                    simLoops++;
                }
                break;
            case 12:
                if (weightBound < weightRange.y)
                {
                    weightBound += dw;
                    weightBound = Mathf.Round(weightBound / dw) * dw;
                }
                break;
            default:
                break;
        }
    }

    void DecrementQuantity()
    {
        switch (selectedQuality)
        {
            case 0:
                if (mass > massRange.x)
                {
                    mass -= dm;
                    mass = Mathf.Round(mass / dm) * dm;
                }
                break;
            case 1:
                if (structurek1 > structurek1Range.x)
                {
                    structurek1 -= dk;
                    structurek1 = Mathf.Round(structurek1 / dk) * dk;
                }
                break;
            case 2:
                if (structurek2 > structurek2Range.x)
                {
                    structurek2 -= dk;
                    structurek2 = Mathf.Round(structurek2 / dk) * dk;
                }
                break;
            case 3:
                if (sheark1 > sheark1Range.x)
                {
                    sheark1 -= dk;
                    sheark1 = Mathf.Round(sheark1 / dk) * dk;
                }
                break;
            case 4:
                if (sheark2 > sheark2Range.x)
                {
                    sheark2 -= dk;
                    sheark2 = Mathf.Round(sheark2 / dk) * dk;
                }
                break;
            case 5:
                if (bendk1 > bendk1Range.x)
                {
                    bendk1 -= dk;
                    bendk1 = Mathf.Round(bendk1 / dk) * dk;
                }
                break;
            case 6:
                if (bendk2 > bendk2Range.x)
                {
                    bendk2 -= dk;
                    bendk2 = Mathf.Round(bendk2 / dk) * dk;
                }
                break;
            case 7:
                if (g > gravityRange.x)
                {
                    g -= dg;
                    g = Mathf.Round(g / dg) * dg;
                }
                break;
            case 8:
                if (dt > dtRange.x)
                {
                    dt -= ddt;
                    dt = Mathf.Round(dt / ddt) * ddt;
                }
                break;
            case 9:
                if (kmu > kmuRange.x)
                {
                    kmu -= dmu;
                    kmu = Mathf.Round(kmu / dmu) * dmu;
                }
                break;
            case 10:
                if (smu > smuRange.x)
                {
                    smu -= dmu;
                    smu = Mathf.Round(smu / dmu) * dmu;
                }
                break;
            case 11:
                if (simLoops > simLoopsLo)
                {
                    simLoops--;
                }
                break;
            case 12:
                if (weightBound > weightRange.x)
                {
                    weightBound -= dw;
                    weightBound = Mathf.Round(weightBound / dw) * dw;
                }
                break;
            default:
                break;
        }
    }

    void ApplyGravity(Mesh m)
    {
        foreach(Vertex v in m.vertices)
        {
            v.force += Vector3.down * g * mass;
        }
    }

    void CalculateSpringForce(Edge e, float k1, float k2)
    {
        Vector3 dp = e.v1.position - e.v2.position;
        Vector3 dv = e.v1.velocity - e.v2.velocity;
        if(dp.magnitude == 0)
        {
            Debug.Log("loldivby0");
        }
        Vector3 springForce = (-((k1 * (dp.magnitude - e.l)) + (k2 * (Vector3.Dot(dv, dp) / dp.magnitude))) / dp.magnitude) * dp;
        e.v1.force += springForce;
        e.v2.force -= springForce;
    }

    void ApplySpringForce(Mesh m)
    {
        foreach(Edge e in m.edges)
        {
            CalculateSpringForce(e, structurek1, structurek2);
        }
        foreach (Edge e in m.stretchEdges)
        {
            CalculateSpringForce(e, sheark1, sheark2);
        }
        foreach (Edge e in m.bendEdges)
        {
            CalculateSpringForce(e, bendk1, bendk2);
        }
    }

    void ApplyNormalForce(Mesh m)
    {
        foreach(Vertex v in m.vertices)
        {
            foreach(SphereCollider sc in sphereColliders)
            {
                if (sc.IsColliding(v.position) || sc.IsCollidingSurface(v.position))
                {
                    Vector3 forceNorm = sc.CollNormal(v.prevPos);
                    float normMag = Mathf.Abs(Vector3.Dot(v.force, forceNorm));
                    v.force += normMag * forceNorm;
                    Vector3 forceDir;
                    if (selectedIntegrator == 2)
                    {
                        forceDir = (v.position - v.prevPos).normalized;
                        if (staticThresh > (v.position - v.prevPos).magnitude)
                        {
                            v.force += -smu * forceDir * normMag;
                        }
                        else
                        {
                            v.force += -kmu * forceDir * normMag;
                        }
                    }
                    else
                    {
                        forceDir = v.velocity.normalized;
                        if (staticThresh > v.velocity.magnitude)
                        {
                            v.force += -smu * forceDir * normMag;
                        }
                        else
                        {
                            v.force += -kmu * forceDir * normMag;
                        }
                    }
                }
            }
            foreach (BoxCollider bc in boxColliders)
            {
                if (bc.IsColliding(v.position) || bc.IsCollidingSurface(v.position))
                {
                    Vector3 forceNorm = bc.GetNorm(v);
                    float normMag = Mathf.Abs(Vector3.Dot(v.force, forceNorm));
                    v.force += normMag * forceNorm;
                    Vector3 forceDir;
                    if (selectedIntegrator == 2)
                    {
                        forceDir = (v.position - v.prevPos).normalized;
                        if (staticThresh > (v.position - v.prevPos).magnitude)
                        {
                            v.force += -smu * forceDir * normMag;
                        }
                        else
                        {
                            v.force += -kmu * forceDir * normMag;
                        }
                    }
                    else
                    {
                        forceDir = v.velocity.normalized;
                        if (staticThresh > v.velocity.magnitude)
                        {
                            v.force += -smu * forceDir * normMag;
                        }
                        else
                        {
                            v.force += -kmu * forceDir * normMag;
                        }
                    }
                }
            }
        }
    }

    void SemiImplicit(Vertex v)
    {
        v.velocity += v.force / mass * dt;
        v.prevPos = v.position;
        v.position += v.velocity * dt;
        v.force = Vector3.zero;
    }

    void Verlet(Vertex v)
    {
        v.acc = 2 * v.position - v.prevPos + (v.force / mass) * dt * dt;
        v.prevPos = v.position;
        v.position = v.acc;
        v.acc = Vector3.zero;
        v.force = Vector3.zero;
    }

    void ForwardEuler(Vertex v)
    {
        v.position += v.velocity * dt;
        v.velocity += v.force / mass * dt;
        v.force = Vector3.zero;
    }

    void Leapfrog(Vertex v)
    {
        v.position = v.position + v.velocity * dt + .5f * v.force / mass * dt * dt;
        v.velocity = v.velocity + .5f * ((v.prevForce / mass) + (v.force / mass)) * dt;
        v.prevForce = v.force;
        v.force = Vector3.zero;
    }

    void Integrate(Mesh m)
    {
        foreach(Vertex v in m.vertices)
        {
            switch (selectedIntegrator)
            {
                case 0:
                    ForwardEuler(v);
                    break;
                case 1:
                    SemiImplicit(v);
                    break;
                case 2:
                    Verlet(v);
                    break;
                case 3:
                    Leapfrog(v);
                    break;
                default:
                    SemiImplicit(v);
                    break;
            }
            Collide(v);
        }
    }

    void Collide(Vertex v)
    {
        SphereCollide(v);
        BoxCollide(v);
    }

    void SphereCollide(Vertex v)
    {
        foreach (SphereCollider sc in sphereColliders)
        {
            if (sc.IsColliding(v.position))
            {
                Vector3 sphNorm = sc.CollNormal(v.position);
                v.position = sc.GetTanPos(v.position);
                Vector3 normVelocity = -Vector3.Project(v.velocity, sphNorm);
                v.velocity += normVelocity;
            }
        }
    }

    void BoxCollide(Vertex v)
    {
        foreach(BoxCollider bc in boxColliders)
        {
            if (bc.IsColliding(v.position))
            {
                Vector3 boxNorm = bc.GetNorm(v.position);
                v.position = bc.GetTanPos(v.position, boxNorm);
                Vector3 normVelocity = -Vector3.Project(v.velocity, boxNorm);
                v.velocity += normVelocity;
            }
        }
    }

    void ConstrainEdge(Edge e)
    {
        Vector3 delP = e.v2.position - e.v1.position;
        Vector3 dx1 = weightBound * (delP.magnitude - e.l) * delP / delP.magnitude;
        e.v1.acc += dx1;
        e.v2.acc -= dx1;
    }

    void Constrain(Mesh m)
    {
        for (int i = 0; i < simLoops; i++)
        {
            foreach (Vertex v in m.vertices)
            {
                v.acc = Vector3.zero;
            }
            foreach (Edge e in m.edges)
            {
                ConstrainEdge(e);
            }
            foreach (Edge e in m.stretchEdges)
            {
                ConstrainEdge(e);
            }
            foreach (Edge e in m.bendEdges)
            {
                ConstrainEdge(e);
            }
            foreach (Vertex v in m.vertices)
            {
                v.position += v.acc;
                v.acc = Vector3.zero;
            }
        }
        foreach(Vertex v in m.vertices)
        {
            Collide(v); 
        }
    }

    void DoEulerian(Mesh m)
    {
        ApplyGravity(m);
        ApplySpringForce(m);
        ApplyNormalForce(m);
        Integrate(m);
    }

    void DoLagrangian(Mesh m)
    {
        ApplyGravity(m);
        ApplyNormalForce(m);
        Integrate(m);
        Constrain(m);
    }
}
