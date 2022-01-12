using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Paramaters")]
    public float health;
    public float maxHealth;
    public float armor;
    public float maxArmor;
    public float regenSpeed;
    public bool canRegen;

    [Header("Unity Setup")]
    public Slider healthBar;
    public Image colorBar;
    public Gradient color;
    [Space(10)]
    public Slider armorBar;
    [Space(10)]
    public float smoothStep;

    bool dead;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth; 
        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        armorBar.maxValue = maxArmor;
        armorBar.value = armor;
    }

    // Update is called once per frame
    void Update()
    {
        Regen();
        UpdateUI();
    }
    public void ApplyDamage(float damage) {
        if(armor > 0) {
            if(armor < damage) {
                damage -= armor;
                armor = 0f;
            } else if(armor > damage) {
                armor -= damage;
                damage = 0f;
            } else {
                armor = 0f;
                damage = 0f;
            }
        }
        health -= damage;
        if(health <= 0) {
            dead = true;
            UpdateUI();
            Destroy(gameObject);
            // TODO Animations
        }
    }

    public void Regen(){
        if(health >= maxHealth) {
            health = maxHealth;
            return;
        }
        if(canRegen) {
            health += regenSpeed * Time.deltaTime;
        }
    }

    public void UpdateUI() {
        if(dead) {
            healthBar.value = health;
            armorBar.value = armor;
            return;
        }
        if(healthBar.value != health) {
            healthBar.value = Mathf.Lerp(healthBar.value, health, smoothStep * Time.deltaTime);
        }
        colorBar.color = color.Evaluate(healthBar.value);
        if(armorBar.value != armor) {
            armorBar.value = Mathf.Lerp(armorBar.value, armor, smoothStep * Time.deltaTime);
        }
    }
}
