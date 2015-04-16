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

	private CharacterController controller;

	void Start() 
	{
		controller = GetComponent<CharacterController>();
	}

	void Update () 
	{
		if (selected) {
			float axisX = Input.GetAxis ("Horizontal"), axisY = Input.GetAxis ("Vertical");

			Vector3 move = (Camera.main.transform.forward * axisY) + (Camera.main.transform.right * axisX);
			move.y = 0;
			move = move.normalized * speed * Time.deltaTime;

			//BOOST HANDLING
			if (Input.GetKeyDown (KeyCode.W)) {
				if (timeSinceForwardPressed < 0.3f)
					Boost ();
				timeSinceForwardPressed = 0.0f;
			}
			timeSinceForwardPressed += Time.deltaTime;

			if (boostMultiplier < 0.4f)
				boostMultiplier = 0;
			move += Camera.main.transform.forward * speed * boostMultiplier * Time.deltaTime;
			move.y = 0;
			controller.Move (move);
			/////////////////

			if (move.magnitude > 0) {
				Quaternion newRot = Quaternion.LookRotation (move);
				transform.rotation = Quaternion.Lerp (transform.rotation, newRot, Time.deltaTime * speed);
			}
			
			if(Input.GetAxis("Jump") > 0 && jumpsDone < 2)
			{
				Jump();
			}

		} 
		else //SIGUEN AL PERSONAJE PRINCIPAL
		{
			Vector3 movement = Vector3.zero;

			Vector3 selectedPlayerPos = Core.selectedPlayer.gameObject.transform.position;
			float toSelectedDist = Vector3.Distance(transform.position, selectedPlayerPos);
			if(toSelectedDist > Core.playerToPlayerFollowDistance)
			{
				Vector3 dir = selectedPlayerPos - transform.position;
				movement += dir.normalized * Time.deltaTime * toSelectedDist;
				movement.y = 0;
				if(movement.magnitude > speed * Time.deltaTime)  
					movement = movement.normalized * speed * Time.deltaTime; 
				transform.forward = Vector3.Lerp(transform.forward, movement, Time.deltaTime * rotSpeed);

				//Separamos a los seguidores
				Vector3 otherFollowerPos = Core.GetOtherFollowerPlayer(GetComponent<Player>()).gameObject.transform.position; 
				float dist = Vector3.Distance(transform.position, otherFollowerPos); 
				if(dist < Core.playerToPlayerFollowDistance * toSelectedDist) //Cuanto mas alejados esten del seleccionado, mayor es la repulsion
				{
					//Repulsion entre los seguidores
					Vector3 repulsionDir = transform.position - otherFollowerPos;
					repulsionDir.y = 0;
					movement += repulsionDir.normalized * speed * Time.deltaTime * (toSelectedDist*0.3f)/dist; //Que se separen
				}
				
				if(movement == Vector3.zero)
				{
					Vector3 selectedForward = Core.selectedPlayer.gameObject.transform.forward;
					transform.forward = Vector3.Lerp(transform.forward, selectedForward, Time.deltaTime * rotSpeed);
				}
			}

			controller.Move(movement);
		}


		boostMultiplier *= boostFading;
		CollisionFlags cf = controller.Move(Vector3.up * Core.gravity * Time.deltaTime); //gravity

		if (cf - CollisionFlags.CollidedBelow == 0) 
		{
			jumpsDone = 0;
		}
	}

	private void Jump()
	{
		++jumpsDone;
		controller.Move(Vector3.up * jumpForce * Time.deltaTime);
	}

	private void Boost()
	{
		if(boostMultiplier > 1.0f) return; //Aun no ha acabado el boost anterior
		boostMultiplier = boostMultiplierForce;
		Vector3 move = Camera.main.transform.forward;
		controller.Move(move * speed * boostMultiplierForce * Time.deltaTime);
	}
}
