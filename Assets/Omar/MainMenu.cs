#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Rendering;
using UnityEngine.Audio;
using System.IO;
using static Unity.VisualScripting.Member;
using System;


namespace Omar
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] GameObject screenFade;
        [SerializeField] Canvas playerUICanvas;
        [SerializeField] Canvas bossUICanvas;
        [SerializeField] BGM omarBGM;
        [SerializeField] PlayerLogic playerLogic;
        [SerializeField] OmarPlayerData playerPrefs;
        string path;
        AudioSource source;


        void Awake()
        {
            path = Application.persistentDataPath + "/omarBossPlayerData.json";
            if(File.Exists(path))
            {
                Debug.Log("yes");
                string saveText = File.ReadAllText(path);
                SaveData playerData = JsonUtility.FromJson<SaveData>(saveText);
                playerPrefs.musicVol = playerData.musicVol;
                playerPrefs.sfxVol = playerData.sfxVol;
            }
            else
            {
                Debug.Log("no");
                SaveAudio(0.5f,0.5f, true);
                string saveText = File.ReadAllText(path);
                SaveData playerData = JsonUtility.FromJson<SaveData>(saveText);
                playerPrefs.musicVol = playerData.musicVol;
                playerPrefs.sfxVol = playerData.sfxVol;
                playerPrefs.UIAnim = playerData.UIAnim;
            }

            
        }

        // Start is called before the first frame update
        void Start()
        {
            bossUICanvas.sortingOrder = -1;
            playerUICanvas.sortingOrder = -1;
            omarBGM.enabled = false;
            screenFade.SetActive(false);
            playerLogic.enabled = false;
            source = FindAnyObjectByType<AudioSource>();
            //StaticInputManager.input.Disable();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                LoadGame();

            if (Input.GetKeyDown(KeyCode.X))
                RestartScene();
        }

        public void LoadGame()
        {
            image.enabled = false;
            playerLogic.enabled = true;
            screenFade.SetActive(true);
            omarBGM.enabled = true;
            bossUICanvas.sortingOrder = 0;
            playerUICanvas.sortingOrder = 0;
        }

        public void RestartScene()
        {
            source.Stop();
            image.enabled = true;
            bossUICanvas.sortingOrder = -1;
            playerUICanvas.sortingOrder = -1;
            omarBGM.enabled = false;
            screenFade.SetActive(false);
            playerLogic.enabled = false;
        }

        public void SaveAudio(float mus, float sfx, bool ui)
        {
            SaveData sd = new SaveData();

            sd.musicVol = mus;
            sd.sfxVol = sfx;
            sd.UIAnim = ui;

            string jsonText = JsonUtility.ToJson(sd);
            File.WriteAllText(path, jsonText);
        }

        //[System.Serializable] I don't think it's required apparently
        public class SaveData
        {
            public float musicVol;
            public float sfxVol;
            public bool hasWon;
            public int deaths;
            public bool UIAnim;
        }
    }
}
