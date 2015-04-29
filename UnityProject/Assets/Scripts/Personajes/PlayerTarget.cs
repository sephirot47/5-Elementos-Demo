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
		if(Core.paused) return;

		if(player.selected)
		{
			ChooseTarget();
		}
	}

	void ChooseTarget()
	{
		player.target = null;
		List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

		Vector3 lookPoint =  CameraControl.GetLookPoint();

		if(enemies.Count > 0 && lookPoint != Vector3.zero)
		{
			//Debug.DrawLine(player.gameObject.transform.position, lookPoint, Color.green, 9999.9f, false);

			//Obtenemos el enemigo mas cerca de donde esta mirando el personaje
			GameObject closestEnemy = enemies[0];
			float minDistance = float.PositiveInfinity;

			foreach(GameObject e in enemies)
			{
				float d = Vector3.Distance(e.transform.position, lookPoint);
				if(d < minDistance)
				{
					closestEnemy = e;
					minDistance = d;
				}
			}
			//

			float distanceToPlayer = Vector3.Distance(Core.selectedPlayer.transform.position, 
			                                          closestEnemy.transform.position);

			if(minDistance < sensibility && 
			   distanceToPlayer < maxTargetDistance)
			{
				player.target = closestEnemy;
			}
		}
	}
}
