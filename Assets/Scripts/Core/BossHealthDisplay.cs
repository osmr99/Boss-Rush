using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthDisplay : MonoBehaviour
{
    [SerializeField] Image barFill;

    int maxHealth;

    public void SetMax(int max)
    {
        maxHealth = max;
    }

    public void UpdateBar(int change, int newCurrent)
    {
        barFill.fillAmount = (float)newCurrent/(float)maxHealth;
    }
}
