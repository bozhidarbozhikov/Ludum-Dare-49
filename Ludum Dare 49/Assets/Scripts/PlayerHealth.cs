using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 3;
    private int currentHealth;

    public MeshRenderer meshRenderer;
    public Material hitMat;

    private void OnEnable()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        StartCoroutine(Blink());

        if (currentHealth <= 0) Die();
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
    }

    IEnumerator Blink()
    {
        Material oldMat = meshRenderer.material;

        meshRenderer.material = hitMat;

        yield return new WaitForSeconds(0.1f);

        meshRenderer.material = oldMat;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}
