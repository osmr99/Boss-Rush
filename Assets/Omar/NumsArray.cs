using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nums Array")]
public class NumsArray : ScriptableObject
{
    public float timeScale;
    public float tick;
    public int index;
    public int[] nums;
}
