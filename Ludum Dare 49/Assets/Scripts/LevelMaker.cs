using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelMaker : MonoBehaviour
{
    public Tilemap tilemap;

    [SerializeField]
    public Sprite[] tiles;

    public GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    void CreateLevel()
    {
        foreach (Vector3Int tilePos in tilemap.cellBounds.allPositionsWithin)
        {
            Sprite sprite = tilemap.GetSprite(tilePos);

            for (int i = 0; i < tiles.Length; i++)
            {
                if (sprite == tiles[i]) PlacePieces(tilePos, i);
            }
        }

        tilemap.transform.GetComponent<TilemapRenderer>().enabled = false;
    }

    void PlacePieces(Vector3Int tilePos, int index)
    {
        Instantiate(objects[index], tilemap.CellToWorld(tilePos) + new Vector3(0.5f, index, 0.5f), Quaternion.Euler(-90, 0, 0));
    }
}
