using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float speed = 8.0f, 
				 rotSpeed = 0.2f,
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
			else Camera.main.GetComponent<CameraControl> ().rotationDampeningEnabled = true;

			if(axisY >= 0) transform.rotation *= Quaternion.AngleAxis(rotSpeed * axisX, Vector3.up);
			else  transform.rotation *= Quaternion.AngleAxis(-rotSpeed * axisX, Vector3.up);

			controller.Move(transform.forward * axisY * speed * Time.deltaTime);
		}

		CollisionFlags cf = controller.Move(Vector3.up * Core.gravity * Time.deltaTime); //gravity
	}
}
