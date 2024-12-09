#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins;

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
        [SerializeField] bool timeBarIsActive = false;
        [SerializeField] float shakingMultiplier;
        float shakingPower;
        float randomX;
        float randomY;
        bool[] wasThisQuestionsAsked;
        float elapsedTime;
        float invertedTime;
        float fillMath;
        float otherMath;
        bool coloring = false;


        // Start is called before the first frame update
        void Start()
        {
            wasThisQuestionsAsked = new bool[questionList.Length];
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