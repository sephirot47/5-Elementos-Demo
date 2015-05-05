using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	private float time = 0.0f;
	public float autoDestructionTime = 5.0f;
	public Player shooterPlayer;
	public Vector3 dir;

	void Start() 
	{
		if(dir == Vector3.zero) Die();
	}

	void Update() 
	{
		if(!GameState.IsPlaying() || GameState.AllPlayersDead()) return;

		transform.position += dir.normalized;
		if(dir != Vector3.zero) transform.forward = dir.normalized;

		time += Time.deltaTime;
		if(time >= autoDestructionTime) Die();
	}

	void OnTriggerEnter(Collider col) 
	{
		if(!col.gameObject.CompareTag("Player") && 
		   !col.gameObject.CompareTag("Projectile")) 
			Die();
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}
