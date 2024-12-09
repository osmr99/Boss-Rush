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
        bool found;
        GameObject here;
        float randomFloat;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //if(Input.GetKeyDown(KeyCode.V))
                //StartBeam();
        }

        public void StartBeam()
        {
            StartCoroutine(Beaming());
            //Debug.Log("Starting beam");
        }

        IEnumerator Beaming()
        {
            found = false;
            randomFloat = Random.Range(2.5f, 4f);
            //Debug.Log("Laser beam delay: " + randomFloat);
            bossScript.SetDelayFloat(randomFloat);
            SoundEffectsManager.instance.PlayAudioClip(sounds.clips[0], true);
            imSorry = FindObjectsOfType<GameObject>();
            foreach (GameObject go in imSorry)
            {
                if(found == false)
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
            }
            yield return new WaitForSeconds(randomFloat);
            imSorry = new GameObject[0];
            here.GetComponent<AudioSource>().volume = 0;
            beam.SetActive(true);
            SoundEffectsManager.instance.PlayAudioClip(sounds.clips[1], true);
            beam.transform.DOScaleZ(17.5f, 3);
            yield return new WaitForSeconds(3.5f);
            beam.transform.localScale = new Vector3(1,1,0.01f);
            beam.SetActive(false);
            bossScript.ChangeStateToIdle();
        }
    }
}