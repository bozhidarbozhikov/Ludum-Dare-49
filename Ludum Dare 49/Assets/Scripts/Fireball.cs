using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Material fireballMat;

    public Texture[] textures;

    public float frameTime;

    public void SetUp(Vector3 position, int damage, float radius)
    {
        transform.position = position + Vector3.up/2;

        transform.localScale = Vector3.one * radius;

        StartCoroutine(Explode(radius, damage));
    }

    IEnumerator Explode(float radius, int damage)
    {
        for (int i = 0; i < 6; i++)
        {
            fireballMat.mainTexture = textures[i];

            yield return new WaitForSeconds(frameTime);

            if (i == 1)
            {
                Collider[] cols = Physics.OverlapSphere(transform.position, radius);

                foreach (Collider col in cols)
                {
                    if (col.CompareTag("Enemy"))
                    {
                        col.GetComponent<GoblinHealth>().TakeDamage(damage);
                    }
                }

            }
        }

        Destroy(gameObject);
    }
}
