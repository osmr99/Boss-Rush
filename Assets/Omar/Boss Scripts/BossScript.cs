#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Rendering;
using UnityEngine.AI;
using brolive;
using Unity.VisualScripting;
using Palmmedia.ReportGenerator.Core;
using System;


namespace Omar
{
    public class BossScript : MonoBehaviour
    {
        Bar bossBar;
        Navigator navigator;
        Transform player;
        NavMeshAgent agent;
        Animator bossAnim;
        BossStateMachine myStateMachine;
        Damageable bossDamageable;
        [SerializeField] OmarBar bossHealthBar;
        [SerializeField] OmarBar energyBar;
        [SerializeField] GameObject collisionDamager;
        [SerializeField] Damager meleeDamager;
        float bossHPPercentage;
        //[SerializeField][Range(0, 1)] float timeScale = 1;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            navigator = GetComponent<Navigator>();
            player = FindObjectOfType<PlayerLogic>().transform;
            bossAnim = GetComponent<Animator>();
            bossDamageable = GetComponent<Damageable>();


            myStateMachine = new BossStateMachine(this);

            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        void OnEnable()
        {
            bossBar = FindAnyObjectByType<Bar>();
            bossBar.SetMax(100);
            bossBar.UpdateBar(0, 100);
        }

        // Update is called once per frame
        void Update()
        {
            if(agent.enabled == true)
            {
                //Debug.Log(Vector3.Distance(player.position, transform.position));
                myStateMachine.Update(); // Elapsed timer
                //Time.timeScale = timeScale;
            }
        }

        public void GetHealthPercentageAndChangePhase()
        {
            bossHPPercentage = bossHealthBar.GetBarFill() * 100;
            //Debug.Log(bossHPPercentage + "%");
            if(bossHPPercentage == 100)
            {
                bossAnim.SetInteger("currentPhase", 1);
            }
            else if(bossHPPercentage >= 65.5f && bossHPPercentage <= 66 && GetAnimatorInt("currentPhase") == 1)
            {
                ToggleDamager(false);
                bossAnim.SetInteger("currentPhase", 2);
                myStateMachine.ChangeState(new BossHurtState(myStateMachine));
            }
            else if(bossHPPercentage >= 32.5f && bossHPPercentage <= 33 && GetAnimatorInt("currentPhase") == 2)
            {
                ToggleDamager(false);
                bossAnim.SetInteger("currentPhase", 3);
                myStateMachine.ChangeState(new BossHurtState(myStateMachine));
            }
            else if(bossHPPercentage >= 0.5f && bossHPPercentage <= 1 && bossAnim.GetBool("canUlti") == false)
            {
                ToggleDamager(false);
                bossAnim.SetBool("canUlti", true);
                myStateMachine.ChangeState(new BossHurtState(myStateMachine));
            }
        }

        public void StartCoroutineMeleeOne()
        {
            StartCoroutine(MeleeOneDelay());
        }

        public void StartCoroutineMeleeTwo()
        {
            StartCoroutine(MeleeTwoDelay());
        }

        public void StartCoroutineMeleeThree()
        {
            StartCoroutine(MeleeThreeDelay());
        }

        IEnumerator MeleeOneDelay()
        {
            yield return new WaitForSeconds(0.86f);
            ToggleDamager(true);
            yield return new WaitForSeconds(0.21f);
            ToggleDamager(false);
            yield return new WaitForSeconds(1.23f);
            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        IEnumerator MeleeTwoDelay()
        {
            yield return new WaitForSeconds(0.95f);
            ToggleDamager(true);
            yield return new WaitForSeconds(0.11f);
            ToggleDamager(false);
            yield return new WaitForSeconds(2.3f);
            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        IEnumerator MeleeThreeDelay()
        {
            yield return new WaitForSeconds(0.93f);
            ToggleDamager(true);
            yield return new WaitForSeconds(0.07f);
            ToggleDamager(false);
            yield return new WaitForSeconds(1.6f);
            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        public void ToggleDamager(bool b)
        {
            meleeDamager.gameObject.SetActive(b);
        }

        public void Death()
        {
            navigator.enabled = false;
            GameManager.instance.GoToNextLevel();
        }

        public void StopCoroutines()
        {
            StopAllCoroutines();
        }

        public void canTakeDamage(bool b)
        {
            bossDamageable.enabled = b;
            collisionDamager.SetActive(b);
        }

        public void SetAnimatorInt(string name, int num)
        {
            bossAnim.SetInteger(name, num);
        }

        public void SetAnimatorFloat(string name, float num)
        {
            bossAnim.SetFloat(name, num);
        }

        public void SetAnimatorBool(string name, bool b)
        {
            bossAnim.SetBool(name, b);
        }
        public int GetAnimatorInt(string name)
        {
            return bossAnim.GetInteger(name);
        }

        public float GetAnimatorFloat(string name)
        {
            return bossAnim.GetFloat(name);
        }

        public bool GetAnimatorBool(string name)
        {
            return bossAnim.GetBool(name);
        }

        public void SetAnimatorTrigger(string n)
        {
            bossAnim.SetTrigger(n);
        }

        public void SetAgentSpeed(float s)
        {
            agent.speed = s;
        }

        public Vector3 BossGetVelocity()
        {
            return agent.velocity;
        }

        public float GetDistanceFromPlayer()
        {
            return Vector3.Distance(player.position, transform.position);
        }

        public void LookAtPlayer()
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }

        public void AgentSetPathToPlayer()
        {
            agent.SetDestination(player.position);
        }

        public void AgentResetPath()
        {
            agent.ResetPath();
        }

    }
}
