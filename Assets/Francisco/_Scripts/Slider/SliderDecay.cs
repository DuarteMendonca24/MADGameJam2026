using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SliderDecay : MonoBehaviour
{
    private Slider escSlider;

    private bool filling = false;
    private float maxLife, damageValue;

    private void Awake()
    {
        escSlider = GetComponent<Slider>();   
    }

    private void Start()
    {
        maxLife = escSlider.maxValue;
        damageValue = 15f;
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && maxLife > 0)
        {
            damageValue = SetDamage(escSlider.value);

            maxLife -= damageValue;
            escSlider.value = maxLife;

            if (escSlider.value < escSlider.maxValue && !filling)
            {
                StartCoroutine(Fillslider());
            }
        }

        else
        {
            //BossDefeated
            Debug.Log("Boss Defeated");
            StopCoroutine(Fillslider());
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
        if (escSlider.value <= 150f && escSlider.value > 75f) damage = 10f;
        else if (escSlider.value <= 75f) damage = 8.5f;
        else { damage = 15f; }
            
        return damage;
    }
}
