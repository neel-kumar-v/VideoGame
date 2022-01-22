using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsStatsParser : MonoBehaviour
{
    public BulletStats bulletStats;

    [Space(10)]
    public Text gunName;
    public Text buy;

    [Space(10)]
    public Text dmg;
    public Text reload;
    public Text bSpeed;
    public Text pierce;
    public Text explode;
    public Text eDmg;
    public Text eRad;
    public Text weight;

    [Space(10)]
    public Text info;


    void Start() {
        Bullet b = bulletStats.bullet;
        // Accessible Attributes
            // float reload;
            // float speed;
            // float damage;
            // bool explode;
            // float explosionDamage;
            // float explosionRadius;
            // float weight;

        gunName.text = bulletStats.gunName;
        buy.text = "$" + bulletStats.price.ToString();

        dmg.text = "Damage: " + b.damage.ToString();
        reload.text = "Reload Time: " + b.reload.ToString();
        bSpeed.text = "Bullet Speed: " + b.speed.ToString();
        pierce.text = "Pierce: " + b.pierce.ToString();
        
        explode.text = "Explosive: " + (b.explode ? "Yes" : "No");

        if(b.explode) {
            eDmg.text = "Explosive Damage: " + b.explosionDamage.ToString();
            eRad.text = "Explosion Radius: " + b.explosionRadius.ToString();
        } else {
            eDmg.gameObject.SetActive(false);
            eRad.gameObject.SetActive(false);
        }

        weight.text = "Weight: " + b.weight.ToString();

        info.text = bulletStats.gunInfo;
    }

    
}
