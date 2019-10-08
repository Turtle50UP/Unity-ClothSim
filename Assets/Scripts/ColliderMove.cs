using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderMove : MonoBehaviour
{
    public int axis;
    Rigidbody rb;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (axis)
        {
            case 0:
                rb.velocity = Vector3.up * speed * Mathf.Sin(Time.fixedTime);
                break;
            default:
                break;
        }
    }
}
