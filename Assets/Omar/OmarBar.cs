#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Omar
{
    public class OmarBar : MonoBehaviour
    {
        Image theBar;
        Color theColor;
        [SerializeField] OmarPlayerData playerData;
        bool healing = false;
        // Start is called before the first frame update

        void Update()
        {
            //if(Input.GetKeyDown(KeyCode.J))
            //{
                //Flash();
            //}
        }
        void Awake()
        {
            theBar = GetComponent<Image>();
            theColor = GetComponent<Image>().color;
        }
        public float GetBarFill()
        {
            return theBar.fillAmount;
        }

        public void Flash()
        {
            if(playerData.UIAnim && healing == false)
            {
                theBar.color = Color.white;
                theBar.DOColor(theColor, 0.4f).ForceInit();
            }
        }

        public void Heal()
        {
            healing = true;
            if(playerData.UIAnim && healing == true)
            {
                healing = false;
                theBar.color = Color.green;
                theBar.DOColor(theColor, 0.4f).ForceInit();
            }
        }
    }
}