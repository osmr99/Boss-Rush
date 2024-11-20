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
    public class BossScript : MonoBehaviour
    {

        Bar bossBar;

        // Start is called before the first frame update
        void Start()
        {
            bossBar = FindAnyObjectByType<Bar>();
            bossBar.SetMax(100);
            bossBar.UpdateBar(0,100);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
