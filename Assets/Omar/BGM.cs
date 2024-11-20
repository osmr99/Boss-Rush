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
        [SerializeField] float zawarudo;
        float bgmTimeElapsed;
        //[SerializeField] int tick; // 20 ticks = 1 second
        [SerializeField] float beat;
        //[SerializeField] int[] markers = new int[20]; //56, 105, 128, 154
        bool startTheTicks = false;
        //int index = 0;
        [SerializeField] NumsArray numsArray;
        float tempNum;

        // Start is called before the first frame update
        void OnEnable()
        {
            StopAllCoroutines();
            startTheTicks = false;
            numsArray.currentBeat = 12;
            numsArray.tick = 0;
            numsArray.lastPlayedIndex = -1;
            source = GetComponent<AudioSource>();
            StartCoroutine(StartBGM());
        }

        // Update is called once per frame
        void Update()
        {


            if (Input.GetKeyDown(KeyCode.V))
            {
                foreach (Image image in allUI)
                {
                    image.transform.localScale = scaleChange;
                    image.transform.DOScale(1, 0.2f).ForceInit();
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
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

            if (Input.GetKeyDown(KeyCode.V))
            {
                numsArray.nums[numsArray.lastPlayedIndex + 1] = numsArray.tick;
            }

                if (source.isPlaying)
            {
                AnimUI();
                //bgmTimeElapsed = Mathf.Round(source.time * 10.0f) * 0.1f;
                //if (bgmTimeElapsed > source.time - 0.05f)
                    //tick++;
                bgmTimeElapsed = source.time;
                //Debug.Log(bgmTimeElapsed);

                if (bgmTimeElapsed > 0 && startTheTicks)
                {
                    StartCoroutine(StartTick());
                    startTheTicks = false;
                }
                //Debug.Log(source.time.ToString("F1"));
                
            }

            Time.timeScale = numsArray.timeScale;
            source.pitch = numsArray.timeScale;
        }

        IEnumerator StartBGM()
        {
            yield return new WaitForSeconds(bgmSecondsDelay);
            source.Play();
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
            if(numsArray.tick <= 640)
            {
                UIBeatHere();
            }
            else if(numsArray.tick > 640 && numsArray.tick <= 2823)
            {
                UIOnBeat();
                if (numsArray.tick == 641)
                    numsArray.currentBeat = 12;
                else if (numsArray.tick == 840)
                    numsArray.currentBeat = 6;
                else if (numsArray.tick == 840)
                    numsArray.currentBeat = 6;
            }
            else if(numsArray.tick > 2823 && numsArray.tick <= 3219) // Chill
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
            if (numsArray.tick % numsArray.currentBeat == 0)
                PerformUIAnim();
        }
    }
}
