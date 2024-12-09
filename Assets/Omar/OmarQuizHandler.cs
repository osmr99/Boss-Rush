#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins;
using Unity.VisualScripting;

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
        [SerializeField] public string[] questionList;
        [SerializeField] bool[] validAnswers;
        [SerializeField] float accuracyPercentage = 100;
        [SerializeField] float questionMaxTime;
        [SerializeField] bool timeBarIsActive;
        [SerializeField] float shakingMultiplier;
        int numOfAnsweredQuestions;
        float shakingPower;
        float randomX;
        float randomY;
        [SerializeField] bool[] wasThisQuestionAsked;
        float elapsedTime;
        float invertedTime;
        float fillMath;
        float otherMath;
        bool coloring = false;
        int randomNum;


        // Start is called before the first frame update
        void Start()
        {
            wasThisQuestionAsked = new bool[questionList.Length];
            numOfAnsweredQuestions = 0;
            timeBarIsActive = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(timeBarIsActive)
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

        void AskQuestion()
        {
            while (wasThisQuestionAsked[randomNum] == true)
            {
                randomNum = Random.Range(0, questionList.Length);
            }
            question.text = questionList[randomNum];
            wasThisQuestionAsked[randomNum] = true;
            if(numOfAnsweredQuestions > 2)
            {
                StartCoroutine(TimeBarDelay());

            }
        }

        IEnumerator TimeBarDelay()
        {
            yield return new WaitForSeconds(1.5f);
            timeBarIsActive = true;
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

        void TimeBarColor(float t)
        {
            timeBar.color = enoughTime;
            timeBar.DOColor(noTime, t);
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
                Reset();
            }
        }

        void ShakingAnim()
        {
            randomX = Random.Range(0 + shakingPower, 0 - shakingPower);
            randomY = Random.Range(0 + shakingPower, 0 - shakingPower);
            timeBarParent.transform.localPosition = new Vector2(randomX, randomY);
        }

        void Reset()
        {
            timeBarParent.transform.localPosition = new Vector2(0, 0);
            timeBar.fillAmount = 1;
            shakingPower = 0;
            timeBar.color = enoughTime;
            coloring = false;
            timeBarIsActive = false;
        }
    }
}