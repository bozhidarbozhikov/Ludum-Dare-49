using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    public Transform movePoint;

    public LayerMask whatStopsMovement;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        //transform.eulerAngles = Vector3.RotateTowards(transform.rotation.eulerAngles, rotateTo, 6.28318531f, rotateSpeed * Time.deltaTime);
        //transform.eulerAngles = rotateTo;

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (Physics.OverlapSphere(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.33f, whatStopsMovement).Length == 0)
                //if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Horizontal")), 0.6f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);

                    StopAllCoroutines();
                    StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f))), rotateSpeed));
                }
            }


            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), 0.33f, whatStopsMovement).Length == 0)
                //if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), 0.6f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));

                    StopAllCoroutines();
                    StartCoroutine(LerpFunction(Quaternion.Euler(GetEulerForRotate(new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f))), rotateSpeed));
                }
            }
        }
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
