using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class PathfinderGrid : MonoBehaviour
{
    public Tilemap tilemap;

    public bool onlyDisplayPathGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float checkRadius;

    public enum Movement { FourSides, EightSides }
    public Movement movement;

    [HideInInspector]
    public Node[,] grid;

    [HideInInspector]
    public bool gridCreated = false;

    [HideInInspector]
    public float nodeDiameter;
    int gridSizeX, gridSizeY;

    //public List<Node> path;
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public void CreateGrid()
    {
        gridCreated = false;

        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                if (tilemap.GetTile(tilemap.WorldToCell(worldPoint)))
                {
                    bool walkable = !(Physics.CheckSphere(worldPoint, checkRadius, unwalkableMask));

                    grid[x, y] = new Node(worldPoint, walkable, x, y);
                }
                else
                {
                    grid[x, y] = new Node(worldPoint, false, x, y);
                }
            }
        }
        gridCreated = true;
    }

    public List<Node> GetNodeNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        if (movement == Movement.FourSides)
        {
            neighbours.Add(GetNeighbour(node, Vector2Int.up));
            neighbours.Add(GetNeighbour(node, Vector2Int.down));
            neighbours.Add(GetNeighbour(node, Vector2Int.right));
            neighbours.Add(GetNeighbour(node, Vector2Int.left));
        }
        else if (movement == Movement.EightSides)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
        }

        return neighbours;
    }

    public Node GetNeighbour(Node node, Vector2Int direction)
    {
        int checkX = node.gridX + direction.x;
        int checkY = node.gridY + direction.y;

        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        {
            return grid[checkX, checkY];
        }
        else return null;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;

                Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }

        
        /*if (onlyDisplayPathGizmos)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        else
        {*/

        
    }

}