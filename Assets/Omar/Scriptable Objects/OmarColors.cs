using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    [CreateAssetMenu(menuName = "Omar Colors")]
    public class OmarColors : ScriptableObject
    {
        public List<Color> colors;
    }
}