using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    float health;
    float lerpTimer;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;

    public Image frontHealth;
    public Image backHealth;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateUI();
    }

    public void UpdateUI()
    {
        float fillFront = frontHealth.fillAmount;
        float fillBack = backHealth.fillAmount;

        float healthFraction = health / maxHealth;

        if(fillBack > healthFraction)
        {
            backHealth.color = Color.red;
            frontHealth.fillAmount = healthFraction;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;

            backHealth.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }
        if(fillFront < healthFraction)
        {
            backHealth.color = Color.green;
            backHealth.fillAmount = healthFraction;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;

            frontHealth.fillAmount = Mathf.Lerp(fillFront, backHealth.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
