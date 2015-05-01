﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	PlayerAnimation anim;

	public bool selected = false;
	public float attack = 5.0f;

	public float maxLife;
	private float currentLife;

	private float aggro = 0.0f;

	public GameObject target;

	void Start()
	{
		anim = GetComponent<PlayerAnimation>();
		currentLife = maxLife = 100.0f;
	}

	void Update()
	{
		if(Core.paused) return;


	}
	
	private void Shoot()
	{
		Vector3 origin = transform.position + Vector3.up * 1.0f;

		GameObject proj = Instantiate(Resources.Load("Projectile"), 
		                              origin, Quaternion.identity) as GameObject;

		Vector3 shootDir = (CameraControl.GetLookPoint() - origin).normalized;
		if(target != null) 
		{
			shootDir = (target.transform.position - origin).normalized;
		}
		
		proj.GetComponent<Projectile>().dir = shootDir;
		proj.transform.forward = shootDir; 
		proj.GetComponent<Projectile>().shooterPlayer = this;
	}

	//COMBO RELATED STUFF /////////////////////////
	public void OnClickComboDown(KeyCode heldControlKey, int clicksSeguidos, int pressedButton) // 0 = left, 1 = right
	{
		if(heldControlKey == KeyCode.None)
		{
			Shoot();
		}
		else if(heldControlKey == KeyCode.LeftControl)
		{
			anim.Play("Combo1");
		}
		else if(heldControlKey == KeyCode.LeftShift)
		{
			anim.Play("ReceiveDamage");
		}
	}

	public void OnClickCombo(KeyCode heldControlKey, int clicksSeguidos, int pressedButton) //Mantener pulsado
	{

	}

	public void OnKeyComboDone(string comboName)
	{
		if(comboName == "forwardBoost") GetComponent<PlayerMovement>().Boost(Camera.main.transform.forward);
		else if(comboName == "rightBoost") GetComponent<PlayerMovement>().Boost(Camera.main.transform.right);
		else if(comboName == "leftBoost") GetComponent<PlayerMovement>().Boost(-Camera.main.transform.right);
		else if(comboName == "backBoost") GetComponent<PlayerMovement>().Boost(-Camera.main.transform.forward);
	}

	public void OnKeyComboKeyDown(string comboName, KeyCode key)
	{

	}
	///////////////////////////////



	public float GetAggro() { return aggro; }

	public void OnApplyDamage() { aggro += attack; }
	public float GetCurrentLife() { return currentLife; }
	public float GetMaxLife() { return maxLife; }

	public Vector3 GetMovement() { return GetComponent<PlayerMovement>().movement; }

	public void ReceiveAttack(Enemy e)
	{
		currentLife -= e.GetAttack();

		if(currentLife <= 0 && anim != null) anim.Play("Die");
		else if (anim != null) anim.Play("ReceiveDamage");
	}

	public bool IsSelected()
	{
		return Core.selectedPlayer == this;
	}
	
	public bool IsJumping()
	{
		return GetComponent<PlayerMovement>().IsJumping();
	}
}
