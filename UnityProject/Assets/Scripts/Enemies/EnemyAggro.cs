using UnityEngine;
using System.Collections;

public class EnemyAggro : MonoBehaviour 
{
	Enemy enemy;
	public float visionField = 15.0f;

	void Start() 
	{
		enemy = GetComponentInParent<Enemy>();
	}
	
	void Update() 
	{
		if(!GameState.IsPlaying()) return;

		if(IsViewingSomebody())
		{
			ChooseTarget();
		}
		else
		{
			enemy.target = null;
		}
	}

	private bool IsViewingSomebody()
	{
		if(Core.kaji != null && !Core.kaji.IsDead() &&
		   Vector3.Distance(transform.position, Core.kaji.transform.position) <= visionField ) return true;
		if(Core.zap != null && !Core.zap.IsDead() &&
		   Vector3.Distance(transform.position, Core.zap.transform.position) <= visionField) return true;
		if(Core.lluvia != null && !Core.lluvia.IsDead() &&
		   Vector3.Distance(transform.position, Core.lluvia.transform.position) <= visionField) return true;
		return false;
	}
	
	void ChooseTarget()
	{
		float aggro = -1.0f;
		enemy.target = null;

		if(!Core.kaji.IsDead() && aggro < Core.kaji.GetAggro()) 
		{
			aggro = Core.kaji.GetAggro();
			enemy.target = Core.kaji;
		}

		if(!Core.zap.IsDead() && aggro < Core.zap.GetAggro()) 
		{
			aggro = Core.zap.GetAggro();
			enemy.target = Core.zap;
		}

		if(!Core.lluvia.IsDead() && aggro < Core.lluvia.GetAggro())
		{
			aggro = Core.lluvia.GetAggro();
			enemy.target = Core.lluvia;
		}
	}
}
