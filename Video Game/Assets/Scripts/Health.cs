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
    [Space(10)]
    public bool isPlayer;

    public bool dead;

    PlayerController controller;
    Renderer rend;
    AIMovement enemyController;
    SphereCollider sphereCollider;
    Renderer[] cylinderRends;

    float savedArmor;
    [HideInInspector] public float startArmor;

    // Start is called before the first frame update
    void Start()
    {
        startArmor = armor;
        controller = GetComponent<PlayerController>();
        enemyController = GetComponent<AIMovement>();
        rend = GetComponent<Renderer>();
        sphereCollider = GetComponent<SphereCollider>();
        StartCoroutine(LateStart());
    }

    public IEnumerator LateStart() {
        yield return new WaitForSeconds(1f);
        cylinderRends = GetComponentsInChildren<Renderer>();
    }


    public void ResetHealth() {
        health = maxHealth; 
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        colorBar.color = color.Evaluate(healthBar.value);

        armor = startArmor;
        armorBar.maxValue = maxArmor;
        armorBar.value = armor;
    }

    // Update is called once per frame
    void Update()
    {
        if(rend.enabled) {
            if(dead) {
                SetState(true);
            }
            dead = false;
        }
        Regen();
        UpdateUI();
        // Debug.Log(healthBar.value);
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

            if(!isPlayer) {
                float formula = ((maxHealth + maxHealth * (canRegen ? regenSpeed : 0f)) + maxArmor + startArmor) * 2;
                Debug.Log(formula);
                Stats.OnKill(formula);
            }

            Destroy((GameObject) Instantiate(deadFx, transform.position, transform.rotation), 3f);

            ResetHealth();
            
            SetState(false);
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
            healthBar.value = 0f;
            armorBar.value = 0f;
        }
        if(healthBar.value != health) {
            healthBar.value = Mathf.Lerp(healthBar.value, health, smoothStep * Time.deltaTime);
        }
        colorBar.color = color.Evaluate(healthBar.value);
        if(armorBar.value != armor) {
            armorBar.value = Mathf.Lerp(armorBar.value, armor, smoothStep * Time.deltaTime);
        }
    }

    public void SetState(bool state) {
        if(enabled) {
            dead = false;
            ResetHealth();
        }
        if(controller != null) {
            controller.enabled = state;
        }
        if(enemyController != null) {
            enemyController.enabled = state;
        }
        rend.enabled = state;
        sphereCollider.enabled = state;

        foreach (Renderer cylinderRend in cylinderRends)
        {      
            cylinderRend.enabled = state;
        }
    }

    

}
