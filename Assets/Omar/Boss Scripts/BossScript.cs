#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.AI;
using brolive;
using Unity.VisualScripting;
using System;
using Cinemachine;


namespace Omar
{
    public class BossScript : MonoBehaviour
    {
        GameManager gameManager;
        Bar bossBar;
        Navigator navigator;
        Transform player;
        
        NavMeshAgent agent;
        Animator bossAnim;
        BossStateMachine myStateMachine;
        Damageable bossDamageable;
        GameObject tempGameobjectOne;
        GameObject tempGameobjectTwo;
        GameObject[] imSorry;
        [SerializeField] CinemachineVirtualCamera cam;
        [SerializeField] CharacterController playerCharacterController;
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
        [SerializeField] AudioClipCollection deathSound;
        [SerializeField] OmarWarcryAttack warcryAttack;
        [SerializeField] OmarTeleport teleport;
        [SerializeField] GameObject triggerOfDeath;
        [SerializeField] OmarBGM bgm;
        [SerializeField] OmarMainMenu menu;
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
            gameManager = FindObjectOfType<GameManager>();

            myStateMachine = new BossStateMachine(this);

            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        void OnEnable()
        {
            bossBar = FindAnyObjectByType<Bar>();
            bossBar.SetMax(100);
            bossBar.UpdateBar(0, 100);
            cam.m_Lens.FieldOfView = 60;
            triggerOfDeath.SetActive(false);
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
                yield return new WaitForSeconds(0.1f);//1
            }
            SetAnimatorBool("canDash", false);
            ToggleDashDamager(false);
            ToggleCollision(true);
            ToggleCollisionDamager(true);
            yield return new WaitForSeconds(0.3f);
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

        public void PerformBeam()
        {
            laserAttack.StartBeam();
        }

        public void PerformWarcry()
        {
            warcryAttack.PerformCry();
        }

        public void PerformTeleport()
        {
            teleport.Teleport();
        }

        public void PerformHeal()
        {
            healTrigger.SetActive(true);
        }

        public void Healing()
        {
            tempGameobjectOne = GameObject.Find("Damage Effect(Clone)");
            Destroy(tempGameobjectOne);
            imSorry = FindObjectsOfType<GameObject>();
            foreach (GameObject go in imSorry)
            {
                if (go.name == "New Game Object" && go.GetComponent<AudioSource>() != null)
                {
                    if (go.GetComponent<AudioSource>().clip == hurtSound.clips[0] || go.GetComponent<AudioSource>().clip == hurtSound.clips[1])
                    {
                        tempGameobjectTwo = go;
                        //Debug.Log("found");
                        break;
                    }
                }
            }
            imSorry = new GameObject[0];
            tempGameobjectTwo.GetComponent<AudioSource>().volume = 0;
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
                laserAttack.StopLaserAttack();
                bossAnim.SetInteger("currentPhase", 2);
                myStateMachine.ChangeState(new BossHurtState(myStateMachine));
                StartCoroutine(StartQuiz());
            }
            else if(bossHPPercentage >= 32.5f && bossHPPercentage <= 33 && GetAnimatorInt("currentPhase") == 2)
            {
                spheresAttack.StopAllCoroutines();
                laserAttack.StopLaserAttack();
                bossAnim.SetInteger("currentPhase", 3);
                myStateMachine.ChangeState(new BossHurtState(myStateMachine));
                StartCoroutine(StartQuiz());
            }
            else if(bossHPPercentage >= 0.01f && bossHPPercentage <= 1 && bossAnim.GetBool("canUlti") == false)
            {
                bossHealthBar.LowHealthBoss();
                spheresAttack.StopAllCoroutines();
                laserAttack.StopLaserAttack();
                bossAnim.SetBool("canUlti", true);
                myStateMachine.ChangeState(new BossHurtState(myStateMachine));
                StartCoroutine(StartUlti());
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
            //Debug.Log("Quiz Start!");
        }

        IEnumerator EndQuiz()
        {
            yield return new WaitForSeconds(0.25f);
            quizCanvas.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            hudAnim.ShowAllAnim(true, true, true);
            StaticInputManager.input.Enable();
            SetAnimatorBool("mustIdle", false);
            mustIdle = false;
            //Debug.Log("Quiz End");
        }

        IEnumerator StartUlti()
        {
            yield return new WaitForSeconds(2.0f);
            ToggleMeleeDamager(false);
            StaticInputManager.input.Disable();
            SetAnimatorBool("mustIdle", true);
            mustIdle = true;
        }

        public void PerformFinalBeam()
        {
            laserAttack.StartFinalBeam();
        }

        public void TeleportsForUlti()
        {
            playerCharacterController.enabled = false;
            player.position = new Vector3(13, 2.5f, -13);
            playerCharacterController.gameObject.transform.LookAt(this.transform.position);
            playerCharacterController.enabled = true;
            teleport.TeleportForUlti();
            StartCoroutine(GettingCameraReady());
        }

        public void TeleportBack()
        {
            playerCharacterController.enabled = false;
            player.position = new Vector3(-13, 2.5f, -13);
            playerCharacterController.enabled = true;
        }

        IEnumerator GettingCameraReady()
        {
            while(cam.m_Lens.FieldOfView != 85)
            {
                yield return new WaitForSeconds(0.05f);
                cam.m_Lens.FieldOfView++;
            }
        }

        public void FailedTheUlti()
        {
            StartCoroutine(WillPerish());
        }

        IEnumerator WillPerish()
        {
            yield return new WaitForSeconds(0.01f);
            triggerOfDeath.SetActive(true);
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
            yield return new WaitForSeconds(1);//1.23f
            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        IEnumerator MeleeTwoDelay()
        {
            yield return new WaitForSeconds(0.95f);
            ToggleMeleeDamager(true);
            yield return new WaitForSeconds(0.11f);
            ToggleMeleeDamager(false);
            yield return new WaitForSeconds(1.1f); //1.4f
            myStateMachine.ChangeState(new BossIdleState(myStateMachine));
        }

        IEnumerator MeleeThreeDelay()
        {
            yield return new WaitForSeconds(0.93f);
            ToggleMeleeDamager(true);
            yield return new WaitForSeconds(0.07f);
            ToggleMeleeDamager(false);
            yield return new WaitForSeconds(1.5f);//1.6f
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
            {
                playerData.hasWon = true;
                menu.SavePrefs(playerData.musicVol,
                    playerData.sfxVol,
                    playerData.hasWon,
                    playerData.deathsTook,
                    playerData.deaths,
                    playerData.UIAnim,
                    playerData.meleeDmg,
                    playerData.projDmg);
            }
                
            StartCoroutine(DeathSFX());
        }

        IEnumerator DeathSFX()
        {
            DoCameraShake();
            bgm.StopTheMusic();
            laserAttack.Died();
            warcryAttack.DeathEffect();
            yield return new WaitForSeconds(0.025f);
            imSorry = FindObjectsOfType<GameObject>();
            foreach (GameObject go in imSorry)
            {
                if (go.name == "New Game Object" && go.GetComponent<AudioSource>() != null)
                {
                    if (go.GetComponent<AudioSource>().clip == deathSound.clips[0])
                    {
                        tempGameobjectTwo = go;
                        break;
                    }
                }
            }
            imSorry = new GameObject[0];
            tempGameobjectTwo.GetComponent<AudioSource>().pitch = 1;
            yield return new WaitForSeconds(4.75f);
            //GameManager.instance.GoToNextLevel();
            GameManager.instance.RestartLevelNoSound();
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

        public void ResetAnimatorTrigger(string n)
        {
            bossAnim.ResetTrigger(n);
        }

        public void SetAgentSpeed(float s)
        {
            agent.speed = s;
        }

        public Vector3 BossGetVelocity()
        {
            return agent.velocity;
        }

        public void SetAgentOffset(float f)
        {
            agent.baseOffset = f;
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

        public void DoCameraShake()
        {
            gameManager.ActivateCameraShake();
        }

    }
}
