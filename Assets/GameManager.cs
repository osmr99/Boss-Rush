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
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void GoToNextLevel()
    {
        StartCoroutine(HandleNextLevel());
    }

    IEnumerator HandleNextLevel()
    {
        screenFade.SetTrigger("fade to black");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}
