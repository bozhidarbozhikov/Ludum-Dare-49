using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    public LayerMask layerMask;

    public void SetUp(Vector3 position, float duration)
    {
        transform.position = position;

        Collider[] cols = Physics.OverlapBox(position, new Vector3(0.1f, 5, 0.1f), Quaternion.identity, layerMask);

        foreach (Collider col in cols)
        {
            transform.position = new Vector3(position.x, col.transform.position.y + 0.55f, position.z);
            break;
        }

        Destroy(gameObject, duration);
    }

    public void SetUpNoDestroy(Vector3 position)
    {
        transform.position = position + new Vector3(0, 0.55f, 0f);
    }

    public IEnumerator Show(float duration)
    {
        GetComponent<MeshRenderer>().enabled = true;

        yield return new WaitForSeconds(duration);

        GetComponent<MeshRenderer>().enabled = false;
    }
}
