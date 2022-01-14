using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public static int kills;
    public int startKills = 0;
    public int startingGold;
    public static int savedGold;
    public static int gold;
    public static int upgradesBought;
    public static int goldSpent;

    public GameObject goldText;
    Text gText;
    bool counting;

    void Start()
    {
        kills = startKills;
        gText = goldText.GetComponent<Text>();
        gText.text = gold.ToString();
        gold = startingGold;
        savedGold = startingGold;
        counting = false;
    }
    void Update() {
        if(int.Parse(gText.text) != gold && !counting) {
            StartCoroutine(Anim(1f));
        }
    }
    public static void OnKill(float num) {
        num = Mathf.Ceil(num);
        kills++;
        gold += (int) num;
        savedGold = gold;
        // animText.GetComponent<Text>().text = "+" + num.ToString();
        // StartCoroutine(Anim(0.5f));
    }
    public IEnumerator Anim(float time) {
        counting = true;
        int countGold = savedGold;
        while(countGold < gold) {
            countGold += gold/100;
            gText.text = countGold.ToString();
            yield return new WaitForSeconds(time * 0.01f);
        }
        counting = false;
    }

}
