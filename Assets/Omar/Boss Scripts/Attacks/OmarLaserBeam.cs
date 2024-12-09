#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace Omar
{
    public class OmarLaserBeam : MonoBehaviour
    {
        [SerializeField] BossScript bossScript;
        [SerializeField] GameObject beam;
        [SerializeField] AudioClipCollection sounds;
        GameObject[] imSorry;
        GameObject here;
        float randomFloat;

        public void StartBeam()
        {
            StartCoroutine(Beaming());
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
    }
}