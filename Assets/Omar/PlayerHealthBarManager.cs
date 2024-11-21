#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Rendering;

namespace Omar
{
    public class PlayerHealthBarManager : MonoBehaviour
    {
        [SerializeField] PlayerHealthDisplay healthDisplay;
        List<Image> hearts;
        [SerializeField] Color heartColor;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(UpdateHealthBar());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator UpdateHealthBar()
        {
            yield return new WaitForSeconds(0.1f);
            healthDisplay.SetMaxHearts(8);
            healthDisplay.UpdateHearts(0, 8);
            foreach (Image heart in healthDisplay.GetComponentsInChildren<Image>())
            {
                heart.color = heartColor;
            }
        }
    }
}
