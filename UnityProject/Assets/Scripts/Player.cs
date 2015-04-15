using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public CharacterController controller;

	void Start() 
	{
		controller = GetComponent<CharacterController> ();
	}

	void Update () 
	{
		controller.Move(Vector3.up * Core.gravity * Time.deltaTime);
		controller.Move(Vector3.forward * 0.1f);
	}
}
