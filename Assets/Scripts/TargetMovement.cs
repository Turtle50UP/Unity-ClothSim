using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public float speed;
    public float multiplier;
    Rigidbody rb;
    public Vector3 initPos;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        this.transform.position = initPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.sm.simOn)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = speed * Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = speed * Vector3.left;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.velocity = speed * Vector3.back;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = speed * Vector3.right;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                rb.velocity = speed * Vector3.up;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                rb.velocity = speed * Vector3.down;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.velocity *= multiplier;
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                this.transform.position = initPos;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
