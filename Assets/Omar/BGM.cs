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
        [SerializeField] int tick; // 20 ticks = 1 second
        [SerializeField] float beat;
        [SerializeField] int[] markers = new int[20]; //56, 105, 128, 154
        bool startTheTicks = false;
        int index = 0;
        [SerializeField] NumsArray numsArray;

        // Start is called before the first frame update
        void OnEnable()
        {
            StopAllCoroutines();
            startTheTicks = false;
            tick = 0;
            index = 0;
            source = GetComponent<AudioSource>();
            StartCoroutine(StartBGM());
        }

        // Update is called once per frame
        void Update()
        {
            

            if(Input.GetKeyDown(KeyCode.C))
            {
                foreach(Image image in allUI)
                {
                    image.transform.localScale = scaleChange;
                    image.transform.DOScale(1, 0.2f).ForceInit();
                }
            }

            if(source.isPlaying)
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

            Time.timeScale = zawarudo;
            source.pitch = zawarudo;
        }

        IEnumerator StartBGM()
        {
            yield return new WaitForSeconds(bgmSecondsDelay);
            Debug.Log("playing music now");
            source.Play();
            startTheTicks = true;
            
        }

        //yield return new WaitForSeconds(0.04703f);
        IEnumerator StartTick()
        {
            tick++;
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
            if(tick == numsArray.nums[index])
            {
                PerformUIAnim();
                Debug.Log(index);
                index++;
            }
            if (tick > 500)
            {
                if (tick % beat == 0)
                {
                    PerformUIAnim();
                }
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
    }
}
