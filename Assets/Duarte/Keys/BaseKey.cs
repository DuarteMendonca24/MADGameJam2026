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

    [SerializeField] GameObject blade, FinalBattle;
    [SerializeField] private bool bladeExpeled = false;

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

    [Header("Key Throw Configurations")]
    [SerializeField] float throwForce;

    private Collider2D collider;
    private int playerLayerIndex;

    private Vector2 keyInitialPosition;

    private Vector2 teleportPosition;

    private Animator animator;

    //Audio

    [SerializeField] private AudioClip clickSound, fallImpulseSound, shurikenSound, bombSound;
    //Audio
    [SerializeField] private AudioClip FightSound;


    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        animator = transform.GetChild(1).GetComponent<Animator>();
        playerLayerIndex = LayerMask.NameToLayer(playerLayerName);

        transform.GetChild(0).GetComponent<Canvas>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;

        keyInitialPosition = transform.position;

        if (keyType == KeyType.Throw)
        {
            keyLerpTargetIncrement = 0.6f;
            keyLerpDuration = 0.15f;
        }

        animator.gameObject.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
    }

    private void FixedUpdate()
    {
        if (keyType == KeyType.Range && !bladeExpeled)
        {
            Debug.Log("raycasting");
            EmitRaycasts();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (keyType == KeyType.None || keyType == KeyType.Spawner)
        {
            StartCoroutine(LerpKeyPosition(true));
            return;
        }

        if (keyType == KeyType.Goal || keyType == KeyType.Throw) { 
            StartCoroutine(LerpKeyPosition(true)); }

        if (collision.gameObject.layer == playerLayerIndex)
        {
            Movement playerMovement = collision.gameObject.GetComponent<Movement>();
            switch (keyType)
            {
                case KeyType.Throw:
                    ImpulsePlayer(collision.gameObject);
                    StartCoroutine(PlayerDeathDelay(playerMovement));
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
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(true);
                    animator.SetTrigger("Explosion");
                    SoundFXManager.Instance.PlaySoundFXClip(bombSound, transform, 1f);
                    playerMovement.Die();
                    break;
                case KeyType.Teleport:
                    playerMovement.Teleport(teleportPosition);
                    break;
                case KeyType.Goal:
                    playerMovement.InvokeGoalReached();
                    TryEnterFinalBattle(playerMovement);
                    break;
                default:
                    break;
            }
        }
    }

    private void TryEnterFinalBattle(Movement movement)
    {
        if(GameManager.Instance.GetGameLevel() == 5)
        {
            movement.inFinalBattle = true;
            FinalBattle.SetActive(true);
            SoundManager.Instance.StopSoundClip();
            SoundManager.Instance.PlaySoundClip(FightSound, transform, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (keyType == KeyType.None || keyType == KeyType.Spawner || keyType == KeyType.Throw)
        {
            StartCoroutine(LerpKeyPosition(false));
        }
    }


    private void ImpulsePlayer(GameObject player)
    {
        Movement playerMovement = player.GetComponent<Movement>();
        playerMovement.AddExternalForce(new Vector2(0, throwForce));
    }


    private IEnumerator FallDown()
    {
        float elapsedTime = 0.0f;

        SoundFXManager.Instance.PlaySoundFXClip(fallImpulseSound, transform, 1f);

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
                bladeExpeled = true;
                GameObject go = Instantiate(blade, transform);
                go.GetComponent<Rigidbody2D>().gravityScale = 0;
                go.GetComponent<Rigidbody2D>().AddForce(direction * 1000, ForceMode2D.Force);
                SoundFXManager.Instance.PlaySoundFXClip(shurikenSound, transform, 1f);
                print("Player Hit by Raycast");
                Debug.Log(direction);
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
            
            if (keyType != KeyType.Throw)
            {
               SoundFXManager.Instance.PlaySoundFXClip(clickSound, transform, 1f);
            }
            
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

        bladeExpeled = false;
    }
}
