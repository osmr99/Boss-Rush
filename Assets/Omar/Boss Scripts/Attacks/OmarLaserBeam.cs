#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

namespace Omar
{
    public class OmarLaserBeam : MonoBehaviour
    {
        [SerializeField] BossScript bossScript;
        [SerializeField] OmarHUDAnim hudAnim;
        [SerializeField] GameObject beam;
        [SerializeField] GameObject finalBeam;
        [SerializeField] AudioClipCollection sounds;
        [SerializeField] OmarNums correctAnswersCount;
        [SerializeField] float finalBeamRate;
        [SerializeField] GameObject winTrigger;
        [SerializeField] AudioClipCollection clickSound;
        [SerializeField] GameObject mashing;
        [SerializeField] RawImage boom;
        [SerializeField] float shakingPower;
        GameObject[] imSorry;
        GameObject here;
        float randomFloat;
        bool doItNow = false;
        bool playerCanFightBack = false;
        float strengthMultiplier;
        bool gamepadConnected;
        bool dead = false;
        float randomX;
        float randomY;

        private void Start()
        {
            if (Gamepad.current != null)
                gamepadConnected = true;
        }


        void Update()
        {
            if(playerCanFightBack && dead == false)
            {
                ShakingAnim();
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

                if (finalBeam.transform.localScale.z < 0.25f)
                {
                    mashing.SetActive(false);
                    winTrigger.SetActive(true);
                }
            }
        }

        void ShakingAnim()
        {
            randomX = Random.Range(0 + shakingPower, 0 - shakingPower);
            randomY = Random.Range(0 + shakingPower, 0 - shakingPower);
            mashing.transform.localPosition = new Vector2(randomX, -319 + randomY);
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
            bossScript.DoCameraShake();
            beam.transform.localScale = new Vector3(2, 2, 0.01f);
            beam.transform.DOScaleZ(22.5f, 3);
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
            bossScript.DoCameraShake();
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

        public void ItGoes()
        {
            StartCoroutine(Boom());
        }

        IEnumerator Boom()
        {
            yield return new WaitForSeconds(0.1f);
            hudAnim.HidePlayerHUD();
            mashing.SetActive(false);
            SoundEffectsManager.instance.PlayAudioClip(sounds.clips[4], true);
            for(int i = 0; i < 11; i++)
            {
                yield return new WaitForSeconds(0.25f);
                boom.color = new Color(1, 1, 1, (float)i / 10);
            }
            bossScript.TeleportBack();
            yield return new WaitForSeconds(7);
            bossScript.FailedTheUlti();
        }

        public void Died()
        {
            dead = true;
            imSorry = FindObjectsOfType<GameObject>();
            foreach (GameObject go in imSorry)
            {
                if (go.name == "New Game Object" && go.GetComponent<AudioSource>() != null)
                {
                    if (go.GetComponent<AudioSource>().clip == sounds.clips[3])
                    {
                        here = go;
                        here.GetComponent<AudioSource>().volume = 0;
                        break;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if(doItNow && dead == false)
            {
                finalBeam.transform.localScale += new Vector3 (0, 0, finalBeamRate);
            }
        }
    }
}