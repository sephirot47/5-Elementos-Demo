using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour 
{
	Player player;
	private Animation anim;

	public float randomIdleDelay = 5.0f;
	private float idleTime;

	void Start () 
	{
		player = GetComponent<Player>();	
		anim = GetComponent<Animation>();
		idleTime = 0.0f;
	}
	
	void Update() 
	{
		if(player.IsDead()) return;
		if(anim == null) return;

		if(!GameState.IsPlaying()) 
		{
			anim.Stop();
			return;
		}

		if(!player.IsJumping())
		{
			Vector2 planeMovement = new Vector2(player.GetMovement().x, player.GetMovement().z);
			if(planeMovement.magnitude > 0.1f)
			{
				Play("Run");
			}
			else
			{
				if(!anim.IsPlaying("Combo1") && !anim.IsPlaying("ReceiveDamage") && !anim.IsPlaying("Die"))
				{
					idleTime += Time.deltaTime;

					if(idleTime > randomIdleDelay)
					{
						idleTime = 0.0f;
						Play("Idle2");
					}
					else if(!anim.IsPlaying("Idle2"))
					{
						Play("IdleDefault");
					}
					else
					{
						idleTime = 0.0f;
					}
				}
			}
		}
	}

	public void Play(string state)
	{
		if(anim.IsPlaying("Die") && player.GetCurrentLife() <= 0 ) return;
		if(!anim.IsPlaying("Combo1") && !anim.IsPlaying("ReceiveDamage") && !anim.IsPlaying("Die"))
		{
			if(!anim.IsPlaying(state)) 
				anim.Play(state);
		}
	}

	public void PlayQueued(string state)
	{
		anim.PlayQueued(state);
	}
}
