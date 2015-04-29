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
		if(Core.paused) return;

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
		if(Vector3.Distance(transform.position, Core.kaji.gameObject.transform.position) <= visionField) return true;
		if(Vector3.Distance(transform.position, Core.zap.gameObject.transform.position) <= visionField) return true;
		if(Vector3.Distance(transform.position, Core.lluvia.gameObject.transform.position) <= visionField) return true;
		return false;
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
}
