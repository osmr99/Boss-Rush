#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace Omar
{
    public class OmarWarcryAttack : MonoBehaviour
    {
        [SerializeField] BossScript bossScript;
        [SerializeField] GameObject sphere;
        [SerializeField] GameObject otherSphere;
        [SerializeField] AudioClipCollection sounds;
        [SerializeField] float duration;
        Vector3 originalScale;
        // Start is called before the first frame update
        void Start()
        {
            originalScale = sphere.transform.localScale;
        }

        public void PerformCry()
        {
            StartCoroutine(CryingAttack());
        }

        IEnumerator CryingAttack()
        {
            sphere.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            SoundEffectsManager.instance.PlayAudioClip(sounds.clips[0], true);
            bossScript.DoCameraShake();
            yield return new WaitForSeconds(0.1f);
            sphere.transform.DOScale(20, duration);
            yield return new WaitForSeconds(duration + 0.75f);
            sphere.transform.localScale = originalScale;
            sphere.SetActive(false);
            bossScript.ChangeStateToIdle();
        }

        public void DeathEffect()
        {
            StartCoroutine(Effect());
        }

        IEnumerator Effect()
        {
            yield return new WaitForSeconds(2.75f);
            bossScript.DoCameraShake();
            otherSphere.SetActive(true);
            otherSphere.transform.DOScale(70, 2);
            yield return new WaitForSeconds(2 + 0.75f);
            otherSphere.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            otherSphere.SetActive(false);
        }
    }
}