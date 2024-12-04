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
        [SerializeField] Damager meleeDamager;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            navigator = GetComponent<Navigator>();
            player = FindObjectOfType<PlayerLogic>().transform;
            bossAnim = GetComponent<Animator>();

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
                Debug.Log(Vector3.Distance(player.position, transform.position));
                myStateMachine.Update(); // Elapsed timer
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
            yield return new WaitForSeconds(3.3f);
            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        IEnumerator MeleeThreeDelay()
        {
            yield return new WaitForSeconds(2.6f);
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

        public void SetAnimatorInt(string name, int num)
        {
            bossAnim.SetInteger(name, num);
        }

        public void SetAnimatorFloat(string name, float num)
        {
            bossAnim.SetFloat(name, num);
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
            transform.LookAt(new Vector3(player.position.x, 0, player.position.z));
            //transform.localEulerAngles = new Vector3(0, 0, 0);
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
