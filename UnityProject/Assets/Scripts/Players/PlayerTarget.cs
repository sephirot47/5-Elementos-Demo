using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTarget : MonoBehaviour 
{
	public float maxTargetDistance = 50.0f;
	public float sensibility = 5.0f;

	private Player player;

	void Start()
	{
		player = GetComponent<Player>();
	}
	
	void Update () 
	{
		if(player.IsDead()) return;
		if(!GameState.IsPlaying() || GameState.AllPlayersDead()) return;

		if(player.IsSelected())
		{
			ChooseTarget();
		}
	}

	void ChooseTarget()
	{
		player.SetTarget(null);
		List<GameObject> targetables = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
		targetables.AddRange(GameObject.FindGameObjectsWithTag("NPC"));

		Vector3 lookPoint =  CameraControl.GetLookPoint();

		if(targetables.Count > 0 && lookPoint != Vector3.zero && Core.selectedPlayer != null)
		{
			//Debug.DrawLine(player.gameObject.transform.position, lookPoint, Color.green, 9999.9f, false);

			//Obtenemos el enemigo mas cerca de donde esta mirando el personaje
			GameObject closestTargetable = targetables[0];
			float minDistance = float.PositiveInfinity;

			foreach(GameObject e in targetables)
			{
				float d = Vector3.Distance(e.transform.position, lookPoint);
				if(d < minDistance)
				{
					closestTargetable = e;
					minDistance = d;
				}
			}
			//

			float distanceToPlayer = Vector3.Distance(Core.selectedPlayer.transform.position, 
			                                          closestTargetable.transform.position);

			if(minDistance < sensibility && 
			   distanceToPlayer < maxTargetDistance)
			{
				player.SetTarget(closestTargetable);
			}
		}
	}
}
