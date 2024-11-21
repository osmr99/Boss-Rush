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
using UnityEngine.AI;


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
        [SerializeField] Navigator bossNav;
        [SerializeField] NavMeshAgent bossAgent;
        [SerializeField] Damager sword;
        [SerializeField] Damager projectile;
        string path;
        AudioSource source;


        void Awake()
        {
            path = Application.persistentDataPath + "/OmarBossPlayerData.json";
            if(File.Exists(path))
                SavePrefs(
                    playerPrefs.musicVol,
                    playerPrefs.sfxVol,
                    playerPrefs.hasWon,
                    playerPrefs.deaths,
                    playerPrefs.UIAnim,
                    playerPrefs.meleeDmg,
                    playerPrefs.projDmg
                    );
            else
                SavePrefs(0.5f, 0.5f, false, 0, true, 6, 3);
            sword.SetDamageAmount(playerPrefs.meleeDmg);
            projectile.SetDamageAmount(playerPrefs.projDmg);

            /*string saveText = File.ReadAllText(path);
            SaveData playerData = JsonUtility.FromJson<SaveData>(saveText);
            playerPrefs.musicVol = playerData.musicVol;
            playerPrefs.sfxVol = playerData.sfxVol;
            playerPrefs.hasWon = playerData.hasWon;
            playerPrefs.deaths = playerData.deaths;
            playerPrefs.UIAnim = playerData.UIAnim;
            playerPrefs.meleeDmg = playerData.meleeDmg;
            sword.SetDamageAmount(playerData.meleeDmg);
            playerPrefs.projDmg = playerData.projDmg;
            projectile.SetDamageAmount(playerData.projDmg);*/
        }

        // Start is called before the first frame update
        void Start()
        {
            bossAgent.enabled = false;
            bossNav.enabled = false;
            bossUICanvas.sortingOrder = -1;
            playerUICanvas.sortingOrder = -1;
            omarBGM.enabled = false;
            screenFade.SetActive(false);
            playerLogic.enabled = false;
            source = FindAnyObjectByType<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                LoadGame();

            if (Input.GetKeyDown(KeyCode.X))
                RestartScene();

            if(Input.GetKeyDown(KeyCode.B))
            {
                if(StaticInputManager.input.asset.enabled)
                {
                    StaticInputManager.input.Disable();
                    Debug.Log("Player controls disabled");
                }
                else
                {
                    StaticInputManager.input.Enable();
                    Debug.Log("Player controls enabled");
                }

            }
                
        }

        public void LoadGame()
        {
            bossAgent.enabled = true;
            bossNav.enabled = true;
            playerLogic.enabled = true;
            
            //bossNav.CalculatePathToPosition(playerLogic.GetComponent<Transform>().position);
            image.enabled = false;
            screenFade.SetActive(true);
            omarBGM.enabled = true;
            bossUICanvas.sortingOrder = 0;
            playerUICanvas.sortingOrder = 0;
            
        }

        public void RestartScene()
        {
            bossAgent.enabled = false;
            bossNav.enabled = false;
            source.Stop();
            image.enabled = true;
            bossUICanvas.sortingOrder = -1;
            playerUICanvas.sortingOrder = -1;
            omarBGM.enabled = false;
            screenFade.SetActive(false);
            playerLogic.enabled = false;
        }

        public void SavePrefs(float mus, float sfx, bool won, int losses, bool ui, int sword, int proj)
        {
            SaveData sd = new SaveData();

            sd.musicVol = mus;
            sd.sfxVol = sfx;
            sd.hasWon = won;
            sd.deaths = losses;
            sd.UIAnim = ui;
            sd.meleeDmg = sword;
            sd.projDmg = proj;

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
            public int meleeDmg;
            public int projDmg;
        }
    }
}
