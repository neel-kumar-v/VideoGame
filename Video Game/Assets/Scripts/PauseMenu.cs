using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject ui;
    public GameObject pauseButton;
    public GameObject blur;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Toggle();
        }
    }

    public void Toggle() {
        blur.SetActive(!paused);
        ui.SetActive(!paused);
        pauseButton.SetActive(paused);
        Time.timeScale = paused ? 1f : 0f;
        paused = !paused;
    }

    public void LoadMenu() {

    } 

    public void Quit() {
        
    }
}
