using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nums Array")]
public class NumsArray : ScriptableObject
{
    public float timeScale;
    public int startPos;
    public int currentBeat;
    public int tick;
    public int lastPlayedIndex;
    public int[] nums;
}
