using UnityEngine;

public class BladeDestruction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hello");
        if (collision.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            print("BLADE NO PLAYER");
            collision.gameObject.GetComponent<Movement>().Die();
        }
    }
}
