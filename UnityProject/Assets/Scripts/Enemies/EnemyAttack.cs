using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	
	Enemy enemy;
	public float attack = 10.0f;
	public float attackRange = 2.0f;
	public float time = 0.0f, attackRate = 1.0f; //Seconds / attack

	void Start() 
	{
		enemy = GetComponentInParent<Enemy>();
	}

	void Update () 
	{
		time += Time.deltaTime;

		Player target = enemy.GetTarget();

		if(target != null && time >= attackRate)
		{
			float distanceToTarget = Vector3.Distance(target.gameObject.transform.position, transform.position);
			if(distanceToTarget < attackRange)
			{
				Attack(target);
			}
		}
	}

	public void Attack(Player p)
	{
		p.ReceiveAttack(GetComponent<Enemy>());
		time = 0.0f;
	}
}
