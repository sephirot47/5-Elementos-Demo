using UnityEngine;
using System.Collections;

public class PlayerAnimationManager : MonoBehaviour 
{
	public static readonly PlayerAnimation 
		Idle0 = new PlayerAnimation("IdleDefault", PlayerAnimation.Priority.Low), 
		Idle1 = new PlayerAnimation("Idle2", PlayerAnimation.Priority.Low),
		
		Run = new PlayerAnimation("Run", PlayerAnimation.Priority.Low), 
		Walk = new PlayerAnimation("Walk", PlayerAnimation.Priority.Low), 
		Jump = new PlayerAnimation("Jump", PlayerAnimation.Priority.Low),
			
		Explosion = new PlayerAnimation("Explosion", PlayerAnimation.Priority.High),
		GuardBegin = new PlayerAnimation("GuardBegin", PlayerAnimation.Priority.High),
		ComboGround = new PlayerAnimation("ComboGround", PlayerAnimation.Priority.High), 
		ComboAerial = new PlayerAnimation("ComboAerial", PlayerAnimation.Priority.High),

		ReceiveDamage = new PlayerAnimation("ReceiveDamage", PlayerAnimation.Priority.VeryHigh), 
		Die = new PlayerAnimation("Die", PlayerAnimation.Priority.VeryHigh);
						      
		private static PlayerAnimation[] playerAnimations = 
		{Idle0, Idle1, Run, Walk, Jump, Explosion, GuardBegin, ComboGround, ComboAerial, ReceiveDamage, Die};

	private Player player;
	private Animation anim;
	private PlayerComboManager playerComboMan;

	public float randomIdleDelay = 5.0f;
	private float idleTime;


	//Si una animacion A es de una prioridad mayor que una animacion B, entonces si
	//B se esta reproduciendo, A puede interrumpirla, si no, no. Si son de igual prioridad, entonces
	//A se esperara a que B acabe.

	void Start () 
	{
		player = GetComponent<Player>();	
		anim = GetComponent<Animation>();
		playerComboMan = GetComponent<PlayerComboManager>();

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
				Play(Run);
			}
			else
			{
				idleTime += Time.deltaTime;

				if(idleTime > randomIdleDelay)
				{
					idleTime = 0.0f;
					Play(Idle1);
				}
				else if(!IsPlaying(Idle1))
				{
					Play(Idle0);
				}
				else
				{
					idleTime = 0.0f;
				}
			}
		}
	}

	public bool IsPlaying(PlayerAnimation animation)
	{
		return anim.IsPlaying(animation.GetName());
	}

	public void Play(PlayerAnimation animation)
	{
		if( player.IsDead() ) return;

		if(CanBePlayed(animation))
		{
			if(!IsPlaying(animation)) 
				anim.Play(animation.GetName());
		}
	}

	public void Stop(PlayerAnimation animation)
	{
		if( player.IsDead() ) return;

		if(IsPlaying(animation)) 
			anim.Stop ();
	}

	private bool CanBePlayed(PlayerAnimation animation)
	{
		string state = animation.GetName();
		PlayerAnimation.Priority p = PlayerAnimation.Priority.Low;
		foreach(PlayerAnimation pa in playerAnimations)
		{
			if(pa.GetName() == state)
			{
				p = pa.GetPriority();
			}
		}

		foreach(PlayerAnimation pa in playerAnimations)
		{
			if(pa.GetPriority() > p && IsPlaying(pa))
			{
				return false; //Si encuentra una animacion con una prioridad mayor, no puede playearse
			}
		}

		return true;
	}
}
