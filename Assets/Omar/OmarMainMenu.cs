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
using TMPro;
using UnityEngine.EventSystems;
using static Unity.Burst.Intrinsics.X86;


namespace Omar
{
    public class OmarMainMenu : MonoBehaviour
    {
        [SerializeField] AudioSource music;
        [SerializeField] GameObject omarCanvas;
        [SerializeField] GameObject screenFade;
        [SerializeField] Canvas playerUICanvas;
        [SerializeField] Canvas bossUICanvas;
        [SerializeField] OmarBGM omarBGM;
        [SerializeField] PlayerLogic playerLogic;
        [SerializeField] OmarPlayerData playerPrefs;
        [SerializeField] OmarColors deathCounterColors;
        [SerializeField] OmarNumsArray playerNumsArray;
        [SerializeField] Navigator bossNav;
        [SerializeField] NavMeshAgent bossAgent;
        [SerializeField] Damager sword;
        [SerializeField] Damager projectile;
        [SerializeField] TMP_Text musicText;
        [SerializeField] Slider musicSlider;
        [SerializeField] GameObject pauseMenu;
        [SerializeField] Button startButton;
        [SerializeField] TMP_Text errorText;
        [SerializeField] Toggle uiAnimToggle;
        [SerializeField] TMP_Text deathCounter;
        [SerializeField] GameObject hasWonGameObject;
        [SerializeField] TMP_Text hasWonText;
        [SerializeField] Image bossHPFillBar;
        [SerializeField] Color bossHPColor;
        Vector3 scaleChange = new Vector3(1.1f, 1.1f, 1.1f);
        string path;
        string user;
        string domain;
        string[] errorMessages = { "Um, couldn't save your prefs... that's weird (is the file read-only?)",
                                   "Ok, apparently your current prefs file is a read-only or something else...",
                                   "If you see this message, I genuinely don't know what happened or how you accomplished to " +
                                    "do this. (The default player prefs failed to create its file)"};
        void Awake()
        {
            path = Application.persistentDataPath + "/OmarBossPlayerData.json";
            user = System.Environment.UserName;
            domain = System.Environment.UserDomainName;
            if (File.Exists(path))
                try
                {
                    SavePrefs(
                    playerPrefs.musicVol,
                    playerPrefs.sfxVol,
                    playerPrefs.hasWon,
                    playerPrefs.deathsTook,
                    playerPrefs.deaths,
                    playerPrefs.UIAnim,
                    playerPrefs.meleeDmg,
                    playerPrefs.projDmg
                    );
                }
                catch
                {
                    ShowFileError(1);
                }
                
            else
                try
                {
                    SavePrefs(0.25f, 0.25f, false, 0, 0, true, 6, 3);
                }
                catch
                {
                    ShowFileError(2);
                }
            DeathCounter();
            hasWon();
            music.volume = playerPrefs.musicVol;
            musicSlider.value = music.volume;
            uiAnimToggle.isOn = playerPrefs.UIAnim;
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
            startButton.enabled = true;
            screenFade.SetActive(false);
            StaticInputManager.input.Disable();
            playerLogic.enabled = false;
            bossHPFillBar.color = bossHPColor;
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Z))
                //LoadGame();

            //if (Input.GetKeyDown(KeyCode.X))
                //RestartScene();

            //if (Input.GetKeyDown(KeyCode.B))
            //{
                //if (StaticInputManager.input.asset.enabled)
                //{
                    //StaticInputManager.input.Disable();
                    //Debug.Log("Player controls disabled");
                //}
                //else
                //{
                    //StaticInputManager.input.Enable();
                    //Debug.Log("Player controls enabled");
                //}
            //}

            if (Mouse.current.leftButton.wasReleasedThisFrame)
                if (EventSystem.current.currentSelectedGameObject?.GetComponent<Slider>())
                {
                    StopAllCoroutines();
                    StartCoroutine(MusicVolumeTest());
                }     
        }

        void ShowFileError(int index)
        {
            Debug.LogError("Error " + index + ": " + errorMessages[index]);
            errorText.text = errorMessages[index];
            errorText.gameObject.SetActive(true);
        }

        public void SetMusicVol()
        {
            music.volume = musicSlider.value;
            playerPrefs.musicVol = musicSlider.value;
        }

        IEnumerator MusicVolumeTest()
        {
            musicSlider.interactable = false;
            if (music.volume > 0)
            {
                int randomNumber = UnityEngine.Random.Range(8, 200);
                music.time = randomNumber;
                music.Play();
                for (int i = 0; i < 40; i++)
                {
                    if (i >= 2 && playerPrefs.UIAnim)
                    {
                        if (i % playerNumsArray.currentBeat == 0)
                        {
                            musicSlider.transform.localScale = scaleChange;
                            musicSlider.transform.DOScale(1, 0.2f).ForceInit();
                        }
                    }
                    if (i == 30)
                        music.DOFade(0, 1);
                    yield return new WaitForSeconds(0.1f);
                }
                music.Stop();
            }
            else
                yield return new WaitForSeconds(0.5f);
            music.volume = playerPrefs.musicVol;
            musicSlider.interactable = true;
        }

        public void LoadGame()
        {
            try
            {
                SavePrefs(
                           playerPrefs.musicVol,
                           playerPrefs.sfxVol,
                           playerPrefs.hasWon,
                           playerPrefs.deaths,
                           playerPrefs.deathsTook,
                           playerPrefs.UIAnim,
                           playerPrefs.meleeDmg,
                           playerPrefs.projDmg
                         );
            }
            catch
            {
                ShowFileError(0);
                return;
            }
            music.volume = playerPrefs.musicVol;
            errorText.gameObject.SetActive(false);
            StopAllCoroutines();
            DOTween.Clear();
            musicSlider.transform.localScale = new Vector3(1, 1, 1);
            musicSlider.interactable = true;
            startButton.enabled = false;
            bossAgent.enabled = true;
            bossNav.enabled = true;
            playerLogic.enabled = true;
            StaticInputManager.input.Enable();
            omarCanvas.SetActive(false);
            screenFade.SetActive(true);
            pauseMenu.SetActive(true);
            omarBGM.enabled = true;
            bossUICanvas.sortingOrder = 0;
            playerUICanvas.sortingOrder = 0;
            
        }

        public void RestartScene()
        {
            bossAgent.enabled = false;
            bossNav.enabled = false;
            music.Stop();
            omarCanvas.SetActive(true);
            bossUICanvas.sortingOrder = -1;
            playerUICanvas.sortingOrder = -1;
            omarBGM.enabled = false;
            screenFade.SetActive(false);
            pauseMenu.SetActive(false);
            playerLogic.enabled = false;
        }

        public void SavePrefs(float mus, float sfx, bool won, int loosesTook, int losses, bool ui, int sword, int proj)
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
            File.WriteAllText(path, jsonText);
            
        }

        public void UIToggle()
        {
            playerPrefs.UIAnim = uiAnimToggle.isOn;
        }

        public void DeathCounter()
        {
            deathCounter.text = playerPrefs.deaths.ToString();
            if(playerPrefs.deaths >= 20)
                deathCounter.color = deathCounterColors.colors[4];
            else if(playerPrefs.deaths >= 15)
                deathCounter.color = deathCounterColors.colors[3];
            else if (playerPrefs.deaths >= 10)
                deathCounter.color = deathCounterColors.colors[2];
            else if (playerPrefs.deaths >= 5)
                deathCounter.color = deathCounterColors.colors[1];
            else if (playerPrefs.deaths >= 0)
                deathCounter.color = deathCounterColors.colors[0];
        }

        public void hasWon()
        {
            if(playerPrefs.hasWon)
            {
                hasWonGameObject.SetActive(true);
                if (playerPrefs.deathsTook == 0)
                    ChangeVictoryText("deaths! WOW, is it that easy?");
                else if (playerPrefs.deathsTook == 1)
                    ChangeVictoryText("death! Very Impressive");
                else if (playerPrefs.deathsTook == 2)
                    ChangeVictoryText("deaths! Third one was the charm!");
                else if (playerPrefs.deathsTook >= 3)
                    ChangeVictoryText("deaths! Alright then!");
            }
        }

        void ChangeVictoryText(string text)
        {
            hasWonText.text = playerPrefs.deathsTook + " " + text;
        }

        //[System.Serializable] I don't think it's required apparently
        public class SaveData
        {
            public float musicVol;
            public float sfxVol;
            public bool hasWon;
            public int deathsTook;
            public int deaths;
            public bool UIAnim;
            public int meleeDmg;
            public int projDmg;
        }
    }
}
