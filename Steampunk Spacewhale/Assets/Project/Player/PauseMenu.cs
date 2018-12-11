using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool paused = false;
    public GameObject pauseMenu;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) {
                Resume();
            } else {
                Paused();
            }
        }
	}

    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }

    public void Paused() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        paused = true;
    }

    public void QuitGame() {
        Debug.Log("Quit Game"); 
        Application.Quit();
    }
}
