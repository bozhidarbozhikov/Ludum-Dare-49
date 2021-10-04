using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCloud : MonoBehaviour
{
    public LayerMask layerMask;

    public Material rainMat;
    public Texture rain1;
    public Texture rain2;

    public float rainSwitchTime;

    private void Start()
    {
        StartCoroutine(SwitchRain());
    }

    public void SetUp(Vector3 position, float duration)
    {
        transform.position = position;

        Collider[] cols = Physics.OverlapBox(position, new Vector3(0.1f, 5, 0.1f), Quaternion.identity, layerMask);

        foreach (Collider col in cols)
        {
            transform.position = new Vector3(position.x, col.transform.position.y + 3, position.z);
            break;
        }

        Destroy(gameObject, duration);
    }

    IEnumerator SwitchRain()
    {
        while (true)
        {
            if (rainMat.mainTexture == rain1) rainMat.mainTexture = rain2;
            else rainMat.mainTexture = rain1;

            yield return new WaitForSeconds(rainSwitchTime);
        }
    }
}
