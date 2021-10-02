using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController characterController;
	private Transform cam;

	public float moveSpeed = 100;

	//private Animator animator;

	public float turnSmoothTime = 0.1f;
	public float turnSmoothVelocity;


	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		//animator = GetComponentInChildren<Animator>();
	}

	private void Update()
	{
		var horizontal = Input.GetAxisRaw("Horizontal");
		var vertical = Input.GetAxisRaw("Vertical");

		
		Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;


		if(direction.magnitude >= 0.1f)
        {
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);

			Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
			characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
	}
}
