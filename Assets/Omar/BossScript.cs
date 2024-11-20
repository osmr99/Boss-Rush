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
        }

        // Update is called once per frame
        void Update()
        {
            if(agent.enabled == true)
            {
                agent.SetDestination(player.position);
                if(agent.velocity != Vector3.zero)
                {
                    bossAnim.SetFloat("speed", 1);
                }
                else
                {
                    bossAnim.SetFloat("speed", 0);
                }
            }
        }
    }
}
