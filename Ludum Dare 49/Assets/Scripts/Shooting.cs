using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public int damage;
    public float fireRate;
    private float timer;
    GameObject[] enemies;
    PlayerController playerController;

    public Transform cast;
    public Material castMat;
    public Texture[] castSprites;
    public Transform firePoint;
    public float castGFXDuration;
    public float castChangeTimes;

    public float castSpellDuration;

    public GameObject fireballPrefab;
    public GameObject plant;
    public GameObject rainCloudPrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && timer >= fireRate)
        {
            CheckForEnemyInSight(transform.up);
            timer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerController.StopAllCoroutines();
                playerController.StartCoroutine(playerController.LerpFunction(Quaternion.Euler(playerController.GetEulerForRotate(Vector3.left)), playerController.rotateSpeed));
            }
            else
            {
                playerController.StopAllCoroutines();
                playerController.StartCoroutine(playerController.LerpFunction(Quaternion.Euler(playerController.GetEulerForRotate(Vector3.right)), playerController.rotateSpeed));
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                playerController.StopAllCoroutines();
                playerController.StartCoroutine(playerController.LerpFunction(Quaternion.Euler(playerController.GetEulerForRotate(Vector3.down)), playerController.rotateSpeed));
            }
            else
            {
                playerController.StopAllCoroutines();
                playerController.StartCoroutine(playerController.LerpFunction(Quaternion.Euler(playerController.GetEulerForRotate(Vector3.up)), playerController.rotateSpeed));
            }
        }
    }
    void CheckForEnemyInSight(Vector3 direction)
    {
        bool hitEnemy = false;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            if (direction.x != 0 && Mathf.Abs(transform.position.z - enemy.transform.position.z) < 0.05f)
            {
                if (direction.x > 0 && enemy.transform.position.x > transform.position.x)
                {
                    Shoot(enemy.transform.position);
                    hitEnemy = true;
                }
                else if (direction.x < 0 && enemy.transform.position.x < transform.position.x)
                {
                    Shoot(enemy.transform.position);
                    hitEnemy = true;
                }
            }
            else if (direction.z != 0 && Mathf.Abs(transform.position.x - enemy.transform.position.x) < 0.05f)
            {
                if (direction.z > 0 && enemy.transform.position.z > transform.position.z)
                {
                    Shoot(enemy.transform.position);
                    hitEnemy = true;

                }
                else if (direction.z < 0 && enemy.transform.position.z < transform.position.z)
                {
                    Shoot(enemy.transform.position);
                    hitEnemy = true;
                }
            }
        }

        if (!hitEnemy)
        {
            ShootBlank(direction);
        }
    }

    void ShootBlank(Vector3 direction)
    {
        float lengthMultiplier = 5f;

        Vector3 midPoint = (firePoint.position + (firePoint.position + direction * lengthMultiplier)) / 2;
        float distance = Vector3.Distance(firePoint.position, firePoint.position + direction * lengthMultiplier);

        firePoint.LookAt(firePoint.position + direction * lengthMultiplier, Vector3.up);

        cast.gameObject.SetActive(true);
        cast.position = midPoint;
        cast.right = firePoint.forward;
        cast.localScale = new Vector3(distance, 1, 1);
        castMat.mainTextureScale = new Vector2(distance, 1);

        StopAllCoroutines();
        StartCoroutine(CastBlink());

    }

    void Shoot(Vector3 enemyPosition)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, enemyPosition - transform.position, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                HitEnemy(hit.transform);
            }
        }
    }

    void HitEnemy(Transform enemy)
    {
        if (Vector3.Distance(transform.position, enemy.position) < 0.25f) return;

        Collider[] _cols = Physics.OverlapSphere(transform.position, 2);
        List<Collider> cols = new List<Collider>(_cols);
        Collider enemyCol = enemy.GetComponent<Collider>();

        foreach (Collider col in cols)
        {
            if (col.CompareTag("Stair"))
            {
                Stair stair = col.GetComponent<Stair>();

                if (Mathf.Abs(transform.position.y - enemy.position.y) > 0.25f)
                {
                    if (Mathf.Abs(transform.position.x - stair.lowPoint.x) < 0.05f && Mathf.Abs(transform.position.z - stair.lowPoint.z) < 0.05f && cols.Contains(enemyCol))
                    {
                        Debug.LogWarning("Blocking stair!");
                        return;
                    }
                    else if (Mathf.Abs(transform.position.x - stair.highPoint.x) < 0.05f && Mathf.Abs(transform.position.z - stair.highPoint.z) < 0.05f && cols.Contains(enemyCol))
                    {
                        Debug.LogWarning("Blocking stair!");
                        return;
                    }
                }
            }
        }

        Vector3 midPoint = (firePoint.position + enemy.position) / 2;
        float distance = Vector3.Distance(firePoint.position, enemy.position);

        firePoint.LookAt(enemy, Vector3.up);

        cast.gameObject.SetActive(true);
        cast.position = midPoint;
        cast.right = firePoint.forward;
        cast.localScale = new Vector3(distance, 1, 1);
        castMat.mainTextureScale = new Vector2(distance, 1);

        StopAllCoroutines();
        StartCoroutine(CastBlink());

        enemy.GetComponent<GoblinHealth>().TakeDamage(damage);

        int randomEffect = Random.Range(0, 12);

        int testInt = 0;

        switch (testInt)
        {
            case 0:
                {
                    GameObject fireball = Instantiate(fireballPrefab, enemy.position, Quaternion.identity);
                    fireball.GetComponent<Fireball>().SetUp(enemy.position, damage * 5, 2);
                }
                break;
            case 1:
                {
                    StartCoroutine(enemy.GetComponent<Pathfinding>().Petrify(castSpellDuration));

                    Debug.Log("Petrified!");
                }
                break;
            case 2:
                {
                    GetComponent<PlayerHealth>().Heal(1);

                    Debug.Log("Healed!");
                }
                break;
            case 3:
                {

                }
                break;
            case 4:
                {
                    enemy.GetComponent<GoblinHealth>().Heal(2);

                    Debug.Log("Healed goblin!");
                }
                break;
            case 5:
                {
                    StartCoroutine(GetComponent<PlayerController>().InvertControls(castSpellDuration));

                    Debug.Log("Inverted!");
                }
                break;
            case 6:
                {
                    StartCoroutine(GetComponent<PlayerController>().Slow(castSpellDuration));

                    Debug.Log("Slowed!");
                }
                break;
            case 7:
                {
                    StartCoroutine(SwitchPositions(enemy));
                }
                break;
            case 8:
                {
                    StartCoroutine(TurnIntoPlant());
                }
                break;
            case 9:
                {
                    GameObject rainCloud = Instantiate(rainCloudPrefab, enemy.position, Quaternion.identity);
                    rainCloud.GetComponent<RainCloud>().SetUp(enemy.position, castSpellDuration * 2);
                }
                break;
            case 10:
                {

                }
                break;
            case 11:
                {

                }
                break;
            default:
                break;
        }
    }

    IEnumerator SwitchPositions(Transform enemy)
    {
        enemy.GetComponent<Pathfinding>().canMove = false;

        yield return new WaitForSeconds(0.2f);

        Vector3 playerPosition = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
        Vector3 enemyPosition = new Vector3(Mathf.Round(enemy.position.x), enemy.position.y, Mathf.Round(enemy.position.z));

        transform.position = enemyPosition;
        GetComponent<PlayerController>().movePoint.position = enemyPosition;
        GetComponent<PlayerController>().belowPoint.position = enemyPosition - new Vector3(0f, 1.25f, 0f);

        enemy.position = playerPosition;
        enemy.GetComponent<Pathfinding>().movePoint = enemy.position;

        yield return new WaitForSeconds(0.2f);

        enemy.GetComponent<Pathfinding>().canMove = true;
    }

    IEnumerator CastBlink()
    {
        for (int i = 0; i < castChangeTimes; i++)
        {
            yield return new WaitForSeconds(castGFXDuration);

            Texture tex = castSprites[0];

            if (castMat.mainTexture == castSprites[0]) tex = castSprites[1];

            castMat.mainTexture = tex;
        }

        cast.gameObject.SetActive(false);
    }

    IEnumerator TurnIntoPlant()
    {
        plant.SetActive(true);
        GetComponent<MeshRenderer>().enabled = false;
        firePoint.position -= new Vector3(0, 0.015f, 0);

        yield return new WaitForSeconds(castSpellDuration);

        plant.SetActive(false);
        GetComponent<MeshRenderer>().enabled = true;
        firePoint.position += new Vector3(0, 0.015f, 0);
    }
}
