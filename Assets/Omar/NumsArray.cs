using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    [CreateAssetMenu(menuName = "Omar Nums Array")]
    public class OmarNumsArray : ScriptableObject
    {
        public float timeScale;
        public int startPos;
        public int currentBeat;
        public float time;
        public int lastPlayedIndex;
        public bool resetMarkers = false;
        public float[] nums;
    }
}
