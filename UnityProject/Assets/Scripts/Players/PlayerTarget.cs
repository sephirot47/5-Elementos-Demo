using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTarget : MonoBehaviour 
{
	public float maxTargetDistance = 50.0f;
	public float sensibility = 5.0f;
    private Player player;
    private GameObject target;

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
        if(Core.selectedPlayer == null) return;

        target = null;
		List<GameObject> targetables = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
		targetables.AddRange(GameObject.FindGameObjectsWithTag("NPC"));

		Vector3 lookDir =  Core.PlaneVector(Core.selectedPlayer.transform.forward).normalized;

        if (targetables.Count > 0 && lookDir != Vector3.zero)
        {
			//Debug.DrawLine(player.gameObject.transform.position, lookPoint, Color.green, 9999.9f, false);

			//Obtenemos el enemigo mas cerca de donde esta mirando el personaje
			GameObject closestTargetable = targetables[0];
			float minDistance = float.PositiveInfinity;
            
			foreach(GameObject e in targetables)
            {
                float dot = Vector3.Dot(lookDir, (e.transform.position - Core.selectedPlayer.transform.position).normalized);
                if (dot < -0.1f) continue;

                float distanceToPlayer = Vector3.Distance(Core.selectedPlayer.transform.position, e.transform.position);
                float finalD = distanceToPlayer - dot * distanceToPlayer * 0.7f;
                if (finalD < minDistance)
				{
					closestTargetable = e;
                    minDistance = finalD;
				}
			}
			//

			float d = Vector3.Distance(Core.selectedPlayer.transform.position, closestTargetable.transform.position);
			if(d < maxTargetDistance)
			{
                target = closestTargetable;
			}
		}
	}

    public void SetTarget(GameObject t)
    {
        target = t;
    }

    public GameObject GetTarget()
    {
        return target;
    }
}
