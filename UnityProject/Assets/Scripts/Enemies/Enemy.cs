using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	[HideInInspector] public CharacterController controller;
	public Player target; //Personaje al que esta siguiendo, lo modifica EnemyAggro.cs

	[HideInInspector] public Vector3 movement;

	public float speed = 7.0f; //velocidad

	void Start() 
	{
		controller = GetComponent<CharacterController>();
		target = null;
	}
	
	void Update() 
	{
		if(!GameState.IsPlaying()) return;

		movement = Vector3.zero;
		movement += Vector3.up * Core.gravity; //gravity

		controller.Move(movement * Time.deltaTime);
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.CompareTag("Projectile")) 
		{
			Projectile p = col.gameObject.GetComponent<Projectile>();
			ReceiveAttack(10.0f);
			p.shooterPlayer.OnApplyDamage();
		}
	}

	public void ReceiveAttack(float damage)
	{
        GetComponent<EnemyCombat>().ReceiveAttack(damage);
	}
	
	public Player GetTarget()  { return target; }
	public float GetCurrentLife()  { return GetComponent<EnemyCombat>().GetCurrentLife(); }
    public float GetMaxLife() { return GetComponent<EnemyCombat>().GetMaxLife(); }
    public float GetAttack() { return GetComponent<EnemyCombat>().GetAttack(); }
    public float GetAttackRange() { return GetComponent<EnemyCombat>().GetAttackRange(); }
    public float GetAttackRate() { return GetComponent<EnemyCombat>().GetAttackRate(); }

	public void Die()
	{
		Destroy(gameObject);
	}
}
 