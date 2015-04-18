using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	private CharacterController controller;
	private Player target;

	public float speed = 7.0f;

	void Start () 
	{
		controller = GetComponent<CharacterController>();
		target = null;
	}
	
	void Update () 
	{
		if(target == null) ChooseTarget();

		Vector3 movement = Vector3.zero;
		Vector3 dir = target.gameObject.transform.position - transform.position;

		movement += dir.normalized * speed;
		movement += Vector3.up * Core.gravity; //gravity

		controller.Move(movement * Time.deltaTime);
	}

	void ChooseTarget()
	{
		int rand = Random.Range(0, 2);
		if(rand == 0) target = Core.kaji;
		else if(rand == 1) target = Core.zap;
		else target = Core.lluvia;
	}
}
 