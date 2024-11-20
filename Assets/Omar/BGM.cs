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
        [SerializeField] float beat;
        bool startTheTicks = false;
        [SerializeField] NumsArray numsArray;
        float tempNum;

        // Start is called before the first frame update
        void OnEnable()
        {
            StopAllCoroutines();
            startTheTicks = false;
            numsArray.currentBeat = -1;
            numsArray.tick = 0;
            if(numsArray.startPos == 0)
                numsArray.lastPlayedIndex = -1;
            source = GetComponent<AudioSource>();
            StartCoroutine(StartBGM());
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.V)) // Manual Beat set
            {
                numsArray.nums[numsArray.lastPlayedIndex + 1] = numsArray.tick;
                if (numsArray.tick > 634)
                    numsArray.lastPlayedIndex++;
                foreach (Image image in allUI)
                {
                    image.transform.localScale = scaleChange;
                    image.transform.DOScale(1, 0.2f).ForceInit();
                }
            }

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

            AnimUI();

            if (source.isPlaying)
            {
                if (startTheTicks && source.time > 0)
                {
                    StartCoroutine(StartTick());
                    startTheTicks = false;
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
                numsArray.tick = numsArray.startPos * 20;
            }
            startTheTicks = true;
            
        }

        //yield return new WaitForSeconds(0.04703f);
        IEnumerator StartTick()
        {
            numsArray.tick++;
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(StartTick());
        }

        void AnimUI()
        {
            /*switch(tick)
            {
                case 55:
                    PerformUIAnim();
                    break;
                case 120:
                    PerformUIAnim();
                    break;
                case 145:
                    PerformUIAnim();
                    break;
                case 165:
                    PerformUIAnim();
                    break;

            }*/
            if (numsArray.tick <= 643)
            {
                UIBeatHere();
            }
            else if (numsArray.tick > 643 && numsArray.tick <= 1240)
            {
                UIOnBeat();
                if (numsArray.tick == 635)
                    numsArray.currentBeat = 12;
                else if (numsArray.tick == 837)
                    numsArray.currentBeat = 6;
            }
            else if (numsArray.tick > 1240) // My favorite part 1
            {
                UIBeatHere();
                UIOnBeat();
            }
            else if (numsArray.tick > 2823 && numsArray.tick <= 3219) // Chill
            {

            }
            else if (numsArray.tick > 3219 && numsArray.tick <= 4011) // 2nd Drop
            {

            }
            else if (numsArray.tick > 4011) // Ending
            {

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
            if (numsArray.tick == numsArray.nums[numsArray.lastPlayedIndex + 1])
            {
                PerformUIAnim();
                numsArray.lastPlayedIndex++;
            }
        }

        void UIOnBeat()
        {
            if(numsArray.currentBeat > 0)
                if (numsArray.tick % numsArray.currentBeat == 0)
                    PerformUIAnim();
        }
    }
}
