using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransition : MonoBehaviour
{
    public Transform cameraHolder;
    public Vector3 cameraPosition;

    public bool playerIn;

    public Pathfinding[] enemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !playerIn)
        {
            Debug.Log("Entered");

            playerIn = true;

            cameraHolder.position = cameraPosition;

            foreach (Pathfinding item in enemies)
            {
                item.canMove = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player") && playerIn)
        {
            Debug.Log("Exited");

            playerIn = false;

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(cameraPosition, 0.5f);
    }

    /*public Transform tilesHolder;
    public Animator holderAnimator;

    public GameObject[] tiles;

    BoxCollider boxCollider;
    public LayerMask layerMask;

    public bool playerIn;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        StartCoroutine(SetUp());

        //StartCoroutine(ShowTiles());
    }

    IEnumerator SetUp()
    {
        yield return new WaitUntil(() => FindObjectOfType<LevelMaker>().madeLevel);

        Collider[] cols = Physics.OverlapBox(transform.position, boxCollider.size/2, Quaternion.identity, layerMask);
        tiles = new GameObject[cols.Length];
        for (int i = 0; i < cols.Length; i++)
        {
            tiles[i] = cols[i].gameObject;
            tiles[i].transform.SetParent(tilesHolder);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !playerIn)
        {
            Debug.Log("Entered");

            playerIn = true;
            holderAnimator.SetTrigger("Expand");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player") && playerIn)
        {
            Debug.Log("Exited");

            playerIn = false;
            holderAnimator.SetTrigger("Shrink");
        }
    }*/
}
