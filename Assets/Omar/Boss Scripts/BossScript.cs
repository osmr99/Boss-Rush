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


namespace Omar
{
    public class BossScript : MonoBehaviour
    {
        Bar bossBar;
        Navigator navigator;
        Transform bossTransform;
        Transform player;
        Rigidbody rb;
        NavMeshAgent agent;
        Animator bossAnim;

        BossStateMachine myStateMachine;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            navigator = GetComponent<Navigator>();
            player = FindObjectOfType<PlayerLogic>().transform;
            bossAnim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            bossTransform = GetComponent<Transform>();

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

        public void Death()
        {
            navigator.enabled = false;
            GameManager.instance.GoToNextLevel();
        }

        public void SetAnimatorSpeed(int s)
        {
            bossAnim.SetFloat("speed", s);
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
