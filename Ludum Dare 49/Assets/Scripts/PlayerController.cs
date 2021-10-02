using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Transform movePoint;

    public LayerMask whatStopsMovement;

    private Vector3 rotateTo;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        transform.rotation = Vector3.RotateTowards();

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (Physics.OverlapSphere(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.33f, whatStopsMovement).Length == 0)
                //if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Horizontal")), 0.6f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }


            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), 0.33f, whatStopsMovement).Length == 0)
                //if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), 0.6f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
                }
            }
        }
    }


}
