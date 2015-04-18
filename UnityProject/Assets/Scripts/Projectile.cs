using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public Player shooterPlayer;
	public Vector3 dir;

	void Start() 
	{
		Destroy(gameObject, 3);
	}

	void Update() 
	{
		transform.position += dir;
	}

	void OnCollisionEnter(Collision col) 
	{
		Destroy(gameObject);
	}
}
