#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

namespace Omar
{
    public class OmarTeleport : MonoBehaviour
    {
        [SerializeField] NavMeshAgent agent;
        [SerializeField] Transform _transform;
        [SerializeField] BossScript bossScript;
        [SerializeField] AudioClipCollection sounds;
        float randomX;
        float randomZ;
        int randomNum;

        public void Teleport()
        {
            StartCoroutine(Teleporting());
        }

        IEnumerator Teleporting()
        {
            agent.baseOffset = 0.47f;
            agent.enabled = false;
            randomX = Random.Range(-13, 13);
            randomZ = Random.Range(-13, 13);
            _transform.position = new Vector3(randomX, 1.5f, randomZ);
            randomNum = Random.Range(0, sounds.clips.Length);
            SoundEffectsManager.instance.PlayAudioClip(sounds.clips[randomNum], true);
            agent.enabled = true;
            yield return new WaitForSeconds(1.5f);
            bossScript.ChangeStateToIdle();
        }

        public void TeleportForUlti()
        {
            agent.enabled = false;
            _transform.position = new Vector3(-13, 1.5f, 13);
            agent.enabled = true;
        }
    }
}