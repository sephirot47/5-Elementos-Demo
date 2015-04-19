using UnityEngine;
using System.Collections;

public class EnemyAggro : MonoBehaviour 
{
	Enemy enemy;

	void Start() 
	{
		enemy = GetComponentInParent<Enemy>();
	}
	
	void Update() 
	{
		ChooseTarget();
	}
	
	void ChooseTarget()
	{
		float aggro = Core.kaji.GetAggro();
		enemy.target = Core.kaji;
		if(aggro < Core.zap.GetAggro()) 
		{
			aggro = Core.zap.GetAggro();
			enemy.target = Core.zap;
		}
		if(aggro < Core.lluvia.GetAggro()) enemy.target = Core.lluvia;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.CompareTag("Projectile")) 
		{
			Projectile p = col.gameObject.GetComponent<Projectile>();
			enemy.ReceiveAttack(10.0f, p.shooterPlayer);
			p.shooterPlayer.OnApplyDamage();
		}
	}
}
