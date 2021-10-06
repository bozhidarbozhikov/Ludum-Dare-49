using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker;
    public Transform target;
    public GameObject hitMarkerPrefab;

    public bool canMove = true;
    public bool attacking = false;
    public float moveTime = 1f;
    public float attackDuration = 2f;
    public float attackDelay = 2f;

    PathfinderGrid grid;
    List<Node> path;
    Stair[] stairs;

    public Material goblinMat;
    public Material petrifiedMat;

    Animator animator;

    [HideInInspector]
    public Vector3 movePoint;
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    Coroutine attackCor = null;

    private void Awake()
    {
        grid = FindObjectOfType<PathfinderGrid>();
        movePoint = transform.position;
    }

    private void Start()
    {
        GameObject[] stairObjects = GameObject.FindGameObjectsWithTag("Stair");
        stairs = new Stair[stairObjects.Length];
        for (int i = 0; i < stairObjects.Length; i++)
        {
            stairs[i] = stairObjects[i].GetComponent<Stair>();
        }

        if (transform.childCount != 0)
            animator = transform.GetChild(0).GetComponent<Animator>();

        StartCoroutine(FollowPath());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (canMove)
            FindPath(seeker.position, target.position);
    }

    IEnumerator FollowPath()
    {
        while (canMove)
        {
            yield return new WaitForSeconds(moveTime * 0.5f);

            if (path.Count == 1 && !attacking && Vector3.Distance(transform.position, movePoint) < 0.05f && target != null && Vector3.Distance(transform.position, target.position) < 1.05f)
            {
                attackCor = StartCoroutine(Attack(target.position));
                continue;
            }

            yield return new WaitForSeconds(moveTime * 0.5f);

            if (path.Count == 0)
            {
                List<Vector3> directions = new List<Vector3> { Vector3.up, Vector3.down, Vector3.right, Vector3.left };
                for (int i = 0; i < directions.Count; i++)
                {
                    Vector3 temp = directions[i];
                    int randomIndex = Random.Range(i, directions.Count);
                    directions[i] = directions[randomIndex];
                    directions[randomIndex] = temp;
                }

                for (int i = 0; i < 4; i++)
                {
                    Vector3 direction = directions[i];
                    directions.Remove(direction);

                    if (direction == Vector3.up) direction = Vector3.forward;
                    if (direction == Vector3.down) direction = Vector3.back;

                    if (grid.NodeFromWorldPoint(transform.position + direction).walkable && !IsThereStair(transform.position + direction, direction))
                    {
                        Vector3 newPos = new Vector3(grid.NodeFromWorldPoint(transform.position).worldPosition.x + direction.x, seeker.position.y, grid.NodeFromWorldPoint(transform.position).worldPosition.z + direction.y);
                        movePoint = newPos;
                        break;
                    }
                }
            }
            else if (path.Count > 1)
            {
                bool onStair = false;

                foreach (Stair stair in stairs)
                {
                    if (grid.NodeFromWorldPoint(stair.transform.position) == path[0] && grid.NodeFromWorldPoint(stair.highPoint) == path[1])
                    {
                        onStair = true;

                        if (path[1] != grid.NodeFromWorldPoint(target.position))
                        {
                            movePoint = new Vector3(path[1].worldPosition.x, seeker.position.y + 1, path[1].worldPosition.z);

                            StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(path[0].worldPosition - grid.NodeFromWorldPoint(transform.position).worldPosition)), rotateSpeed, 0));
                        }
                    }
                    else if (grid.NodeFromWorldPoint(stair.transform.position) == path[0] && grid.NodeFromWorldPoint(stair.lowPoint) == path[1])
                    {
                        onStair = true;

                        if (path[1] != grid.NodeFromWorldPoint(target.position))
                        {
                            movePoint = new Vector3(path[1].worldPosition.x, seeker.position.y - 1, path[1].worldPosition.z);

                            StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(path[0].worldPosition - grid.NodeFromWorldPoint(transform.position).worldPosition)), rotateSpeed, 0));
                        }
                    }
                }

                Vector3 nextNodePos = new Vector3(path[0].worldPosition.x, seeker.position.y, path[0].worldPosition.z);

                if (!onStair)
                {
                    movePoint = nextNodePos;

                    StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(path[0].worldPosition - grid.NodeFromWorldPoint(transform.position).worldPosition)), rotateSpeed, 0));
                }
            }

        }
        if (!canMove)
        {
            yield return null;
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator Attack(Vector3 targetPos)
    {
        canMove = false;
        attacking = true;

        GameObject hitMarker = Instantiate(hitMarkerPrefab, Vector3Int.RoundToInt(targetPos), Quaternion.Euler(90, 0, 0));
        hitMarker.GetComponent<HitMarker>().SetUp(Vector3Int.RoundToInt(targetPos), attackDuration);

        StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(path[0].worldPosition - grid.NodeFromWorldPoint(transform.position).worldPosition)), rotateSpeed, 0.25f));

        yield return new WaitForSeconds(attackDuration);

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelay);

        if (path.Count == 0)
        {
            List<Vector3> directions = new List<Vector3> { Vector3.up, Vector3.down, Vector3.right, Vector3.left };
            for (int i = 0; i < directions.Count; i++)
            {
                Vector3 temp = directions[i];
                int randomIndex = Random.Range(i, directions.Count);
                directions[i] = directions[randomIndex];
                directions[randomIndex] = temp;
            }

            for (int i = 0; i < 4; i++)
            {
                Vector3 direction = directions[i];
                directions.Remove(direction);

                if (direction == Vector3.up) direction = Vector3.forward;
                if (direction == Vector3.down) direction = Vector3.back;

                if (grid.NodeFromWorldPoint(transform.position + direction).walkable && !IsThereStair(transform.position + direction, direction))
                {
                    Vector3 newPos = new Vector3(grid.NodeFromWorldPoint(transform.position).worldPosition.x + direction.x, seeker.position.y, grid.NodeFromWorldPoint(transform.position).worldPosition.z + direction.y);
                    movePoint = newPos;
                    break;
                }
            }

            canMove = true;
            attacking = false;

            yield break;
        }

        if (grid.NodeFromWorldPoint(target.position) == grid.NodeFromWorldPoint(targetPos))
        {
            Debug.Log("Hit player");

            yield return new WaitForSeconds(attackDelay);

            FindObjectOfType<PlayerHealth>().TakeDamage(1);
        }


        canMove = true;
        attacking = false;
    }

    public IEnumerator Petrify(float duration)
    {
        canMove = false;
        if (attacking)
            StopCoroutine(attackCor);
        attacking = false;

        Vector3 pos = new Vector3(path[0].worldPosition.x, transform.position.y, path[0].worldPosition.z);

        while (Vector3.Distance(transform.position, pos) > 0.05f)
        {
            yield return null;
        }

        GetComponent<MeshRenderer>().material = petrifiedMat;

        yield return new WaitForSeconds(duration);

        GetComponent<MeshRenderer>().material = goblinMat;

        canMove = true;
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);

                return;
            }

            foreach (Node neighbour in grid.GetNodeNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);

                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;

        this.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) return 14 * dstY + (dstX - dstY) * 10;
        else return 14 * dstX + (dstY - dstX) * 10;
    }

    bool IsThereStair(Vector3 pos, Vector3 dir)
    {
        GameObject[] stairs = GameObject.FindGameObjectsWithTag("Stair");

        foreach (GameObject stair in stairs)
        {
            if (Vector3.Distance(pos, stair.transform.position) < 0.05f) return true;
        }
        return false;
    }

    public IEnumerator LerpFunction(Quaternion endValue, float duration, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endValue;
    }

    public Vector3 GetEulerForRotate(Vector3 direction)
    {
        if (direction == Vector3.forward) return new Vector3(-90, -180, 0);
        if (direction == Vector3.back) return new Vector3(-90, 0, 0);
        if (direction == Vector3.right) return new Vector3(-90, -90, 0);
        if (direction == Vector3.left) return new Vector3(-90, 90, 0);
        else return new Vector3(-90, 0, 0);
    }
}
