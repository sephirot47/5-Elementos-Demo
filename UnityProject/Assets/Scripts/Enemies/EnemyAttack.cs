using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	
	Enemy enemy;
	public float attack = 10.0f;
	public float attackRange = 2.0f;
	public float time = 0.0f, attackRate = 1.0f; //Seconds / attack
	public float rotSpeed = 5.0f;

	void Start() 
	{
		enemy = GetComponentInParent<Enemy>();
	}

	void Update () 
	{
		if(!GameState.IsPlaying()) return;

		time += Time.deltaTime;

		Player target = enemy.GetTarget();

		if(target != null) 
		{
			Vector3 movement = Vector3.zero;
			float distanceToTarget = Vector3.Distance(target.gameObject.transform.position, transform.position);

			if(distanceToTarget < attackRange) //Ya esta cerca del player, attaaack!
			{
				if(time >= attackRate)
				{
					Attack(target);
				}
			}
			else //Aun esta lejos del player, a perseguirlo!
			{
				//A perseguir al player que mas aggro tieneeee!		
				Vector3 dir = target.gameObject.transform.position - transform.position;
				dir = new Vector3(dir.x, 0, dir.z);
				
				movement += dir.normalized * enemy.speed;
			}

			//Miramos hacia el jugador
			Quaternion newRot =  Quaternion.LookRotation(Core.PlaneVector(target.transform.position - transform.position));
			transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * rotSpeed);

			enemy.controller.Move(movement * Time.deltaTime);
		}
	}

	public void Attack(Player p)
	{
		p.ReceiveAttack(GetComponent<Enemy>());
		time = 0.0f;
	}
}
