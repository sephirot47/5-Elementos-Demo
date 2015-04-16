using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float speed = 8.0f, boostMultiplierForce = 4.0f, boostFading = 0.95f,
				 rotSpeed = 5.0f,
				 jumpForce = 0.2f;
	private float boostMultiplier = 0.0f;
	private int jumpsDone = 0;

	public bool selected = false;

	private float timeSinceForwardPressed = 0.0f;
	private Vector3 movement = Vector3.zero;
	private CharacterController controller;

	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update () 
	{
		movement = Vector3.zero;
		if(selected) 
		{
			float axisX = Input.GetAxis ("Horizontal"), axisY = Input.GetAxis ("Vertical");

			movement = (Camera.main.transform.forward * axisY) + (Camera.main.transform.right * axisX);
			movement.y = 0;
			movement = movement.normalized * speed;

			//BOOST HANDLING
			if (Input.GetKeyDown (KeyCode.W)) {
				if (timeSinceForwardPressed < 0.3f) Boost();
				timeSinceForwardPressed = 0.0f;
			}
			timeSinceForwardPressed += Time.deltaTime;

			if (boostMultiplier < 0.4f) boostMultiplier = 0;

			movement += Camera.main.transform.forward * speed * boostMultiplier;
			movement.y = 0;
			/////////////////

			if (movement.magnitude > 0) {
				Quaternion newRot = Quaternion.LookRotation(movement);
				transform.rotation = Quaternion.Lerp (transform.rotation, newRot, Time.deltaTime * speed);
			}
			
			if(Input.GetAxis("Jump") > 0 && jumpsDone < 2)
			{
				Jump();
			}
		} 
		else //SIGUEN AL PERSONAJE PRINCIPAL
		{
			Vector3 selectedPlayerPos = Core.selectedPlayer.gameObject.transform.position;
			float toSelectedDist = Vector3.Distance(transform.position, selectedPlayerPos);
			if(toSelectedDist > Core.playerToPlayerFollowDistance)
			{
				Vector3 dir = selectedPlayerPos - transform.position;
				movement += dir.normalized * toSelectedDist;
				movement.y = 0;
				if(movement.magnitude > speed * Time.deltaTime) movement = movement.normalized * speed;
				transform.forward = Vector3.Lerp(transform.forward, movement, Time.deltaTime * rotSpeed);

				//Separamos a los seguidores
				Vector3 otherFollowerPos = Core.GetOtherFollowerPlayer(GetComponent<Player>()).gameObject.transform.position; 
				float dist = Vector3.Distance(transform.position, otherFollowerPos); 
				if(dist < Core.playerToPlayerFollowDistance * toSelectedDist) //Cuanto mas alejados esten del seleccionado, mayor es la repulsion
				{
					//Repulsion entre los seguidores
					Vector3 repulsionDir = transform.position - otherFollowerPos;
					repulsionDir.y = 0;
					movement += repulsionDir.normalized * speed * (toSelectedDist*0.3f)/dist; //Que se separen
				}
				
				if(movement == Vector3.zero)
				{
					Vector3 selectedForward = Core.selectedPlayer.gameObject.transform.forward;
					transform.forward = Vector3.Lerp(transform.forward, selectedForward, Time.deltaTime * rotSpeed);
				}
			}
		}

		boostMultiplier *= boostFading;
		movement += Vector3.up * Core.gravity; //gravity
		if (controller.isGrounded) jumpsDone = 0;
		controller.Move(movement  * Time.deltaTime);
	}

	private void Jump()
	{
		++jumpsDone;
		movement += Vector3.up * jumpForce;
	}

	private void Boost()
	{
		if(boostMultiplier > 1.0f) return; //Aun no ha acabado el boost anterior
		boostMultiplier = boostMultiplierForce;
	}
}
