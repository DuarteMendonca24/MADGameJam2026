using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public void OnExplosionEnded()
    {
        //SpriteRenderer spriteRenderer = this.gameObject.GetComponentInParent<SpriteRenderer>();
        transform.parent.GetComponentInParent<SpriteRenderer>().enabled = true;
        transform.parent.GetChild(0).gameObject.SetActive(true);
        gameObject.SetActive(false);
        print("EXPLODE AQUI");
    }
    
}
