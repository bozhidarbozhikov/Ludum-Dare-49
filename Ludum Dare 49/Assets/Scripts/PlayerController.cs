using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    public float checkRadius;

    public Transform movePoint;
    public Transform belowPoint;

    public LayerMask whatStopsMovement;

    public bool inverted = false;

    private void OnEnable()
    {
        movePoint.position = transform.position;
        belowPoint.position = transform.position + new Vector3(0f, -1.25f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        belowPoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (inverted)
        {
            float tmp = horizontal;
            horizontal = vertical;
            vertical = tmp;
        }

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(horizontal) == 1f)
            {
                Collider[] checkMP = Physics.OverlapSphere(movePoint.position + new Vector3(horizontal, 0f, 0f), checkRadius, whatStopsMovement);
                Collider[] checkBP = Physics.OverlapSphere(belowPoint.position + new Vector3(horizontal, 0f, 0f), checkRadius, whatStopsMovement);

                if (SteppedOnStair(checkBP))
                {
                    StairDown(checkBP, new Vector3(horizontal, 0f, 0f));
                    return;
                }

                if (checkMP.Length == 0 && checkBP.Length != 0)
                {
                    movePoint.position += new Vector3(horizontal, 0f, 0f);
                    belowPoint.position += new Vector3(horizontal, 0f, 0f);

                    StopAllCoroutines();
                    StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(new Vector3(horizontal, 0f, 0f))), rotateSpeed));
                }
                else StairUp(checkMP, new Vector3(horizontal, 0f, 0f));
            }
            else if (Mathf.Abs(vertical) == 1f)
            {
                Collider[] checkMP = Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, vertical), checkRadius, whatStopsMovement);
                Collider[] checkBP = Physics.OverlapSphere(belowPoint.position + new Vector3(0f, 0f, vertical), checkRadius, whatStopsMovement);

                if (SteppedOnStair(checkBP))
                {
                    StairDown(checkBP, new Vector3(0f, vertical, 0f));
                    return;
                }

                if (checkMP.Length == 0 && checkBP.Length != 0)
                {
                    movePoint.position += new Vector3(0f, 0f, vertical);
                    belowPoint.position += new Vector3(0f, 0f, vertical);

                    StopAllCoroutines();
                    StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(new Vector3(0f, vertical, 0f))), rotateSpeed));
                }
                else StairUp(checkMP, new Vector3(0f, vertical, 0f));
            }
        }
    }

    void StairUp(Collider[] arr, Vector3 rotateDir)
    {
        if (arr.Length != 0)
        {
            foreach (Collider col in arr)
            {
                if (col.CompareTag("Stair"))
                {
                    Stair stair = col.GetComponent<Stair>();

                    if (Mathf.Abs(transform.position.x - stair.lowPoint.x) < 0.05f && Mathf.Abs(transform.position.z - stair.lowPoint.z) < 0.05f)
                    {
                        movePoint.position = stair.highPoint;
                        belowPoint.position = stair.highPoint - Vector3.up * 1.25f;

                        StopAllCoroutines();
                        StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(rotateDir)), rotateSpeed));
                    }
                }
            }
        }
    }
    void StairDown(Collider[] arr, Vector3 rotateDir)
    {
        if (arr.Length != 0)
        {
            foreach (Collider col in arr)
            {
                if (col.CompareTag("Stair"))
                {
                    Stair stair = col.GetComponent<Stair>();

                    if (Mathf.Abs(transform.position.x - stair.highPoint.x) < 0.05f && Mathf.Abs(transform.position.z - stair.highPoint.z) < 0.05f)
                    {
                        movePoint.position = stair.lowPoint;
                        belowPoint.position = stair.lowPoint - Vector3.up * 1.25f;

                        StopAllCoroutines();
                        StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(rotateDir)), rotateSpeed));
                    }
                }
            }
        }
    }

    bool SteppedOnStair(Collider[] arr)
    {
        foreach (Collider col in arr)
        {
            if (col.CompareTag("Stair"))
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator LerpFunction(Quaternion endValue, float duration)
    {
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
        if (direction == Vector3.up) return new Vector3(-90, 0, -180);
        if (direction == Vector3.down) return new Vector3(-90, 0, 0);
        if (direction == Vector3.right) return new Vector3(-90, 0, -90);
        if (direction == Vector3.left) return new Vector3(-90, 0, 90);
        else return Vector3.zero;
    }

    public IEnumerator InvertControls(float duration)
    {
        inverted = true;

        yield return new WaitForSeconds(duration);

        inverted = false;
    }
    public IEnumerator Slow(float duration)
    {
        moveSpeed /= 2;

        yield return new WaitForSeconds(duration);

        moveSpeed *= 2;

    }
}