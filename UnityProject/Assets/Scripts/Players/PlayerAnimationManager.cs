using UnityEngine;
using System.Collections;

public class PlayerAnimationManager : MonoBehaviour 
{
	public static readonly PlayerAnimation 
		Idle0 = new PlayerAnimation("IdleDefault"),
		Idle1 = new PlayerAnimation("Idle2"),
		
		Run = new PlayerAnimation("Run"),
		Walk = new PlayerAnimation("Walk"),

        Jump = new PlayerAnimation("Jump"),
        Fall = new PlayerAnimation("Fall"),
        Land = new PlayerAnimation("Land"),
			
		Explosion = new PlayerAnimation("Explosion"),
		GuardBegin = new PlayerAnimation("GuardBegin"),
		ComboGround = new PlayerAnimation("ComboGround"),
        ComboAerial = new PlayerAnimation("ComboAerial"),

		ReceiveDamage = new PlayerAnimation("ReceiveDamage"),
		Die = new PlayerAnimation("Die");
						      

	private Player player;
    private Animation anim;
	private PlayerMovement playerMove;
	private PlayerComboManager playerComboMan;

	public float randomIdleDelay = 15.0f;
	private float idleTime = 0.0f, timeSinceNoJump = float.PositiveInfinity;
    private float landTimeThreshold = 0.2f;


	//Si una animacion A es de una prioridad mayor que una animacion B, entonces si
	//B se esta reproduciendo, A puede interrumpirla, si no, no. Si son de igual prioridad, entonces
	//A se esperara a que B acabe.

	void Start () 
	{
		player = GetComponent<Player>();	
		anim = GetComponent<Animation>();
        playerMove = GetComponent<PlayerMovement>();
		playerComboMan = GetComponent<PlayerComboManager>();
	}
	
	void Update() 
	{
        if (player.IsDead()) { Play(PlayerAnimationManager.Die); return; }
        if (playerComboMan.IsAttacking()) return;
		if(anim == null) return;

        timeSinceNoJump += Time.deltaTime;

		if(!GameState.IsPlaying()) 
		{
			anim.Stop();
			return;
		}

		if ( playerMove.IsGrounded() && !playerComboMan.IsAttacking() )
		{
            Vector2 planeMovement = new Vector2(player.GetMovement().x, player.GetMovement().z);

            if (timeSinceNoJump < 0.1f)
            {
                if (planeMovement.magnitude > 0.05f) ForcePlay(Run);
                else ForcePlay(Idle0);
            }

            if (planeMovement.magnitude > 0.05f)
            {
                if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt  )) Play(Walk);
                else Play(Run);
            }
            else if (!playerComboMan.IsAttacking())
            {
                idleTime += Time.deltaTime;

                if (idleTime > randomIdleDelay)
                {
                    idleTime = 0.0f;
                    Play(Idle1);
                }
                else if (!IsPlaying(Idle1))
                {
                    Play(Idle0);
                }
                else
                {
                    idleTime = 0.0f;
                }
            }
		}
        else
        {
            timeSinceNoJump = 0.0f;
            
            Vector3 movement = player.GetMovement();
            if (!playerMove.IsSecondJumping()) Play(Fall);
            else
            {
                Play(Die);
            }
        }
	}

	public bool IsPlaying(PlayerAnimation animation)
	{
		return anim.IsPlaying(animation.GetName());
	}

    private void ForcePlay(PlayerAnimation animation)
    {
        if (player.IsDead()) return;

        if (!IsPlaying(animation) && anim.GetClip(animation.GetName()) != null)
        {
            anim.CrossFade(animation.GetName());
        }
    }

	public void Play(PlayerAnimation animation)
	{
		if( player.IsDead() ) return;

        if (!IsPlaying(animation) && anim.GetClip(animation.GetName()) != null) 
        {
            anim.CrossFade(animation.GetName());
        }
	}

	public void Stop()
	{
		if( player.IsDead() ) return;
        anim.Stop();
	}

	public static float GetAnimationDuration(PlayerAnimation animation, Player p)
	{
        if( p.GetComponent<Animation>().GetClip(animation.GetName()) != null)
            return p.GetComponent<Animation>().GetClip(animation.GetName()).length;
        return 0.0f;
	}
}
