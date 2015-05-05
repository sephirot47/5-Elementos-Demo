using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	[HideInInspector] public CharacterController controller;
	public Player target; //Personaje al que esta siguiendo, lo modifica EnemyAggro.cs

	[HideInInspector] public Vector3 movement;

	[SerializeField] protected float maxLife; //vida maxima
	private float currentLife; //vida actual

	public float speed = 7.0f; //velocidad

	void Start() 
	{
		controller = GetComponent<CharacterController>();
		target = null;
		currentLife = maxLife; //Empieza con 100% vida
	}
	
	void Update() 
	{
		if(!GameState.IsPlaying()) return;

		movement = Vector3.zero;
		movement += Vector3.up * Core.gravity; //gravity

		if(currentLife <= 0) Die();

		controller.Move(movement * Time.deltaTime);
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.CompareTag("Projectile")) 
		{
			Projectile p = col.gameObject.GetComponent<Projectile>();
			ReceiveAttack(10.0f, p.shooterPlayer);
			p.shooterPlayer.OnApplyDamage();
		}
	}

	public void ReceiveAttack(float damage, Player player)
	{
		currentLife -= damage;
	}
	
	public Player GetTarget()  { return target; }
	public float GetCurrentLife()  { return currentLife; }
	public float GetMaxLife()  { return maxLife; }
	public float GetAttack() { return GetComponent<EnemyAttack>().attack; }
	public float GetAttackRange() { return GetComponent<EnemyAttack>().attackRange; }
	public float GetAttackRate() { return GetComponent<EnemyAttack>().attackRate; }

	public void Die()
	{
		Destroy(gameObject);
	}
}
 