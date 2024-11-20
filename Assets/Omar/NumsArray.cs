using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    [CreateAssetMenu(menuName = "Nums Array")]
    public class OmarNumsArray : ScriptableObject
    {
        public float timeScale;
        public int startPos;
        public int currentBeat;
        public int tick;
        public int lastPlayedIndex;
        public int[] nums;
    }
}
