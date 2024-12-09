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
            yield return new WaitForSeconds(0.1f);
            sphere.transform.DOScale(20, duration);
            yield return new WaitForSeconds(duration + 0.75f);
            sphere.transform.localScale = originalScale;
            sphere.SetActive(false);
            bossScript.ChangeStateToIdle();
        }
    }
}