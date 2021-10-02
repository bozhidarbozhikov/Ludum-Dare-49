using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public int damage;
    public float fireRate;
    private float timer;
    Vector3 shootDir = Vector3.right;
    GameObject[] enemies;
    public GoblinHealth goblinHp;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && timer >= fireRate)
        {
            CheckForEnemyInSight(shootDir);
            timer = 0f;
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
        goblinHp.TakeDamage(damage);
        Debug.Log("Hit");
    }
}
