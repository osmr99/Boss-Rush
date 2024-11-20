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
    public class BGM : MonoBehaviour
    {
        AudioSource source;
        Vector3 scaleChange = new Vector3(1.1f, 1.1f, 1.1f);
        [SerializeField] float bgmSecondsDelay;
        [SerializeField] List<Image> allUI;
        [SerializeField] OmarNumsArray numsArray;
        [SerializeField] OmarPlayerData playerData;
        float tempNum;

        void OnEnable()
        {
            StopAllCoroutines();
            numsArray.currentBeat = -1;
            numsArray.time = 0;
            if(numsArray.startPos == 0)
                numsArray.lastPlayedIndex = -1;
            if(numsArray.resetMarkers)
            {
                numsArray.resetMarkers = false;
                numsArray.nums = new float[1000];
            }
            source = GetComponent<AudioSource>();
            StartCoroutine(StartBGM());
            //Time.maximumDeltaTime = 0.05f;
        }

        // Update is called once per frame
        void Update()
        {
            //if (source.isPlaying)
            //{

            //numsArray.time = Mathf.Round((numsArray.time * 100)) / 100.0f;
            //}
            if(playerData.UIAnim)
            {
                numsArray.time = source.time;
                UIBeatHere();

                if (Input.GetKeyDown(KeyCode.V)) // Manual Beat set
                {
                    numsArray.nums[numsArray.lastPlayedIndex + 1] = numsArray.time;
                    numsArray.lastPlayedIndex++;
                    PerformUIAnim();
                }
            }


            //if (Gamepad.current.bButton.wasPressedThisFrame) // Manual Beat set
            //{
                //numsArray.nums[numsArray.lastPlayedIndex + 1] = numsArray.time;
                //numsArray.lastPlayedIndex++;
                //PerformUIAnim();
            //}



            if (Input.GetKeyDown(KeyCode.C)) // Freeze
            {
                if (Time.timeScale != 0)
                {
                    tempNum = numsArray.timeScale;
                    numsArray.timeScale = 0;
                }
                else
                {
                    numsArray.timeScale = tempNum;
                }
            }

            Time.timeScale = numsArray.timeScale;
            source.pitch = numsArray.timeScale;
        }

        IEnumerator StartBGM()
        {
            yield return new WaitForSeconds(bgmSecondsDelay);
            source.Play();
            if(numsArray.startPos != 0)
            {
                source.time = numsArray.startPos;
                numsArray.time = numsArray.startPos;
            }
        }

        void PerformUIAnim()
        {
            foreach (Image image in allUI)
            {
                image.transform.localScale = scaleChange;
                image.transform.DOScale(1, 0.2f).ForceInit();
            }
        }

        void UIBeatHere()
        {
            if (numsArray.time >= numsArray.nums[numsArray.lastPlayedIndex + 1] && numsArray.time < numsArray.nums[numsArray.lastPlayedIndex + 2])
            {
                PerformUIAnim();
                numsArray.lastPlayedIndex++;
            }
        }

        //void UIOnBeat()
        //{
        //if(numsArray.currentBeat > 0)
        //if (numsArray.time % numsArray.currentBeat == 0)
        //PerformUIAnim();
        //}
    }
}
