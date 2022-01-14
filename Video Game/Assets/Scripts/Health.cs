using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
    public bool runUI = true;
    public Slider healthBar;
    public Image colorBar;
    public Gradient color;
    public Slider armorBar;
    public float smoothStep;
    [Space(10)]
    public GameObject deadFx;

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
            Stats.OnKill((maxHealth * (canRegen ? regenSpeed : 0f) * 2) + maxArmor);
            Destroy((GameObject) Instantiate(deadFx, transform.position, transform.rotation), 3f);
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
        if(!runUI) return;
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
