using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Damageable : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] float iTime = 0.5f;

    public UnityEvent<int> OnInitialize;
    public UnityEvent<Damage> OnHit;
    public UnityEvent OnDeath;
    public UnityEvent<int, int> OnHealthChanged;

    int currentHealth;
    float timeSinceHit = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        OnInitialize?.Invoke(maxHealth);
        OnHealthChanged?.Invoke(maxHealth, maxHealth);
    }

    private void Update()
    {
        timeSinceHit += Time.deltaTime;
    }

    public void Hit(Damage damage)
    {
        Debug.Log("Hit! maybe");
        if (timeSinceHit < iTime)
            return;

        if (currentHealth == 0)
            return;

        timeSinceHit = 0;

        currentHealth -= damage.amount;

        OnHit?.Invoke(damage); // handle any additional hit functions

        OnHealthChanged?.Invoke(damage.amount, currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    void Death()
    {
        OnDeath?.Invoke();
    }

    [ContextMenu("Test Hit")]
    public void TestHit()
    {
        Damage test = new Damage();
        test.amount = 1;
        test.direction = Vector3.zero;
        test.knockbackForce = 0;
        Hit(test);
    }
}
