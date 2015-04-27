using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	private LifeBar lifeBar;
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
		movement = Vector3.zero;

		if(target != null)
		{	
			//A perseguir al player que mas aggro tieneeee!		
			Vector3 dir = target.gameObject.transform.position - transform.position;
			dir = new Vector3(dir.x, 0, dir.z);

			movement += dir.normalized * speed;
			
			if(currentLife <= 0) Die();
		}
		else {	/* A 'vagar', esta programado en EnemyWander.cs */ }

		movement += Vector3.up * Core.gravity; //gravity
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
	
	public float GetCurrentLife()  { return currentLife; }
	public float GetMaxLife()  { return maxLife; }

	void Die()
	{
		Destroy(gameObject);
	}
}
 