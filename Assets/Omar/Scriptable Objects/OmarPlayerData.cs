using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    [CreateAssetMenu(menuName = "Omar Player Data")]
    public class OmarPlayerData : ScriptableObject
    {
        public float musicVol;
        public float sfxVol;
        public bool hasWon = false;
        public int deaths = 0;
        public bool UIAnim = true;
        public int meleeDmg = 6;
        public int projDmg = 3;
    }
}
