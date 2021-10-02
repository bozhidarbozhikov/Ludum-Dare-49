using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    
    public Tilemap tilemap;
    public Sprite[] tiles;


    void Start()
    {
        foreach (Vector3Int tilePos in tilemap.cellBounds.allPositionsWithin)
        {
            Sprite sprite = tilemap.GetSprite(tilePos);

            for (int i = 0; i < tiles.Length; i++)
            {
                if (sprite == tiles[i]) PlacePieces(tilePos, i);
            }
        }

    }

    private void PlacePieces(Vector3Int tilePos, int i)
    {
        switch (i)
        {
            
            case 10:
                {
                    Instantiate(, tilemap.CellToWorld(tilePos) + new Vector3(5, 0, -5), Quaternion.Euler(0, 90, 0));
                }
                break;
            default:
                {
                    //Debug.Log(sprite.name.ToString());
                }
                break;
        }
    }
}
