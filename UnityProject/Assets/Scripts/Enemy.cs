using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	private LifeBar lifeBar;
	private CharacterController controller;
	public Player target;

	private float currentLife;

	[SerializeField] //Para que se vea en el inspector ;)
	protected float maxLife;

	public float speed = 7.0f;

	void Start() 
	{
		controller = GetComponent<CharacterController>();
		target = null;

		currentLife = maxLife;
	}
	
	void Update() 
	{
		if(target == null) return;

		Vector3 movement = Vector3.zero;
		Vector3 dir = target.gameObject.transform.position - transform.position;

		movement += dir.normalized * speed;
		movement += Vector3.up * Core.gravity; //gravity
		controller.Move(movement * Time.deltaTime);

		if(currentLife <= 0) Die();
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
 