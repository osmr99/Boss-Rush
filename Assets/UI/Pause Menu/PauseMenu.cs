//Omar changed this script.
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject pauseScreen;
    bool gamepadConnected;
    // Start is called before the first frame update
    void Start()
    {
        if (Gamepad.current != null)
            gamepadConnected = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(gamepadConnected)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Gamepad.current.startButton.wasPressedThisFrame)
            {
                if (Time.timeScale == 1)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                if (Time.timeScale == 1)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            }
        }

    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
        //Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        //if (EditorApplication.isPlaying)
            //EditorApplication.isPlaying = false;
        //else
            Application.Quit();
    }
}
