using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    public Transform movePoint;
    public Transform belowPoint;

    public LayerMask whatStopsMovement;

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

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                Collider[] checkMP = Physics.OverlapSphere(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.33f, whatStopsMovement);
                Collider[] checkBP = Physics.OverlapSphere(belowPoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.33f, whatStopsMovement);

                if (SteppedOnStair(checkBP))
                {
                    StairDown(checkBP, new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f));
                    return;
                }

                if (checkMP.Length == 0 && checkBP.Length != 0)
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    belowPoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);

                    StopAllCoroutines();
                    StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f))), rotateSpeed));
                }
                else StairUp(checkMP, new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f));
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                Collider[] checkMP = Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), 0.33f, whatStopsMovement);
                Collider[] checkBP = Physics.OverlapSphere(belowPoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), 0.33f, whatStopsMovement);

                if (SteppedOnStair(checkBP))
                {
                    StairDown(checkBP, new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f));
                    return;
                }

                if (checkMP.Length == 0 && checkBP.Length != 0)
                {
                    movePoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
                    belowPoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));

                    StopAllCoroutines();
                    StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f))), rotateSpeed));
                }
                else StairUp(checkMP, new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f));
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

                    if (transform.position.x == stair.lowPoint.x && transform.position.z == stair.lowPoint.z)
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

                    if (transform.position.x == stair.highPoint.x && transform.position.z == stair.highPoint.z)
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

    IEnumerator LerpFunction(Quaternion endValue, float duration)
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

    Vector3 GetEulerForRotate(Vector3 direction)
    {
        if (direction == Vector3.up) return new Vector3(-90, 0, -180);
        if (direction == Vector3.down) return new Vector3(-90, 0, 0);
        if (direction == Vector3.right) return new Vector3(-90, 0, -90);
        if (direction == Vector3.left) return new Vector3(-90, 0, 90);
        else return Vector3.zero;
    }
}