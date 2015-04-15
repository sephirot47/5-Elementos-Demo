using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float speed = 8.0f, boostMultiplierForce = 4.0f, boostFading = 0.95f,
				 rotSpeed = 5.0f,
				 jumpForce = 0.2f;

	public bool selected = false;

	private float boostMultiplier = 0.0f;
	private float timeSinceForwardPressed = 0.0f;

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

			Vector3 move = (Camera.main.transform.forward * axisY) + (Camera.main.transform.right * axisX);
			move.y = 0;
			move = move.normalized * speed * Time.deltaTime;

			//BOOST HANDLING
			if(Input.GetKeyDown(KeyCode.W))
			{
				if(timeSinceForwardPressed < 0.3f) Boost();
				timeSinceForwardPressed = 0.0f;
			}
			timeSinceForwardPressed += Time.deltaTime;

			Debug.Log(boostMultiplier);
			if(boostMultiplier < 0.4f) boostMultiplier = 0;
			move += Camera.main.transform.forward * speed * boostMultiplier * Time.deltaTime;
			move.y = 0;
			controller.Move(move);
			/////////////////

			if(move.magnitude > 0)
			{
				Quaternion newRot = Quaternion.LookRotation(move);
				transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * speed);
			}
		}
		boostMultiplier *= boostFading;
		CollisionFlags cf = controller.Move(Vector3.up * Core.gravity * Time.deltaTime); //gravity
	}

	void Boost()
	{
		if(boostMultiplier > 1.0f) return; //Aun no ha acabado el boost anterior
		boostMultiplier = boostMultiplierForce;
		Vector3 move = Camera.main.transform.forward;
		controller.Move(move * speed * boostMultiplierForce * Time.deltaTime);
	}
}
