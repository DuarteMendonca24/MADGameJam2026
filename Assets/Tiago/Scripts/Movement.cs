using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    private Vector2 movement;

    private Vector2 externalForce = new Vector2(0.0f, 0.0f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = 0;
        movement.y = 0;
        if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.y = -1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            movement.y = 1;
        }
    }

    void FixedUpdate()
    {
        if (movement.x != 0) { movement.y = 0; }
        rb.linearVelocity = (movement.normalized * moveSpeed) + externalForce;


        // slowly decay impulses (fake friction)
        externalForce = Vector2.Lerp(externalForce, Vector2.zero, 5f * Time.fixedDeltaTime);
        // Considering the external force is a one time event, we reset it
        // if(externalForce != Vector2.zero) { externalForce = Vector2.zero; }


    }

    public void AddExternalForce(Vector2 force)
    {
        externalForce = force;
    }
}
