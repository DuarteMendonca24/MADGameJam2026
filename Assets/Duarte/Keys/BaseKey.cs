using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public enum KeyType
{
    None,
    Explode,
    Hole,
    Range,
    Throw,
    Teleport,
    Spawner,
    Goal
}

[RequireComponent(typeof(Collider2D))]
public class BaseKey : MonoBehaviour
{
    
    // The player layer to check to detect collisions
    [SerializeField] string playerLayerName;

    [Header("Key Base Configurations")]
    // The key unique id should correspond to one of the keyboard keys
    [SerializeField] string keyID;
    [SerializeField] KeyType keyType;
    [SerializeField] float keyLerpTargetIncrement;
    [SerializeField] float keyLerpDuration;

    [Header("Key Hole Configurations")]
    [SerializeField] float fallingDownSpeed;
    [SerializeField] float fallingDownDuration;

    [Header("Key Range Configurations")]
    [SerializeField] List<Vector2> raycastDirections;
    [SerializeField] float raycastDistance;

    private Collider2D collider;
    private int playerLayerIndex;

    private Vector2 keyInitialPosition;

    private Vector2 teleportPosition;


    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        playerLayerIndex = LayerMask.NameToLayer(playerLayerName);

        transform.GetChild(0).GetComponent<Canvas>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;

        keyInitialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (keyType == KeyType.Range) { EmitRaycasts(); }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (keyType == KeyType.None || keyType == KeyType.Spawner)
        {
            StartCoroutine(LerpKeyPosition(true));
            return;
        }

        if (keyType == KeyType.Goal) { StartCoroutine(LerpKeyPosition(true)); }

        if (collision.gameObject.layer == playerLayerIndex)
        {
            Movement playerMovement = collision.gameObject.GetComponent<Movement>();
            switch (keyType)
            {
                case KeyType.Throw:
                    ImpulsePlayer(collision.gameObject);
                    playerMovement.Die();
                    break;
                case KeyType.Hole:
                    playerMovement.FallDown(100, GetComponent<SpriteRenderer>().sortingOrder);
                    StartCoroutine(PlayerDeathDelay(playerMovement));
                    StartCoroutine(FallDown());
                    break;
                case KeyType.Range:
                    playerMovement.Die();
                    break;
                case KeyType.Explode:
                    playerMovement.Die();
                    break;
                case KeyType.Teleport:
                    playerMovement.Teleport(teleportPosition);
                    break;
                case KeyType.Goal:
                    playerMovement.InvokeGoalReached();
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (keyType == KeyType.None || keyType == KeyType.Spawner)
        {
            StartCoroutine(LerpKeyPosition(false));
        }
    }


    private void ImpulsePlayer(GameObject player)
    {
        Movement playerMovement = player.GetComponent<Movement>();
        playerMovement.AddExternalForce(new Vector2(0, 30));
    }


    private IEnumerator FallDown()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fallingDownDuration)
        {
            transform.position += Vector3.down * fallingDownSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;

            yield return null; // wait one frame
        }
    }

    private IEnumerator PlayerDeathDelay(Movement playerMovement)
    {
        yield return new WaitForSeconds(1.0f);
        playerMovement.Die();
    }

    private void EmitRaycasts()
    {
        foreach (Vector2 direction in raycastDirections)
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, 1 << playerLayerIndex);
            Debug.DrawLine(transform.position, transform.position + (Vector3)(direction * raycastDistance), Color.red, 1.0f);

            if (hit)
            {
                print("Player Hit by Raycast");
            }



        }
    }

    private IEnumerator LerpKeyPosition(bool isPressed)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition;

        if (isPressed)
        {
            targetPosition = new Vector2(startPosition.x, startPosition.y + keyLerpTargetIncrement);
        }
        else
        {
            targetPosition = keyInitialPosition;

        }

        while (time < keyLerpDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / keyLerpDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    public string GetKeyID() { return keyID; }
    public void SetKeyID(string keyID) { this.keyID = keyID; }

    public KeyType GetKeyType() { return keyType; }

    public void SetTeleportPosition(Vector2 position) { teleportPosition = position; }

    public void SetKeyType(KeyType type) { keyType = type; }

    public void ResetPosition()
    {
        transform.position = keyInitialPosition;
    }
}
