using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    public float musicVol;
    public float sfxVol;
    public bool hasWon = false;
    public int deaths = 0;
    public bool UIAnim = true;
}
