using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public delegate void LetterGoalReached();
    public LetterGoalReached letterGoalReached;

    public delegate void PlayerDied();
    public PlayerDied playerDied;

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    
    private Vector2 movement;
    private Vector2 externalForce = new Vector2(0.0f, 0.0f);
    
    private BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;

    private bool blockMovement = false;
    public bool isDead = false;

    private int originalSortingOrder;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        originalSortingOrder = spriteRenderer.sortingOrder;
    }

    void Update()
    {
        movement.x = 0;
        movement.y = 0;

        if (blockMovement ) { return; }
        if (isDead) {  return; }

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

        // Slowly decay impulses (fake friction)
        externalForce = Vector2.Lerp(externalForce, Vector2.zero, 5f * Time.fixedDeltaTime);
    }

    // Applies an external force for the movement, used to simulate
    // and impulse
    public void AddExternalForce(Vector2 force)
    {
        externalForce = force;
    }

    public void Die()
    {
        isDead = true;
        playerDied?.Invoke();
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

    public void StopFallDown()
    {
        rb.gravityScale = 0.0f;
        spriteRenderer.sortingOrder = originalSortingOrder;
        collider.enabled = true;
        blockMovement = false;
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

    public void InvokeGoalReached()
    {
        letterGoalReached?.Invoke();
        print("Word invoke");
    }

    public void Respawn()
    {
        isDead = false;
        rb.linearVelocity = Vector2.zero;
    }
}
