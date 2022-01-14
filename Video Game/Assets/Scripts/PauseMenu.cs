using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject ui;
    public GameObject shopPrompt;
    public GameObject shop;
    public GameObject pauseButton;
    public GameObject blur;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !ShopMenu.inShop) {
            Toggle();
        }
    }

    public void Toggle() {
        // paused is still set to old state here
        blur.SetActive(!paused);
        ui.SetActive(!paused);
        shop.SetActive(false);
        shopPrompt.SetActive(false);
        pauseButton.SetActive(paused);
        Time.timeScale = paused ? 1f : 0f;
        // now set paused to new state
        paused = !paused;
    }

    public void LoadMenu() {

    } 

    public void Quit() {
        
    }
}
