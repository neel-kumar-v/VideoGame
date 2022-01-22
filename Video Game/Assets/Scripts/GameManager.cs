using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Unity Setup")]
    public GameObject player;
        PlayerController playerMovement;
        Health playerHealth;
        Renderer playerRenderer;
        Renderer[] playerRenderers;
        SphereCollider playerSC;

    public GameObject enemy;
        AIMovement enemyMovement;
        Health enemyHealth;
        Renderer enemyRenderer;
        Renderer[] enemyRenderers;
        SphereCollider enemySC;

    public float countdown = 3f;
    public GameObject countdownObj;
    public Text tCountdownText;
    public GameObject baseBullet;

    [Header("Countries")]
    public string playerCountry;
    public string[] countries;
    string enemyCountry;
    [HideInInspector] public List<string> enemyCountries;
    public BulletStats[] usa;
    public BulletStats[] ger;
    public BulletStats[] fra;
    public BulletStats[] gb;
    public BulletStats[] rus;
    [HideInInspector] public BulletStats weapon;

    [Space(10)] public GameObject[] weaponItems;

    [Space(10)] public Text playerCountryText;
    public Text enemyCountryText;

    [Space(20)]
    [Header("Events")]
    public UnityEvent onPlayerDead;
    bool calledPlayer;
    public UnityEvent onAllEnemiesDead;
    bool calledEnemies = false;
    public GameObject winScene;

    [Space(10)]
    [Header("Scaling")]
    [Range(0f, 5f)]
    public float enemySpeedScaling;
    [Range(0f, 5f)]
    public float enemyBulletSpeedScaling;
    [Range(0f, 5f)]
    public float enemyDamageScaling;

    [HideInInspector] public int rounds;
    int gunRounds;
    BulletStats[] gunsToPickFrom;

    BulletStats[] currentCountry;

    #region GameLoop 

    public void Start() {
        weapon = (BulletStats) ScriptableObject.CreateInstance("BulletStats");
        weapon.gunName = "Base Bullet";
        playerCountry = MainMenu.countryString == null ? "France" : MainMenu.countryString;

        playerMovement = player.GetComponent<PlayerController>();
        playerHealth = player.GetComponent<Health>();
        playerRenderer = player.GetComponent<Renderer>();
        playerSC = player.GetComponent<SphereCollider>();
        playerRenderers = player.GetComponentsInChildren<Renderer>();

        enemyMovement = enemy.GetComponent<AIMovement>();
        enemyHealth = enemy.GetComponent<Health>();
        enemyRenderer = enemy.GetComponent<Renderer>();
        enemyRenderers = enemy.GetComponentsInChildren<Renderer>();
        enemySC = enemy.GetComponent<SphereCollider>();

        SetEnemyCountry();
        
        Reset();
        StartShopUI();
    }

    #region StartFunctions
    
    public void SetEnemyGun() {
        gunsToPickFrom = new BulletStats[3];

        if(enemyCountry == "USA") {
            gunsToPickFrom = usa;
        }
        if(enemyCountry == "France") {
            gunsToPickFrom = fra;
        }
        if(enemyCountry == "Germany") {
            gunsToPickFrom = ger;
        }
        if(enemyCountry == "Russia") {
            gunsToPickFrom = rus;
        }
        if(enemyCountry == "Great Britain") {
            gunsToPickFrom = gb;
        }
        enemyMovement.bullet = gunsToPickFrom[gunRounds].bulletObj;
    }

    public void SetEnemyCountry() {
        foreach (string country in countries)
        {
            if(country != playerCountry) {
                enemyCountries.Add(country);
            }
        }
    }

    #endregion StartFunctions

    public void Update() {
        if(!playerMovement.enabled && !calledPlayer) {
            onPlayerDead.Invoke();
            calledPlayer = true;
        }
        if(!enemyMovement.enabled && !calledEnemies) {
            onAllEnemiesDead.Invoke();
            StartCoroutine(DeleteAllBullets());
            calledEnemies = true;
        }
        if(playerMovement.enabled && calledPlayer) {
            calledPlayer = true;
        }
        if(enemyMovement.enabled && calledEnemies) {
            calledEnemies = false;
        }
        if(enemyCountryText.text != enemyCountry) {
            UpdateCountries();
        }
    }

    public void Win() {
        winScene.SetActive(true);
    }

    public void Reset() {

        rounds++;

        if(rounds == 6) {
            Win();
            return;
        } else {
            enemyCountry = enemyCountries[rounds - 1];
        }

        float multiplier = (float) rounds - 1;

        StartCoroutine(DeleteAllBullets());

        #region Player

        playerMovement.enabled = true;
        foreach (Renderer tempRend in playerRenderers) tempRend.enabled = true;
        playerRenderer.enabled = true;
        playerHealth.ResetHealth();
        playerSC.enabled = true;
        playerMovement.Reset();

        #endregion Player

        #region Enemy

        enemyMovement.enabled = true;        
        foreach (Renderer tempRend in enemyRenderers) tempRend.enabled = true;
        enemyRenderer.enabled = true;
        enemyHealth.startArmor = 125 * multiplier;
        enemyHealth.regenSpeed = 1f + multiplier - 1;
        enemyHealth.ResetHealth();
        enemySC.enabled = true;

        if(rounds == 1) {
            enemyMovement.bullet = baseBullet; 
        } else {
            if (rounds == 2) {
                gunRounds = 0;
            } else if (rounds == 3) {
                gunRounds = 1;
            } else if(rounds == 4 || rounds == 5) {
                gunRounds = 2;
            }
        } 

        SetEnemyGun();

        enemyMovement.Reset();
        enemyMovement.speed = enemyMovement.speed + enemySpeedScaling * multiplier;

        #endregion Enemy

        // Bullet bullet = enemyMovement.bullet.GetComponent<Bullet>();
        // bullet.damage = bullet.damage + enemyDamageScaling * multiplier;
        // bullet.speed = bullet.speed + enemyDamageScaling * multiplier;
        StartCoroutine(Countdown());
    }


    public IEnumerator Countdown() {
        tCountdownText = countdownObj.GetComponent<Text>();

        yield return new WaitForSeconds(0.5f);

        WaitForSeconds waitTime = new WaitForSeconds(1f);

        StartCoroutine(Anim("Round " + rounds.ToString()));
        yield return waitTime;

        StartCoroutine(Anim(3f.ToString()));
        yield return waitTime;

        StartCoroutine(Anim(2f.ToString()));
        yield return waitTime;

        StartCoroutine(Anim(1f.ToString()));
        yield return waitTime;

        StartCoroutine(Anim("FIGHT!"));
    }


    public IEnumerator Anim(string num) {
        tCountdownText.text = num;
        countdownObj.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        countdownObj.SetActive(false);
    }

    #endregion GameLoop

    #region HelperFunctions

    public void UpdateCountries() {
        playerCountryText.text = playerCountry;
        enemyCountryText.text = enemyCountry;
    }

    public IEnumerator DeleteAllBullets() {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        yield return new WaitForSeconds(1f);

        GameObject[] particles = GameObject.FindGameObjectsWithTag("Particles");
        foreach (GameObject particle in particles)
        {
            Destroy(particle);
        }
    }

    #endregion HelperFunctions

    #region ShopUI

    public void StartShopUI() {
        currentCountry = new BulletStats[3];

        if(playerCountry == "USA") {
            currentCountry = usa;
        }
        if(playerCountry == "France") {
            currentCountry = fra;
        }
        if(playerCountry == "Germany") {
            currentCountry = ger;
        }
        if(playerCountry == "Russia") {
            currentCountry = rus;
        }
        if(playerCountry == "Great Britain") {
            currentCountry = gb;
        }
        for (int i = 0; i < weaponItems.Length; i++)
        {
            var parser = weaponItems[i].GetComponent<BulletsStatsParser>();
            parser.bulletStats = currentCountry[i]; // sfsdf
        }
    }

    public void BuyPlayerGun(int i) {
        if(playerMovement.bullet == currentCountry[i].bulletObj) return;
        if(Stats.gold > currentCountry[i].price) {
            playerMovement.bullet = currentCountry[i].bulletObj;
            weapon = currentCountry[i];
            Stats.Spend(currentCountry[i].price);
        }
    }

    public void BuyPlayerHealth(float speed) {
        if(playerHealth.regenSpeed >= speed) return;
        int cost = 0;
        if(speed == 2f) {
            cost = 500;
        } else if (speed == 4f) {
            cost = 1000;
        } else if (speed == 8f) {
            cost = 2000;
        }
        if(Stats.gold > cost) {
            Stats.Spend(cost);
            playerHealth.regenSpeed = speed;
        }
    }

    public void BuyPlayerArmor(float armor) {
        if(playerHealth.startArmor >= armor) return;
        int cost = 0;
        if(armor == 100f) {
            cost = 500;
        } else if (armor == 250f) {
            cost = 1000;
        } else if (armor == 500f) {
            cost = 2000;
        }
        if(Stats.gold > cost) {
            playerHealth.startArmor = armor;
            Stats.Spend(cost);
        }
    }

    #endregion ShopUI



    
}
