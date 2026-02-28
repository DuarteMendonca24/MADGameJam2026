using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    
    private Vector2 movement;
    private Vector2 externalForce = new Vector2(0.0f, 0.0f);
    
    private BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;


    private bool blockMovement = false;
    private bool animating = false;
    private int lastAxis = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = 0;
        movement.y = 0;

        if (blockMovement ) { return; }

        if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1;
            if (!animating)
            {
                animator.SetTrigger("Andar_Direita");
                animating = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            animator.SetTrigger("Idle_Direita");
            animating = false;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1;
            if (!animating)
            {
                animator.SetTrigger("Andar_Esquerda");
                animating = true;
            }
            
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetTrigger("Idle_Esquerda");
            animating = false;
        }

        else if (Input.GetKey(KeyCode.S))
        {
            movement.y = -1;
            if (!animating)
            {
                animator.SetTrigger("Andar_Frente");
                animating = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetTrigger("Idle_Frente");
            animating = false;
        }

        else if (Input.GetKey(KeyCode.W))
        {
            movement.y = 1;
            if (!animating)
            {
                animator.SetTrigger("Andar_Costas");
                animating = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetTrigger("Idle_Costas");
            animating = false;
        }
    }

    void FixedUpdate()
    {
        if (movement.x != 0) { movement.y = 0;} 
        rb.linearVelocity = (movement.normalized * moveSpeed) + externalForce;

        // Slowly decay impulses (fake friction)
        externalForce = Vector2.Lerp(externalForce, Vector2.zero, 5f * Time.fixedDeltaTime);
    }

    // Applies an external force for the movement, used to simulate
    // and impulse
    public void AddExternalForce(Vector2 force)
    {
        externalForce = force;
    }


    // Applies gravity and correct sorting order to fall down due
    // to a Hole key
    public void FallDown(int gravityForce, int orderingLayer)
    {
        rb.gravityScale = gravityForce;
        spriteRenderer.sortingOrder = orderingLayer;
        collider.enabled = false;
        blockMovement = true;
    }

    public void Teleport(Vector2 finalPos)
    {
        Color color = spriteRenderer.color;
        color.a = 0.0f;
        spriteRenderer.color = color;
        transform.position = finalPos;
        color.a = 1.0f;
        spriteRenderer.color = color;

    }
}
