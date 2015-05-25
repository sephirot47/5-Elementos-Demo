using UnityEngine;
using System.Collections;

public class PlayerAnimationManager : MonoBehaviour
{
    private Player player;
    private Animation anim;
    private PlayerMovement playerMove;
    private PlayerComboManager playerComboMan;

	public CustomAnimation Idle0, Idle1, Run, Walk, Jump, Fall, Land, Explosion, GuardBegin,
                           ComboGround, ComboGround1, ComboGround2, ComboGround3, ComboGround4,
                           ComboAerial, ReceiveDamage, Die;
						      
	public float randomIdleDelay = 15.0f;
	private float idleTime = 0.0f, timeSinceNoJump = float.PositiveInfinity;

    void Awake()
    {
		player = GetComponent<Player>();	
		anim = GetComponent<Animation>();
        playerMove = GetComponent<PlayerMovement>();
		playerComboMan = GetComponent<PlayerComboManager>();

        Idle0 = new CustomAnimation("IdleDefault", anim);
        Idle1 = new CustomAnimation("Idle2", anim);

        Run = new CustomAnimation("Run", anim);
        Walk = new CustomAnimation("Walk", anim);

        Jump = new CustomAnimation("Jump", anim);
        Fall = new CustomAnimation("Fall", anim);
        Land = new CustomAnimation("Land", anim);

        Explosion = new CustomAnimation("Explosion", anim, 2.0f);
        GuardBegin = new CustomAnimation("GuardBegin", anim);

        ComboGround = new CustomAnimation("ComboGround", anim);
        ComboGround1 = new CustomAnimation("ComboGround1", anim, 1.4f);
        ComboGround2 = new CustomAnimation("ComboGround2", anim, 1.4f);
        ComboGround3 = new CustomAnimation("ComboGround3", anim, 1.4f);
        ComboGround4 = new CustomAnimation("ComboGround4", anim);

        ComboAerial = new CustomAnimation("ComboAerial", anim);

        ReceiveDamage = new CustomAnimation("ReceiveDamage", anim);
        Die = new CustomAnimation("Die", anim);
    }

	void Start() 
	{
	}
	
	void Update() 
	{
        if (player.IsDead()) { Play(Die); return; }
        if (playerComboMan.AnyComboBeingDone()) return;

		if(anim == null) return;

        timeSinceNoJump += Time.deltaTime;

		if(!GameState.IsPlaying()) 
		{
			Stop();
			return;
		}

		if ( playerMove.IsGrounded() )
		{
            Vector2 planeMovement = new Vector2(player.GetMovement().x, player.GetMovement().z);

            if (timeSinceNoJump < 0.1f)
            {
                if (planeMovement.magnitude > 0.05f) Play(Run);
                else Play(Idle0);
            }

            if (planeMovement.magnitude > 0.001f)
            {
                if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt  )) Play(Walk);
                else Play(Run);
            }
            else
            {
                idleTime += Time.deltaTime;

                if (idleTime > randomIdleDelay)
                {
                    idleTime = 0.0f;
                    Play(Idle1);
                }
                else Play(Idle0);
            }
		}
        else
        {
            timeSinceNoJump = 0.0f;
            
            Vector3 movement = player.GetMovement();
            if (playerMove.IsFirstJump()) Play(Fall);
            else if(!playerMove.ComesFromSuspendingInAir()) Play(Die);
        }
	}

	public void Play(CustomAnimation animation)
	{
        if (player.IsDead()) return;
        animation.Play();
	}

    public void Play(CustomAnimation animation, float fadeTime)
    {
        if (player.IsDead()) return;
        animation.Play();
    }

	public void Stop()
	{
		if( player.IsDead() ) return;
        Play(Idle0);
	}

	public static float GetAnimationDuration(CustomAnimation animation, Player p)
	{
        if( p.GetComponent<Animation>().GetClip(animation.GetName()) != null)
            return p.GetComponent<Animation>().GetClip(animation.GetName()).length;
        return 0.0f;
	}
}
