#pragma warning disable IDE0051
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

namespace Omar
{
    public class OmarButtons : MonoBehaviour
    {
        [SerializeField] OmarMainMenu menu;
        [SerializeField] Button button;
        [SerializeField] GameObject reset;

        public void LoadGame()
        {
            menu.LoadGame();
        }

        public void ResetAll()
        {
            reset.SetActive(true);
            menu.allowQuit = false;
            button.enabled = false;
            menu.ResetAllStats(0.25f, 0.25f, false, 0, 0, true, 6, 6);
            StartCoroutine(Cooldown());
        }

        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}