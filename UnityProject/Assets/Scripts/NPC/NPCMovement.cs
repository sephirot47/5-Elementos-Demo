using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour 
{
	NPC npc;
	
	public float rotSpeed = 4.0f, visionRange = 10.0f;
	private Quaternion initialRot;

	void Start() 
	{
		npc = GetComponent<NPC>();
		initialRot = transform.rotation;
	}
	
	void Update() 
	{
		if(Core.selectedPlayer == null || !GameState.IsPlaying()) return;
		
		Vector3 from = Core.PlaneVector(transform.position);
		Vector3 to = Core.PlaneVector(Core.selectedPlayer.transform.position);
		
		Quaternion newRot = Quaternion.identity;
		float d = Vector3.Distance(Core.selectedPlayer.transform.position, transform.position);
		
		if(d < visionRange) newRot = Quaternion.LookRotation(to - from, Vector3.up);
		else newRot = initialRot;
		
		transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * rotSpeed);
	}
}
