using TMPro;
using UnityEngine;

enum KeyType
{
    None,
    Explode,
    Hole,
    Range,
    Spawner
}

[RequireComponent(typeof(Collider2D))]
public class BaseKey : MonoBehaviour
{
    // The player layer to check to detect collisions
    [SerializeField] LayerMask playerLayer;

    [Header("Key Configurations")]
    // The key unique id should correspond to one of the keyboard keys
    [SerializeField] string keyID;
    [SerializeField] KeyType keyType;

    Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = keyID;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (keyType == KeyType.None || keyType == KeyType.Spawner) return;

        if (collision.gameObject.layer == playerLayer)
        {
            switch (keyType)
            {
                case KeyType.Explode:
                    break;
                case KeyType.Hole:
                    break;
                case KeyType.Range:
                    break;
            }
            print("Key collided with player");
        }
    }
}
