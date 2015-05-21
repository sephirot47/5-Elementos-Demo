using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	void Start() 
	{
	}
	
	void Update() 
	{
		if(!GameState.IsPlaying()) return;
	}

	void LateUpdate()
    {
    }

	void OnTriggerEnter(Collider col)
	{
	}

	public void ReceiveAttack(float damage)
	{
        GetComponent<EnemyCombat>().ReceiveAttack(damage);
	}
	
	public Player GetTarget()  { return GetComponent<EnemyTarget>().GetTarget(); }
	public float GetCurrentLife()  { return GetComponent<EnemyCombat>().GetCurrentLife(); }
    public float GetMaxLife() { return GetComponent<EnemyCombat>().GetMaxLife(); }
    public float GetAttack() { return GetComponent<EnemyCombat>().GetAttack(); }
    public float GetAttackRange() { return GetComponent<EnemyCombat>().GetAttackRange(); }
    public float GetAttackRate() { return GetComponent<EnemyCombat>().GetAttackRate(); }
    public Vector3 GetMovement() { return GetComponent<EnemyMovement>().GetMovement(); }

	public void Die()
	{
		Destroy(gameObject);
	}
}
 