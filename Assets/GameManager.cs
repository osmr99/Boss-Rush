using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Animator screenFade;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void RestartLevel()
    {
        StartCoroutine(HandleRestart());
    }

    IEnumerator HandleRestart()
    {
        screenFade.SetTrigger("fade to black");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void GoToNextLevel()
    {
        StartCoroutine(HandleNextLevel());
    }

    IEnumerator HandleNextLevel()
    {
        screenFade.SetTrigger("fade to black");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void ActivateHitStop(float time)
    {
        StartCoroutine(HandleHitStop(time));
    }

    IEnumerator HandleHitStop(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }
}
