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
using Unity.VisualScripting;


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
        [SerializeField] OmarNumsArray playerNumsArray;
        [SerializeField] Navigator bossNav;
        [SerializeField] NavMeshAgent bossAgent;
        [SerializeField] Damager sword;
        [SerializeField] Damager projectile;
        string path;
        string user;
        string domain;
        AudioSource source;


        void Awake()
        {
            path = Application.persistentDataPath + "/OmarBossPlayerData.json";
            user = System.Environment.UserName;
            domain = System.Environment.UserDomainName;
            if (File.Exists(path))
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
            if(playerNumsArray.timeScale == 0)
                playerNumsArray.timeScale = 1;
            bossAgent.enabled = false;
            bossNav.enabled = false;
            bossUICanvas.sortingOrder = -1;
            playerUICanvas.sortingOrder = -1;
            omarBGM.enabled = false;
            screenFade.SetActive(false);
            StaticInputManager.input.Disable();
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
            StaticInputManager.input.Enable();
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

            int randomNumber = UnityEngine.Random.Range(0, 10);
            Debug.Log(randomNumber);
            switch(randomNumber)
            {
                case 0:
                    jsonText += "\n" + user + ", what are you planning?";
                    break;
                case 1:
                    jsonText += "\n" + "First time here? @" + user;
                    break;
                case 2:
                    jsonText += "\n" + "This filename is pretty obvious isn't?";
                    break;
                case 3:
                    jsonText += "\n" + "What were you thinking??";
                    break;
                case 4:
                    jsonText += "\n" + "Oh hey! It's your data!" +
                                "\n" + "(Read only)";
                    break;
                case 5:
                    jsonText += "\n" + "\"Also try Minecraft!\"";
                    break;
                case 6:
                    jsonText += "\n" + "Hi, you are just passing by?..." +
                                "\n" + "ARE YOU?";
                    break;
                case 7:
                    jsonText += "\n" + "ngl... I'll really miss this class....";
                    break;
                case 8:
                    jsonText += "\n" + "Let me break it down for you, " + user + "." +
                                "\n" + "You can't edit this data from here.";
                    break;
                case 9:
                    jsonText += "\n" + "Hmmm, let's see here..." +
                                "\n" + "\"" + domain + "\"" +
                                "\n" + "ahahaha, you looked terrified for a sec.";
                    break;
            }

            try
            {
                File.WriteAllText(path, jsonText);
            }
            catch
            {
                Debug.LogError("Nice try "+ user + ", but the game won't work now :v");
                this.gameObject.SetActive(false);
            }
            
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
