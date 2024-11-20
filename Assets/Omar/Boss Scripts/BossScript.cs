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
        int speed = 0;

        BossStateMachine myStateMachine;

        // Start is called before the first frame update
        void Start()
        {
            bossBar = FindAnyObjectByType<Bar>();
            bossBar.SetMax(100);
            bossBar.UpdateBar(0,100);

            agent = GetComponent<NavMeshAgent>();
            navigator = GetComponent<Navigator>();
            player = FindObjectOfType<PlayerLogic>().transform;
            bossAnim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            bossTransform = GetComponent<Transform>();

            myStateMachine = new BossStateMachine();

            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        // Update is called once per frame
        void Update()
        {
            if(agent.enabled == true)
            {
                agent.SetDestination(player.position);
                if(agent.velocity != Vector3.zero)
                {
                    bossAnim.SetFloat("speed", speed);
                }
                else
                {
                    bossAnim.SetFloat("speed", speed);
                }

                myStateMachine.Update();
            }
        }

        public void SetSpeed(int s)
        {
            speed = s;
        }

        public int GetSpeed()
        {
            return speed;
        }

        public Vector3 GetVelocity()
        {
            return agent.velocity;
        }
    }
}
