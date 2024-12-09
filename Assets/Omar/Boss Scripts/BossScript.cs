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
        GameObject tempGameobject;
        GameObject[] imSorry;
        [SerializeField] OmarPlayerData playerData;
        [SerializeField] GameObject quizCanvas;
        [SerializeField] OmarQuizHandler quizHandler;
        [SerializeField] OmarBar bossHealthBar;
        [SerializeField] OmarBar energyBar;
        [SerializeField] OmarHUDAnim hudAnim;
        [SerializeField] GameObject bossCollision;
        [SerializeField] GameObject collisionDamager;
        [SerializeField] Damager meleeDamager;
        [SerializeField] Damager dashDamager;
        [SerializeField] OmarDamagerSphere spheresAttack;
        [SerializeField] GameObject[] spheres;
        [SerializeField] OmarLaserBeam laserAttack;
        [SerializeField] GameObject healTrigger;
        [SerializeField] AudioClipCollection hurtSound;
        float bossHPPercentage;
        bool mustIdle = false;
        float tempDelay;
        //[SerializeField][Range(0, 1)] float timeScale = 1;

        // Start is called before the first frame update
        void Awake()
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
                //Debug.Log(agent.remainingDistance);
            }
        }

        public void StartDash()
        {
            StartCoroutine(Dash());
        }

        IEnumerator Dash()
        {
            ToggleCollision(false);
            ToggleCollisionDamager(false);
            SetAnimatorBool("canDash", true);
            while (agent.remainingDistance > 0.5f)
            {
                yield return new WaitForSeconds(1.0f);
            }
            SetAnimatorBool("canDash", false);
            ToggleDashDamager(false);
            ToggleCollision(true);
            ToggleCollisionDamager(true);
            ChangeStateToIdle();
        }

        public void ChangeStateToIdle()
        {
            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        public void StartProjectiles()
        {
            spheresAttack.StartProjAttack();
        }

        public void PerformHeal()
        {
            healTrigger.SetActive(true);
        }

        public void Healing()
        {
            imSorry = FindObjectsOfType<GameObject>();
            foreach (GameObject go in imSorry)
            {
                if (go.name == "New Game Object" && go.GetComponent<AudioSource>() != null)
                {
                    if (go.GetComponent<AudioSource>().clip == hurtSound.clips[0] || go.GetComponent<AudioSource>().clip == hurtSound.clips[1])
                    {
                        tempGameobject = go;
                        //Debug.Log("found");
                        break;
                    }
                }
            }
            imSorry = new GameObject[0];
            tempGameobject.GetComponent<AudioSource>().volume = 0;
            healTrigger.SetActive(false);
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
                spheresAttack.StopAllCoroutines();
                bossAnim.SetInteger("currentPhase", 2);
                myStateMachine.ChangeState(new BossHurtState(myStateMachine));
                StartCoroutine(StartQuiz());
            }
            else if(bossHPPercentage >= 32.5f && bossHPPercentage <= 33 && GetAnimatorInt("currentPhase") == 2)
            {
                spheresAttack.StopAllCoroutines();
                bossAnim.SetInteger("currentPhase", 3);
                myStateMachine.ChangeState(new BossHurtState(myStateMachine));
                StartCoroutine(StartQuiz());
            }
            else if(bossHPPercentage >= 0.5f && bossHPPercentage <= 1 && bossAnim.GetBool("canUlti") == false)
            {
                spheresAttack.StopAllCoroutines();
                bossAnim.SetBool("canUlti", true);
                myStateMachine.ChangeState(new BossHurtState(myStateMachine));
            }
        }

        IEnumerator StartQuiz()
        {
            yield return new WaitForSeconds(2.0f);
            ToggleMeleeDamager(false);
            hudAnim.HideAnim(true, true, false);
            StaticInputManager.input.Disable();
            SetAnimatorBool("mustIdle", true);
            mustIdle = true;
            yield return new WaitForSeconds(1.5f);
            quizCanvas.SetActive(true);
            quizHandler.StartQuestions();
            Debug.Log("Quiz Start!");
        }

        IEnumerator EndQuiz()
        {
            quizCanvas.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            hudAnim.ShowAllAnim(true, true, true);
            StaticInputManager.input.Enable();
            SetAnimatorBool("mustIdle", false);
            mustIdle = false;
            Debug.Log("Quiz End");
        }

        public void EndTheQuiz()
        {
            StartCoroutine(EndQuiz());
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
            ToggleMeleeDamager(true);
            yield return new WaitForSeconds(0.21f);
            ToggleMeleeDamager(false);
            yield return new WaitForSeconds(1.23f);
            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        IEnumerator MeleeTwoDelay()
        {
            yield return new WaitForSeconds(0.95f);
            ToggleMeleeDamager(true);
            yield return new WaitForSeconds(0.11f);
            ToggleMeleeDamager(false);
            yield return new WaitForSeconds(1.4f); //2.3f
            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        IEnumerator MeleeThreeDelay()
        {
            yield return new WaitForSeconds(0.93f);
            ToggleMeleeDamager(true);
            yield return new WaitForSeconds(0.07f);
            ToggleMeleeDamager(false);
            yield return new WaitForSeconds(1.6f);
            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        public void ToggleMeleeDamager(bool b)
        {
            meleeDamager.gameObject.SetActive(b);
        }

        public bool GetMeleeDamagerState()
        {
            return meleeDamager.gameObject.activeInHierarchy;
        }

        public void ToggleDashDamager(bool b)
        {
            dashDamager.gameObject.SetActive(b);
        }

        public void Death()
        {
            navigator.enabled = false;
            if(playerData.hasWon == false)
                playerData.hasWon = true;
            //GameManager.instance.GoToNextLevel();
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

        public void SuccessfulMelee()
        {
            collisionDamager.SetActive(false);
            bossCollision.SetActive(false);
        }

        public void SuccessfulMeleeEnd()
        {
            collisionDamager.SetActive(true);
            bossCollision.SetActive(true);
        }

        public void ToggleCollision(bool b)
        {
            bossCollision.SetActive(b);
        }

        public void ToggleCollisionDamager(bool b)
        {
            collisionDamager.SetActive(b);
        }    

        public void SetAgentStoppingDistance(float num)
        {
            agent.stoppingDistance = num;
        }

        public bool GetMustIdle()
        {
            return mustIdle;
        }

        public void ToggleProjectiles(bool b)
        {
            foreach(GameObject these in spheres)
            {
                these.SetActive(b);
            }
        }

        public void SetDelayFloat(float f)
        {
            tempDelay = f;
        }

        public float GetDelayFloat()
        {
            return tempDelay;
        }

        public void PerformBeam()
        {
            laserAttack.StartBeam();
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

        public Transform GetPlayerTransform()
        {
            return player.transform;
        }

    }
}
