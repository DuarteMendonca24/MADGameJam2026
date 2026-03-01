using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SliderDecay : MonoBehaviour
{
    private Slider escSlider;

    private bool filling = false;
    [SerializeField] private float maxLife, damageValue;

    [SerializeField] private RectTransform bossPos, esc, smash;
    private bool isShaking = false, hasTextAppeared = false;

    private void Awake()
    {
        escSlider = GetComponent<Slider>();   
    }

    private void Start()
    {
        maxLife = escSlider.maxValue;
        damageValue = 15f;
        
        StartCoroutine(GrowText(1f, smash, false));
        StartCoroutine(GrowText(1f, esc, true));
    }

    private void Update()
    {
        if(hasTextAppeared) Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && maxLife > 0)
        {

            StartCoroutine(BossShake(1f, 1.5f));
            damageValue = SetDamage(escSlider.value);

            maxLife -= damageValue;
            escSlider.value = maxLife;

            if (escSlider.value < escSlider.maxValue && !filling)
            {
                StartCoroutine(Fillslider());
            }
        }

        else if (maxLife < 0) 
        {
            //BossDefeated
            Debug.Log("Boss Defeated");
            StopCoroutine(Fillslider());
            GameManager.Instance.UpdateGameState(GameState.EndGame);
        }
    }


    private IEnumerator Fillslider()
    {
        filling = true;
        while (escSlider.value < escSlider.maxValue && maxLife > 0)
        {
            maxLife++;
            escSlider.value = maxLife;
            yield return new WaitForSeconds(0.005f);
        }
        filling = false;
    }

    private float SetDamage(float sliderValue)
    {
        float damage;
        if (escSlider.value <= 150f && escSlider.value > 75f) damage = 12.5f;
        else if (escSlider.value <= 75f) damage = 11.5f;
        else { damage = 15f; }
            
        return damage;
    }

    public IEnumerator BossShake(float duration, float magnitude)
    {
        if (isShaking) yield break; // Prevent overlapping shakes
        isShaking = true;
        
        RectTransform bossPosInit = bossPos;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Random offset within a sphere
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            bossPos.transform.position = new Vector3(bossPos.position.x + x, bossPos.position.y + y, bossPos.position.z);

            elapsed += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        bossPos.position = bossPosInit.transform.position; // Reset position
        isShaking = false;
    }

    private IEnumerator GrowText(float duration, RectTransform textComponent, bool hasTextAppeared)
    {
        Vector3 startScale = textComponent.transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Linear interpolation of scale over time
            textComponent.transform.localScale = Vector3.Lerp(startScale, Vector3.one, elapsed / duration);
            yield return null; // Wait for next frame
        }

        textComponent.transform.localScale = Vector3.one;
        this.hasTextAppeared = hasTextAppeared;
    }
}
