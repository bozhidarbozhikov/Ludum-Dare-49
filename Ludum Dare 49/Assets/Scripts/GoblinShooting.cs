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
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowForce;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            if (Mathf.Abs(transform.position.z - player.transform.position.z) < 0.05f)
            {
                Shoot(player.transform.position);
                timer = 0f;
            }
            else if (Mathf.Abs(transform.position.x - player.transform.position.x) < 0.05f)
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
            if (hit.transform.CompareTag("Player"))
            {
                ShootArrow(enemyPosition);
            }
        }
    }

    void ShootArrow(Vector3 position)
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        arrow.transform.LookAt(position, Vector3.up);
        arrow.transform.eulerAngles += new Vector3(90, 0, 0);

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        rb.AddForce((position - firePoint.position).normalized * arrowForce, ForceMode.Impulse);
    }
}
