using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public static int kills;
    public int startKills = 0;
    public static int gold;

    public GameObject goldText;
    Text gText;
    public GameObject animText;
    void Start()
    {
        kills = startKills;
        gText = goldText.GetComponent<Text>();
    }
    void Update() {
        Debug.Log(gold);
        gText.text = gold.ToString();
    }
    public static void OnKill(float num) {
        num = Mathf.Ceil(num);
        kills++;
        gold += (int) num;
        // animText.GetComponent<Text>().text = "+" + num.ToString();
        // StartCoroutine(Anim(0.5f));
    }
    public IEnumerator Anim(float time) {
        animText.SetActive(true);
        yield return new WaitForSeconds(time);
        animText.SetActive(false);
    }

}
