#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Rendering;


namespace Omar
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] GameObject screenFade;
        [SerializeField] Canvas playerUICanvas;
        [SerializeField] Canvas bossUICanvas;
        [SerializeField] BGM omarBGM;
        // Start is called before the first frame update
        void Start()
        {
            bossUICanvas.sortingOrder = -1;
            playerUICanvas.sortingOrder = -1;
            omarBGM.enabled = false;
            screenFade.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                LoadGame();
        }

        public void LoadGame()
        {
            image.enabled = false;
            screenFade.SetActive(true);
            omarBGM.enabled = true;
            bossUICanvas.sortingOrder = 0;
            playerUICanvas.sortingOrder = 0;
        }
    }
}
