#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace Omar
{
    public class OmarDamagerSphere : MonoBehaviour
    {
        [SerializeField] GameObject sphereOne;
        [SerializeField] GameObject sphereTwo;
        [SerializeField] GameObject sphereThree;
        [SerializeField] GameObject sphereFour;
        [SerializeField] GameObject sphereFive;
        [SerializeField] BossScript bossScript;
        [SerializeField] float duration;
        [SerializeField] float cooldown;
        Vector3 playerPos;

        // Start is called before the first frame update
        void Start()
        {
            playerPos = bossScript.GetPlayerTransform().position;
        }

        public void StartProjAttack()
        {
            StartCoroutine(DoIt());
        }

        IEnumerator DoIt()
        {
            StartCoroutine(AttackPhysics(sphereOne));
            yield return new WaitForSeconds(cooldown);
            StartCoroutine(AttackPhysics(sphereTwo));
            yield return new WaitForSeconds(cooldown);
            StartCoroutine(AttackPhysics(sphereThree));
            yield return new WaitForSeconds(cooldown);
            StartCoroutine(AttackPhysics(sphereFour));
            yield return new WaitForSeconds(cooldown);
            StartCoroutine(AttackPhysics(sphereFive));
            yield return new WaitForSeconds(2);
            bossScript.SetAnimatorBool("canProjectile", false);
            bossScript.ChangeStateToIdle();
        }

        IEnumerator AttackPhysics(GameObject p)
        {
            p.SetActive(true);
            Vector3 startPos = p.transform.position;
            yield return new WaitForSeconds(0.8f);
            playerPos = bossScript.GetPlayerTransform().position;
            p.transform.DOMove(playerPos, duration).ForceInit();
            yield return new WaitForSeconds(duration + 0.35f);
            p.transform.position = startPos;
            p.SetActive(false);
        }
    }
}