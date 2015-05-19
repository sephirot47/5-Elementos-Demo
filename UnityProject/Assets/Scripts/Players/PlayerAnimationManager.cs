using UnityEngine;
using System.Collections;

public class PlayerAnimationManager : MonoBehaviour
{
    private Player player;
    private Animation anim;
    private PlayerMovement playerMove;
    private PlayerComboManager playerComboMan;

	public PlayerAnimation Idle0, Idle1, Run, Walk, Jump, Fall, Land, Explosion, GuardBegin,
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

        Idle0 = new PlayerAnimation("IdleDefault", anim);
        Idle1 = new PlayerAnimation("Idle2", anim);

        Run = new PlayerAnimation("Run", anim);
        Walk = new PlayerAnimation("Walk", anim);

        Jump = new PlayerAnimation("Jump", anim);
        Fall = new PlayerAnimation("Fall", anim);
        Land = new PlayerAnimation("Land", anim);

        Explosion = new PlayerAnimation("Explosion", anim);
        GuardBegin = new PlayerAnimation("GuardBegin", anim);

        ComboGround = new PlayerAnimation("ComboGround", anim);
        ComboGround1 = new PlayerAnimation("ComboGround1", anim);
        ComboGround2 = new PlayerAnimation("ComboGround2", anim);
        ComboGround3 = new PlayerAnimation("ComboGround3", anim);
        ComboGround4 = new PlayerAnimation("ComboGround4", anim);

        ComboAerial = new PlayerAnimation("ComboAerial", anim);

        ReceiveDamage = new PlayerAnimation("ReceiveDamage", anim);
        Die = new PlayerAnimation("Die", anim);
    }

	void Start() 
	{
	}
	
	void Update() 
	{
        if (player.IsDead()) { Play(Die); return; }
        if (playerComboMan.IsComboing()) return;
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

            if (planeMovement.magnitude > 0.05f)
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
            if (!playerMove.IsSecondJumping()) Play(Fall);
            else Play(Die);
        }
	}

	public void Play(PlayerAnimation animation)
	{
        if (player.IsDead()) return;
        animation.Play();
	}

    public void Play(PlayerAnimation animation, float fadeTime)
    {
        if (player.IsDead()) return;
        animation.Play();
    }

	public void Stop()
	{
		if( player.IsDead() ) return;
        Play(Idle0);
	}

	public static float GetAnimationDuration(PlayerAnimation animation, Player p)
	{
        if( p.GetComponent<Animation>().GetClip(animation.GetName()) != null)
            return p.GetComponent<Animation>().GetClip(animation.GetName()).length;
        return 0.0f;
	}
}
