using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollider : MonoBehaviour
{
    /* Orthogonal to world axes boxes...
     */
    Vector3 maxCorner
    {
        get
        {
            return this.transform.localScale * .5f + this.transform.position;
        }
    }
    Vector3 minCorner
    {
        get
        {
            return this.transform.localScale * -.5f + this.transform.position;
        }
    }
    float error = 0.0000001f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 ToBoxSpace(Vector3 v)
    {
        Vector3 shiftedV = v - this.transform.position;
        return new Vector3(shiftedV.x / this.transform.localScale.x, shiftedV.y / this.transform.localScale.y, shiftedV.z / this.transform.localScale.z);
    }

    public bool IsColliding(Vector3 collidingPoint)
    {
        if(collidingPoint.x >= minCorner.x && collidingPoint.x <= maxCorner.x)
        {
            if (collidingPoint.y >= minCorner.y && collidingPoint.y <= maxCorner.y)
            {
                if (collidingPoint.z >= minCorner.z && collidingPoint.z <= maxCorner.z)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsCollidingSurface(Vector3 collidingPoint)
    {
        if (collidingPoint.x >= minCorner.x && collidingPoint.x <= maxCorner.x)
        {
            if (collidingPoint.y >= minCorner.y && collidingPoint.y <= maxCorner.y)
            {
                return (collidingPoint.z > minCorner.z - error && collidingPoint.z < minCorner.z + error) || (collidingPoint.z > maxCorner.z - error && collidingPoint.z < maxCorner.z + error);
            }
        }
        if (collidingPoint.x >= minCorner.x && collidingPoint.x <= maxCorner.x)
        {
            if (collidingPoint.z >= minCorner.z && collidingPoint.z <= maxCorner.z)
            {
                return (collidingPoint.y > minCorner.y - error && collidingPoint.y < minCorner.y + error) || (collidingPoint.y > maxCorner.y - error && collidingPoint.y < maxCorner.y + error);
            }
        }
        if (collidingPoint.z >= minCorner.z && collidingPoint.z <= maxCorner.z)
        {
            if (collidingPoint.y >= minCorner.y && collidingPoint.y <= maxCorner.y)
            {
                return (collidingPoint.x > minCorner.x - error && collidingPoint.x < minCorner.x + error) || (collidingPoint.x > maxCorner.x - error && collidingPoint.x < maxCorner.x + error);
            }
        }
        return false;
    }

    public Vector3 GetNorm(Vector3 collidingPoint)
    {
        Vector3 ncp = ToBoxSpace(collidingPoint);
        Vector3 absncp = new Vector3(Mathf.Abs(ncp.x), Mathf.Abs(ncp.y), Mathf.Abs(ncp.z));
        if (absncp.x >= absncp.y && absncp.x > absncp.z)
        {
            return Vector3.right * Mathf.Sign(ncp.x);
        }
        else if (absncp.y >= absncp.x && absncp.y > absncp.z)
        {
            return Vector3.up * Mathf.Sign(ncp.y);
        }
        else
        {
            return Vector3.forward * Mathf.Sign(ncp.z);
        }
    }

    public Vector3 GetNorm(Vertex v)
    {
        return GetNorm(v.prevPos);
    }

    public Vector3 GetTanPos(Vector3 collidingPoint, Vector3 colNorm)
    {
        float newComp;
        if (colNorm.x != 0)
        {
            newComp = this.transform.position.x + (this.transform.localScale.x / 2f * colNorm.x);
            return ((-Vector3.Dot(collidingPoint, colNorm) * colNorm) + collidingPoint) + newComp * Vector3.right;
        }
        else if(colNorm.y != 0)
        {
            newComp = this.transform.position.y + (this.transform.localScale.y / 2f * colNorm.y);
            return ((-Vector3.Dot(collidingPoint, colNorm) * colNorm) + collidingPoint) + newComp * Vector3.up;
        }
        else
        {
            newComp = this.transform.position.z + (this.transform.localScale.z / 2f * colNorm.z);
            return ((-Vector3.Dot(collidingPoint, colNorm) * colNorm) + collidingPoint) + newComp * Vector3.forward;
        }
    }
}
