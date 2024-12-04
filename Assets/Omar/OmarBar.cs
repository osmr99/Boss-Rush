using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Omar
{
    public class OmarBar : MonoBehaviour
    {
        Image theBar;
        // Start is called before the first frame update
        void Awake()
        {
            theBar = GetComponent<Image>();
        }
        public float GetBarFill()
        {
            return theBar.fillAmount;
        }
    }
}