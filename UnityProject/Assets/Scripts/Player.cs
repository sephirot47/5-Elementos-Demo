using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float speed = 8.0f, 
				 rotSpeed = 5.0f,
				 jumpForce = 0.2f;

	public bool selected = false;

	private CharacterController controller;

	void Start() 
	{
		controller = GetComponent<CharacterController>();
	}

	void Update () 
	{
		if(selected)
		{
			float axisX = Input.GetAxis("Horizontal"), axisY = Input.GetAxis("Vertical");

			if (axisX == 0 && axisY == 0) Camera.main.GetComponent<CameraControl> ().rotationDampeningEnabled = false;

			Vector3 move = (Camera.main.transform.forward * axisY) + (Camera.main.transform.right * axisX);
			move.y = 0;
			move.Normalize();
			controller.Move(move * speed * Time.deltaTime);

			Quaternion newRot = Quaternion.LookRotation(move);
			transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * speed);
		}

		CollisionFlags cf = controller.Move(Vector3.up * Core.gravity * Time.deltaTime); //gravity
	}
}
