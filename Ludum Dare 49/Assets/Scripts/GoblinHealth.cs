using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHealth : MonoBehaviour
{
    public int startingHealth;
    private int currentHealth;

    private void OnEnable()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

}
