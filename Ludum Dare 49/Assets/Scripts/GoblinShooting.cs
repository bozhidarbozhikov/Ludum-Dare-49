using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinShooting : MonoBehaviour
{
    public int damage;
    public float fireRate;
    private float timer;
    public PlayerHealth playerHp;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            if (transform.position.z == player.transform.position.z)
            {
                Shoot(player.transform.position);
                timer = 0f;
            }
            else if (transform.position.x == player.transform.position.x)
            {
                Shoot(player.transform.position);
                timer = 0f;
            }
        }
    }

    void Shoot(Vector3 enemyPosition)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, enemyPosition - transform.position, out hit))
        {

            HitEnemy();

        }
    }

    void HitEnemy()
    {
        playerHp.TakeDamage(damage);
        Debug.Log("Goblin Hit");
    }
}
