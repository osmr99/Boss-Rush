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
    public class OmarStartButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] OmarMainMenu menu;
        [SerializeField] float addedScale;
        [SerializeField] float time;
        //int strength = 10;
        //int vibrato = 50;
        //int randomness = 50;
        float originalScale;

        // Start is called before the first frame update
        void Start()
        {
            //button = GetComponent<Button>();
            originalScale = transform.localScale.x;
        }

        public void OnPointerEnter(PointerEventData eventData) // Why won't this work???
        {
            Debug.Log(eventData.pointerClick);
            Debug.Log("aa");
            if (button.enabled)
                transform.DOScale(originalScale + addedScale, time);
        }

        public void OnPointerExit(PointerEventData eventData) // Why won't this work???
        {
            Debug.Log(eventData.pointerClick);
            Debug.Log("ee");
            if (button.enabled)
                transform.DOScale(originalScale, time);
        }

        private void OnMouseEnter()
        {
            Debug.Log("ii");
            if (button.enabled)
                transform.DOScale(originalScale + addedScale, time);
        }

        private void OnMouseExit()
        {
            Debug.Log("oo");
            if (button.enabled)
                transform.DOScale(originalScale, time);
        }

        public void LoadGame()
        {
            //transform.DOShakePosition(0.5f, strength, vibrato, randomness);
            menu.LoadGame();
        }
    }
}