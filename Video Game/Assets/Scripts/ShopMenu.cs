using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    public static bool inShop = false;
    public GameObject[] uiElementsToRemove;
    public GameObject[] uiElementsToAdd;
    [Space(10)]
    public GameObject shopPrompt;

    public void ToggleShop() {
        foreach (GameObject ui in uiElementsToRemove)
        {
            ui.SetActive(inShop);
        }
        foreach (GameObject ui in uiElementsToAdd)
        {
            ui.SetActive(!inShop);
        }
        Time.timeScale = inShop ? 1f : 0f;
        inShop = !inShop;
    }

    public void Prompt() {
        
    }
}
