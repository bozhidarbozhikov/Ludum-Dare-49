using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    public IEnumerator Show(float waitTime, float duration)
    {
        yield return new WaitForSeconds(waitTime);

        gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        gameObject.SetActive(false);
    }
}
