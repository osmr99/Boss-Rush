#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using TMPro;

namespace Omar
{
    public class OmarLaserBeam : MonoBehaviour
    {
        [SerializeField] BossScript bossScript;
        [SerializeField] GameObject beam;
        [SerializeField] GameObject finalBeam;
        [SerializeField] AudioClipCollection sounds;
        [SerializeField] OmarNums correctAnswersCount;
        [SerializeField] float finalBeamRate;
        [SerializeField] GameObject winTrigger;
        [SerializeField] AudioClipCollection clickSound;
        [SerializeField] GameObject mashing;
        GameObject[] imSorry;
        GameObject here;
        float randomFloat;
        bool doItNow = false;
        bool playerCanFightBack = false;
        float strengthMultiplier;
        bool gamepadConnected;

        private void Start()
        {
            if (Gamepad.current != null)
                gamepadConnected = true;
        }


        private void Update()
        {
            if(playerCanFightBack)
            {
                if (gamepadConnected)
                {
                    if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow) || Gamepad.current.rightShoulder.wasPressedThisFrame))
                    {
                        SoundEffectsManager.instance.PlayAudioClip(clickSound.clips[0], false);
                        finalBeam.transform.localScale -= new Vector3(0, 0, 0.2f * strengthMultiplier);
                    }
                        
                    else if (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow) || Gamepad.current.leftShoulder.wasPressedThisFrame))
                    {
                        SoundEffectsManager.instance.PlayAudioClip(clickSound.clips[0], false);
                        finalBeam.transform.localScale -= new Vector3(0, 0, 0.2f * strengthMultiplier);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow)))
                    {
                        SoundEffectsManager.instance.PlayAudioClip(clickSound.clips[0], false);
                        finalBeam.transform.localScale -= new Vector3(0, 0, 0.2f * strengthMultiplier);
                    }
                    else if (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow)))
                    {
                        SoundEffectsManager.instance.PlayAudioClip(clickSound.clips[0], false);
                        finalBeam.transform.localScale -= new Vector3(0, 0, 0.2f * strengthMultiplier);
                    }
                }

                if (finalBeam.transform.localScale.z < 0.5f)
                {
                    winTrigger.SetActive(true);
                }
            }

        }

        public void StartBeam()
        {
            StartCoroutine(Beaming());
        }

        public void StartFinalBeam()
        {
            StartCoroutine(Ulti());
        }

        IEnumerator Beaming()
        {
            randomFloat = Random.Range(2.5f, 4f);
            //Debug.Log("Laser beam delay: " + randomFloat);
            bossScript.SetDelayFloat(randomFloat);
            SoundEffectsManager.instance.PlayAudioClip(sounds.clips[0], true);
            imSorry = FindObjectsOfType<GameObject>();
            foreach (GameObject go in imSorry)
            {
                if (go.name == "New Game Object" && go.GetComponent<AudioSource>() != null)
                {
                    if (go.GetComponent<AudioSource>().clip == sounds.clips[0])
                    {
                        here = go;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(randomFloat);
            imSorry = new GameObject[0];
            here.GetComponent<AudioSource>().volume = 0;
            beam.SetActive(true);
            SoundEffectsManager.instance.PlayAudioClip(sounds.clips[1], true);
            beam.transform.DOScaleZ(17.5f, 3);
            yield return new WaitForSeconds(3.5f);
            beam.transform.localScale = new Vector3(2,2,0.01f);
            beam.SetActive(false);
            bossScript.ChangeStateToIdle();
        }

        public void StopLaserAttack()
        {
            beam.SetActive(false);
            imSorry = FindObjectsOfType<GameObject>();
            foreach (GameObject go in imSorry)
            {
                if (go.name == "New Game Object" && go.GetComponent<AudioSource>() != null)
                {
                    if (go.GetComponent<AudioSource>().clip == sounds.clips[0])
                    {
                        here = go;
                        here.GetComponent<AudioSource>().volume = 0;
                        break;
                    }
                }
            }
            foreach (GameObject go in imSorry)
            {
                if (go.name == "New Game Object" && go.GetComponent<AudioSource>() != null)
                {
                    if (go.GetComponent<AudioSource>().clip == sounds.clips[1])
                    {
                        here = go;
                        here.GetComponent<AudioSource>().volume = 0;
                        break;
                    }
                }
            }
            StopAllCoroutines();
        }

        IEnumerator Ulti()
        {
            bossScript.TeleportsForUlti();
            SoundEffectsManager.instance.PlayAudioClip(sounds.clips[2], true);
            yield return new WaitForSeconds(3);
            finalBeam.SetActive(true);
            SoundEffectsManager.instance.PlayAudioClip(sounds.clips[3], true);
            switch (correctAnswersCount.myInt)
            {
                case 0:
                    strengthMultiplier = 1;
                    break;
                case 1:
                    strengthMultiplier = 1.2f;
                    break;
                case 2:
                    strengthMultiplier = 1.4f;
                    break;
                case 3:
                    strengthMultiplier = 1.6f;
                    break;
                case 4:
                    strengthMultiplier = 1.8f;
                    break;
                case 5:
                    strengthMultiplier = 2;
                    break;
                case 6:
                    strengthMultiplier = 2.25f;
                    break;
                case 7:
                    strengthMultiplier = 2.5f;
                    break;
                case 8:
                    strengthMultiplier = 3;
                    break;
                case 9:
                    strengthMultiplier = 4;
                    break;
                case 10:
                    strengthMultiplier = 5;
                    break;
            }
            doItNow = true;
            yield return new WaitForSeconds(4);
            bossScript.canTakeDamage(true);
            playerCanFightBack = true;
            mashing.SetActive(true);

        }

        private void FixedUpdate()
        {
            if(doItNow)
            {
                finalBeam.transform.localScale += new Vector3 (0, 0, finalBeamRate);
            }
        }
    }
}