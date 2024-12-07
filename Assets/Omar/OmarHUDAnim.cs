#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Omar
{
    public class OmarHUDAnim : MonoBehaviour
    {
        [SerializeField] GameObject playerHealthHUD;
        [SerializeField] GameObject playerEnergyHUD;
        [SerializeField] GameObject bossHealthHUD;
        [SerializeField] float theY;
        [SerializeField] float animTime;
        float originPlayerHealth;
        float originPlayerEnergy;
        float originBossHealth;

        private void Awake()
        {
            originPlayerHealth = playerHealthHUD.transform.position.y;
            originPlayerEnergy = playerEnergyHUD.transform.position.y;
            originBossHealth = bossHealthHUD.transform.position.y;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                HideAnim(true, true, true);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                ShowAllAnim(true, true, true);
            }
        }

        public void HideAnim(bool pHealth, bool pEnergy, bool bHealth)
        {
            if (pHealth)
                playerHealthHUD.transform.DOMoveY(playerHealthHUD.transform.position.y + theY, animTime).ForceInit();
            if (pEnergy)
                playerEnergyHUD.transform.DOMoveY(playerEnergyHUD.transform.position.y + theY, animTime).ForceInit();
            if (bHealth)
                bossHealthHUD.transform.DOMoveY(bossHealthHUD.transform.position.y - theY, animTime).ForceInit();
        }

        public void ShowAllAnim(bool pHealth, bool pEnergy, bool bHealth)
        {
            if (pHealth)
                playerHealthHUD.transform.DOMoveY(originPlayerHealth, animTime).ForceInit();
            if (pEnergy)
                playerEnergyHUD.transform.DOMoveY(originPlayerEnergy, animTime).ForceInit();
            if (bHealth)
                bossHealthHUD.transform.DOMoveY(originBossHealth, animTime).ForceInit();
        }
    }
}