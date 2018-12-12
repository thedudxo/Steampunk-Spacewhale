using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour {

    public static bool paused = false;
    public GameObject pauseMenu;
    public Slider mouseSensitivity;
    public GameObject sensitivityNo;
    public AudioMixer footsteps;
    public static PauseMenu instance;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) {
                Resume();
            } else {
                Paused();
            }
        }
	}

    public void Paused() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }

    public void MouseSensitivity() {
        Debug.Log(CamRotations.Instance.turnSpeed);
        CamRotations.Instance.turnSpeed = mouseSensitivity.value;
        sensitivityNo.GetComponent<Text>().text = "" + CamRotations.Instance.turnSpeed;
    }

    public void QuitGame() {
        Debug.Log("Quit Game"); 
        Application.Quit();
    }

    public void FootstepAudio (float volume) {
        footsteps.SetFloat("FootstepAudio", volume);
    }

    public void Fullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }
}
