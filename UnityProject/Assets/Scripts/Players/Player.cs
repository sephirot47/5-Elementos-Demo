﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	PlayerAnimationManager anim;

	public float attack = 5.0f;

	public float maxLife = 100.0f;
	private float currentLife;

	private float aggro = 0.0f;

	private GameObject target;

	void Start()
	{
		anim = GetComponent<PlayerAnimationManager>();
		currentLife = maxLife;
	}

	void Update()
	{
		if(!IsSelected()) return;

		if(GameState.IsPlaying() && !GameState.AllPlayersDead())
		{
			if(Input.GetButtonDown("Speak"))
			{
				if(target != null && target.CompareTag("NPC"))
				{
					SpeakTo(target.GetComponent<NPC>());
				}
			}
		}
	}

	private void SpeakTo(NPC npc)
	{
		npc.OnSpeakWithMe();
	}
	
	private void Shoot()
	{
		if(IsDead ()) return;

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

	public float GetAggro() { return aggro; }
	public void OnApplyDamage() { aggro += attack; }
	public float GetCurrentLife() { return currentLife; }
	public float GetMaxLife() { return maxLife; }

	public Vector3 GetMovement() { return GetComponent<PlayerMovement>().movement; }

	public void ReceiveAttack(Enemy e)
	{
		currentLife -= e.GetAttack();
		GetComponent<PlayerComboManager>().OnReceiveDamage();

		if(IsDead() && anim != null)
		{
			Die();
		}

		else if (anim != null) anim.Play(PlayerAnimationManager.ReceiveDamage);
	}
	
	public void SetTarget(GameObject t)
	{
		target = t;
	}

	public GameObject GetTarget()
	{
		return target;
	}

	public void Die()
	{
		currentLife = 0;
		anim.Play(PlayerAnimationManager.Die);
	}

	public bool IsDead()
	{
		return currentLife <= 0;
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
