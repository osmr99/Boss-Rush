#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

namespace Omar
{
    public class OmarQuizHandler : MonoBehaviour
    {
        [SerializeField] Canvas canvas;
        [SerializeField] Color enoughTime;
        [SerializeField] Color noTime;
        [SerializeField] TMP_Text question;
        [SerializeField] Image timeBar;
        [SerializeField] GameObject timeBarParent;
        [SerializeField] GameObject tLetter;
        [SerializeField] GameObject fLetter;
        [SerializeField] AudioClipCollection sounds;
        [SerializeField] OmarNums correctAnswersCount;
        [SerializeField] BossScript bossScript;
        [SerializeField] public string[] questionList;
        [SerializeField] bool[] validAnswers;
        [SerializeField] bool[] wasThisQuestionAsked;
        //[SerializeField] float accuracyPercentage = 100;
        [SerializeField] float questionMaxTime;
        [SerializeField] bool timeBarIsActive;
        [SerializeField] bool canAnswer = false;
        [SerializeField] float shakingMultiplier;
        [SerializeField] int maxQuestionsPerPhase;
        Vector3 scaleChange = new Vector3(1.25f, 1.25f, 1.25f);
        bool gamepadConnected = false;
        int numOfAnsweredQuestions;
        float shakingPower;
        float randomX;
        float randomY;
        float elapsedTime;
        float invertedTime;
        float fillMath;
        float otherMath;
        bool coloring = false;
        int randomNum;
        int questionsCount;


        // Start is called before the first frame update
        void Start()
        {
            correctAnswersCount.myInt = 0;
            correctAnswersCount.myFloat = 0;
            numOfAnsweredQuestions = 0;
            canAnswer = false;
            timeBarIsActive = false;
            if (Gamepad.current != null)
                gamepadConnected = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(canAnswer)
            {
                if(gamepadConnected)
                {
                    if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow) || Gamepad.current.rightShoulder.wasPressedThisFrame))
                    {
                        CheckAnswer(true);
                    }
                        
                    else if(Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow) || Gamepad.current.leftShoulder.wasPressedThisFrame))
                    {
                        CheckAnswer(false);
                    }
                }
                        
                else
                {
                    if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow)))
                    {
                        CheckAnswer(true);
                    }
                        
                    else if(Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow)))
                    {
                        CheckAnswer(false);
                    }
                        
                }
            }

            if(timeBarIsActive && canAnswer)
            {
                if(coloring == false)
                {
                    coloring = true;
                    elapsedTime = questionMaxTime;
                    invertedTime = 0;
                    TimeBarColor(questionMaxTime);
                }
                TimeBarDrain();
                ShakingAnim();
            }
        }

        public void StartQuestions()
        {
            canAnswer = true;
            questionsCount = 1;
            AskQuestion();
        }

        void AnswerAnim(GameObject g)
        {
            g.transform.localScale = scaleChange;
            g.transform.DOScale(1, 0.2f).ForceInit();
        }

        public void AskQuestion()
        {
            if(questionsCount <= maxQuestionsPerPhase)
            {
                ResetBar();
                randomNum = Random.Range(0, questionList.Length);
                while (wasThisQuestionAsked[randomNum] == true)
                {
                    randomNum = Random.Range(0, questionList.Length);
                }
                question.text = questionList[randomNum];
                SetMaxTime(randomNum);
                wasThisQuestionAsked[randomNum] = true;
                StartCoroutine(TimeBarDelay());
                //if (numOfAnsweredQuestions > 1)
                //{
                //StartCoroutine(TimeBarDelay());
                //}
            }
            else
            {
                canAnswer = false;
                timeBarIsActive = false;
                bossScript.EndTheQuiz();
            }
        }

        public void CheckAnswer(bool submitted)
        {
            numOfAnsweredQuestions++;
            questionsCount++;
            canAnswer = false;
            if (submitted == true)
                AnswerAnim(tLetter);
            else
                AnswerAnim(fLetter);
            if(submitted == validAnswers[randomNum])
            {
                SoundEffectsManager.instance.PlayAudioClip(sounds.clips[0], true);
                correctAnswersCount.myInt++;
            }
                
            else
            {
                SoundEffectsManager.instance.PlayAudioClip(sounds.clips[1], true);
                bossScript.PerformHeal();
            }
            AskQuestion();
        }

        IEnumerator TimeBarDelay()
        {
            yield return new WaitForSeconds(0.1f);
            canAnswer = true;
            yield return new WaitForSeconds(1.5f);
            timeBarIsActive = true;
        }

        void TimeBarColor(float t)
        {
            timeBar.color = enoughTime;
            timeBar.DOColor(noTime, t).ForceInit();
        }

        void TimeBarDrain()
        {
            if(elapsedTime > 0)
            {
                invertedTime += Time.deltaTime;
                elapsedTime -= Time.deltaTime;
                fillMath = (elapsedTime / questionMaxTime);
                otherMath = (invertedTime / questionMaxTime);
                timeBar.fillAmount = fillMath;
                shakingPower = shakingMultiplier * otherMath;
            }
            else
            {
                SoundEffectsManager.instance.PlayAudioClip(sounds.clips[1], true);
                bossScript.PerformHeal();
                AskQuestion();
            }
        }

        void ShakingAnim()
        {
            randomX = Random.Range(0 + shakingPower, 0 - shakingPower);
            randomY = Random.Range(0 + shakingPower, 0 - shakingPower);
            timeBarParent.transform.localPosition = new Vector2(randomX, randomY);
        }

        void ResetBar()
        {
            if (DOTween.IsTweening(timeBar))
                DOTween.Kill(timeBar);
            StopAllCoroutines();
            timeBar.color = enoughTime;
            timeBarParent.transform.localPosition = new Vector2(0, 0);
            timeBar.fillAmount = 1;
            shakingPower = 0;
            timeBar.color = enoughTime;
            coloring = false;
            timeBarIsActive = false;
        }

        void SetMaxTime(int index)
        {
            switch (index)
            {
                case 0:
                    questionMaxTime = 5;
                    break;
                case 1:
                    questionMaxTime = 14;
                    break;
                case 2:
                    questionMaxTime = 5;
                    break;
                case 3:
                    questionMaxTime = 7;
                    break;
                case 4:
                    questionMaxTime = 9;
                    break;
                case 5:
                    questionMaxTime = 5;
                    break;
                case 6:
                    questionMaxTime = 10;
                    break;
                case 7:
                    questionMaxTime = 6;
                    break;
                case 8:
                    questionMaxTime = 2; //troll
                    break;
                case 9:
                    questionMaxTime = 8;
                    break;
                case 10:
                    questionMaxTime = 7;
                    break;
                case 11:
                    questionMaxTime = 6;
                    break;
                case 12:
                    questionMaxTime = 20; //troll
                    break;
                case 13:
                    questionMaxTime = 20; //troll
                    break;
                case 14:
                    questionMaxTime = 20; //troll
                    break;
                case 15:
                    questionMaxTime = 8;
                    break;
                case 16:
                    questionMaxTime = 10; //troll
                    break;
                case 17:
                    questionMaxTime = 10;
                    break;
                case 18:
                    questionMaxTime = 10;
                    break;
                case 19:
                    questionMaxTime = 15; //troll
                    break;
            }
        }
    }
}