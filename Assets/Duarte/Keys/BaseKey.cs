using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    [SerializeField] string playerLayerName;

    [Header("Key Base Configurations")]
    // The key unique id should correspond to one of the keyboard keys
    [SerializeField] string keyID;
    [SerializeField] KeyType keyType;

    [Header("Key Hole Configurations")]
    [SerializeField] float fallingDownSpeed;
    [SerializeField] float fallingDownDuration;

    Collider2D collider;
    int playerLayerValue;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        playerLayerValue = LayerMask.NameToLayer(playerLayerName);
    }

    private void Start()
    {
        transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = keyID;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (keyType == KeyType.None || keyType == KeyType.Spawner) return;

        if (collision.gameObject.layer == playerLayerValue)
        {
            switch (keyType)
            {
                case KeyType.Explode:
                    ImpulsePlayer(collision.gameObject);
                    break;
                case KeyType.Hole:
                    StartCoroutine(FallDown());
                    break;
                case KeyType.Range:
                    break;
            }
        }
    }


    private void ImpulsePlayer(GameObject player)
    {
        print("Impulse");
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


}
