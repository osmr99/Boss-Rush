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
        string path;
        [SerializeField] float music;
        [SerializeField] float sfx;
        

        void Awake()
        {
            path = Application.persistentDataPath + "/playerData.json";
            if(File.Exists(path))
            {
                Debug.Log("yes");
                string saveText = File.ReadAllText(path);
                SaveData playerData = JsonUtility.FromJson<SaveData>(saveText);
                music = playerData.musicVolume;
                sfx = playerData.SFXVolume;
            }
            else
            {
                Debug.Log("no");
                SaveAudio(6,9);
                string saveText = File.ReadAllText(path);
                SaveData playerData = JsonUtility.FromJson<SaveData>(saveText);
                music = playerData.musicVolume;
                sfx = playerData.SFXVolume;
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
            playerLogic.enabled = true;
            screenFade.SetActive(true);
            omarBGM.enabled = true;
            bossUICanvas.sortingOrder = 0;
            playerUICanvas.sortingOrder = 0;
        }

        public void SaveAudio(float mus, float sfx)
        {
            SaveData sd = new SaveData();

            sd.musicVolume = mus;
            sd.SFXVolume = sfx;

            string jsonText = JsonUtility.ToJson(sd);
            File.WriteAllText(path, jsonText);
        }

        [System.Serializable]
        public class SaveData
        {
            public float musicVolume;
            public float SFXVolume;
        }
    }
}
