using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinArrow : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerHealth>().TakeDamage(1);
        }

        Destroy(gameObject);
    }
}
