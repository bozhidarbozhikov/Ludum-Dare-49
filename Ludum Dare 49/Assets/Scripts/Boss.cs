using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
    public GameObject hitMarkerPrefab;
    public GameObject firePrefab;

    public List<Vector3> possiblePositions;
    Vector3 movePoint;
    public float moveSpeed = 5f;

    public Vector3 bottomLeftGrid;

    public Vector3[,] battleGrid;
    GameObject[,] hitMarkers;
    GameObject[,] fires;
    public Texture fire1;
    public Texture fire2;
    public Material fireMat;
    public float fireFrameLength;
    public float fireDuration;

    public bool canMove = true;
    public int gridX;
    public int gridY;

    public float stayAtPositionTime = 5f;

    private void Start()
    {
        StartCoroutine(PickRandomPosition());

        MakeGrid();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);
    }

    void MakeGrid()
    {
        battleGrid = new Vector3[gridX, gridY];

        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                battleGrid[i, j] = bottomLeftGrid + new Vector3(i, 0, j);
            }
        }

        hitMarkers = new GameObject[gridX, gridY];
        fires = new GameObject[gridX, gridY];

        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                GameObject hitMarker = Instantiate(hitMarkerPrefab, bottomLeftGrid + new Vector3(i, 0, j), Quaternion.Euler(90, 0, 0));
                hitMarkers[i, j] = hitMarker;
                hitMarker.GetComponent<HitMarker>().SetUpNoDestroy(bottomLeftGrid + new Vector3(i, 0, j));
                hitMarker.GetComponent<MeshRenderer>().enabled = false;

                GameObject fire = Instantiate(firePrefab, bottomLeftGrid + new Vector3(i, 1.33f, j), Quaternion.identity);
                fires[i, j] = fire;
                fire.SetActive(false);
            }
        }

        StartCoroutine(FireAnimate());
    }

    IEnumerator PickRandomPosition()
    {
        yield return new WaitUntil(() => canMove);

        List<Vector3> _possiblePositions = new List<Vector3>(possiblePositions);
        _possiblePositions.Remove(movePoint);

        movePoint = _possiblePositions[Random.Range(0, _possiblePositions.Count)];
        yield return new WaitForSeconds(stayAtPositionTime * 0.75f);

        StartCoroutine(MakeRandomAttack());

        yield return new WaitForSeconds(stayAtPositionTime * 0.25f);

        StartCoroutine(PickRandomPosition());
    }

    IEnumerator MakeRandomAttack()
    {
        yield return null;

        List<Vector3> attackedPositions = new List<Vector3>();

        int pattern = Random.Range(0, 7);

        //int testValue = 2;

        switch (pattern)
        {
            case 0: // shahmat 1
                {
                    for (int i = 0; i < gridX; i++)
                    {
                        for (int j = 0; j < gridY; j++)
                        {
                            if ((i + j) % 2 == 0)// && j % 2 == 0)
                            {
                                AttackOnTile(i, j);

                                attackedPositions.Add(new Vector3(i, 0, j) + bottomLeftGrid);
                            }
                        }
                    }
                }
                break;
            case 1: // shahmat 2
                {
                    for (int i = 0; i < gridX; i++)
                    {
                        for (int j = 0; j < gridY; j++)
                        {
                            if ((i + j) % 2 == 1)// && j % 2 == 0)
                            {
                                AttackOnTile(i, j);

                                attackedPositions.Add(new Vector3(i, 0, j) + bottomLeftGrid);
                            }
                        }
                    }
                }
                break;
            case 2: //prez red
                {
                    for (int i = 0; i < gridX; i++)
                    {
                        if (i % 2 == 1)
                        {
                            continue;
                        }

                        for (int j = 0; j < gridY; j++)
                        {
                            AttackOnTile(i, j);

                            attackedPositions.Add(new Vector3(i, 0, j) + bottomLeftGrid);

                        }
                    }
                }
                break;
            case 3: //prez kolona
                {
                    for (int i = 0; i < gridX; i++)
                    {
                        for (int j = 0; j < gridY; j++)
                        {
                            if (j % 2 == 0)
                            {
                                continue;
                            }
                            AttackOnTile(i, j);

                            attackedPositions.Add(new Vector3(i, 0, j) + bottomLeftGrid);

                        }
                    }
                }
                break;
            case 4: // SHESTDESET I DEVET 69
                {
                    List<Vector2> positions69 = new List<Vector2> { new Vector2(3, 0), new Vector2(2, 5), 
                        new Vector2(1, 1), new Vector2(1, 3), new Vector2(1, 4), new Vector2(2, 3), new Vector2(2, 4),
                        new Vector2(3, 1), new Vector2(3, 2), new Vector2(4, 1), new Vector2(4, 2), new Vector2(4, 4) };

                    for (int i = 0; i < gridX; i++)
                    {
                        for (int j = 0; j < gridY; j++)
                        {
                            if (!positions69.Contains(new Vector2(i, j)))
                            {
                                AttackOnTile(i, j);

                                attackedPositions.Add(new Vector3(i, 0, j) + bottomLeftGrid);
                            }
                        }
                    }
                }
                break;
            case 5: // chetni x2
                {
                    for (int i = 0; i < gridX; i++)
                    {
                        for (int j = 0; j < gridY; j++)
                        {
                            if (i % 2 == 0 && j % 2 == 0)
                            {
                                AttackOnTile(i, j);

                                attackedPositions.Add(new Vector3(i, 0, j) + bottomLeftGrid);
                            }
                        }
                    }
                }
                break;
            case 6: // centur
                {
                    List<Vector2> centur = new List<Vector2> { new Vector2(2, 2), new Vector2(2, 3), new Vector2(3, 3), new Vector2(3, 2) };

                    for (int i = 0; i < gridX; i++)
                    {
                        for (int j = 0; j < gridY; j++)
                        {
                            if (centur.Contains(new Vector2(i, j)))
                            {
                                AttackOnTile(i, j);

                                attackedPositions.Add(new Vector3(i, 0, j) + bottomLeftGrid);
                            }
                        }
                    }
                }
                break;
            case 7: // krai
                {
                    for (int i = 0; i < gridX; i++)
                    {
                        for (int j = 0; j < gridY; j++)
                        {
                            if (!(i > 0 && i < gridX - 1 && j > 0 && j < gridY - 1))
                            {
                                AttackOnTile(i, j);

                                attackedPositions.Add(new Vector3(i, 0, j) + bottomLeftGrid);
                            }
                        }
                    }
                }
                break;
        }

        yield return new WaitForSeconds(1);

        Vector3Int playerPosition = Vector3Int.RoundToInt(new Vector3(player.position.x, 0, player.position.z));

        if (attackedPositions.Contains(playerPosition))
        {
            player.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

    void AttackOnTile(int i, int j)
    {

        StartCoroutine(hitMarkers[i, j].GetComponent<HitMarker>().Show(1));
        StartCoroutine(fires[i, j].GetComponent<FirePoint>().Show(1, fireDuration));
    }

    public IEnumerator Petrify(float duration)
    {
        canMove = false;

        yield return new WaitForSeconds(duration);

        canMove = true;
    }

    IEnumerator FireAnimate()
    {
        if (fireMat.mainTexture == fire1) fireMat.mainTexture = fire2;
        else fireMat.mainTexture = fire1;

        yield return new WaitForSeconds(fireFrameLength);

        StartCoroutine(FireAnimate());
    }

    private void OnDrawGizmosSelected()
    {
        /*foreach (Vector3 position in possiblePositions)
        {
            Gizmos.DrawWireSphere(position, 0.5f);
        }*/
        /*
        foreach (Vector3 position in battleGrid)
        {
            Gizmos.DrawWireSphere(position + Vector3.up, 0.5f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bottomLeftGrid, 0.5f);*/
    }
}
