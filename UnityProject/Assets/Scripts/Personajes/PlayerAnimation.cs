using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour 
{
	Player player;
	private Animation anim;

	void Start () 
	{
		player = GetComponent<Player>();	
		anim = GetComponent<Animation>();
	}
	
	void Update () 
	{
		if(anim)
		{
			if(!player.IsJumping())
			{
				Vector2 planeMovement = new Vector2(player.movement.x, player.movement.z);
				if(planeMovement.magnitude > 0.1f)
				{
					Play("Run");
				}
				else
				{
					if(!anim.IsPlaying("Combo1") && !anim.IsPlaying("ReceiveDamage"))
						anim.Stop();
					//Play("Idle");
				}
			}
		}
	}

	public void Play(string state)
	{
		if(!anim.IsPlaying(state))	
			anim.Play(state);
	}
}
