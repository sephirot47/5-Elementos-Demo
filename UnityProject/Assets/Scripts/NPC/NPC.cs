using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour 
{
	public float rotSpeed = 1.0f;

	void Start() 
	{
	}
	
	void Update() 
	{
		if(Core.selectedPlayer == null || !GameState.IsPlaying()) return;

		Vector3 from = Core.PlaneVector(transform.position);
		Vector3 to = Core.PlaneVector(Core.selectedPlayer.transform.position);

		Quaternion newRot = Quaternion.LookRotation(to - from, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * rotSpeed);
	}
}