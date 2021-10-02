using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    Vector3 shootDir = Vector3.right;

    GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckForEnemyInSight(shootDir);
        }
    }

    void CheckForEnemyInSight(Vector3 direction)
    {
        List<GameObject> detectedEnemies = new List<GameObject>();
        foreach (GameObject enemy in enemies)
        {
            if (direction.x != 0 && transform.position.z == enemy.transform.position.z)
            {
                Shoot(enemy.transform.position);
            }
            else if (direction.z != 0 && transform.position.x == enemy.transform.position.x)
            {
                Shoot(enemy.transform.position);
            }
        }
    }

    void Shoot(Vector3 enemyPosition)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, enemyPosition - transform.position, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                HitEnemy();
            }
        }
    }

    void HitEnemy()
    {
        Debug.Log("Hit");
    }
}
