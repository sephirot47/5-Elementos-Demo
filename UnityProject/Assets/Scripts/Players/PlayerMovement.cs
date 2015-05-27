using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	Player player;

    public float speed = 16.0f, speedFadeSpeed = 5.0f, boostFading = 0.95f,
				 rotSpeed = 5.0f,
				 jumpForce = 25.0f,
				 boostForce = 100.0f;
	
	private int jumpsDone = 0;

	public Vector3 boost = Vector3.zero;
	public Vector3 movement = Vector3.zero;

	private CharacterController controller;

    private bool movementLockedToTarget = false;
    private bool suspendedInAir = false;

	void Start ()
	{
		player = GetComponent<Player>();
		controller = GetComponent<CharacterController>();
	}

	void Update ()
    {
        if (IsGrounded() && player.IsSelected()) { jumpsDone = 0; }
		if(!GameState.IsPlaying() || GameState.AllPlayersDead()) return;
		if(player.IsDead()) return;

		if(player.IsSelected())
		{
			SelectedMove();
			if(Input.GetKeyDown(KeyCode.Space) && jumpsDone < 2) Jump();
		}
		else FollowSelected(); //SIGUEN AL PERSONAJE SELECCIONADO

		controller.Move((movement + boost * speed) * Time.deltaTime);
		//
	}

	void FixedUpdate()
	{
		if(!GameState.IsPlaying() || GameState.AllPlayersDead()) return;
		
		boost *= boostFading;
        if (!suspendedInAir)
        {
            movement.y += Core.gravity * Time.fixedDeltaTime; //gravity
        }
	}
	
	private void SelectedMove()
	{
		float axisX = Input.GetAxis ("Horizontal"), 
              axisY = Input.GetAxis ("Vertical");

        if (GetComponent<PlayerComboManager>().IsComboingCombo("chargedJump"))
        {
            Vector3 forwardd = Camera.main.transform.forward, rightt = Camera.main.transform.right;
            forwardd.y = rightt.y = 0.0f;
            Vector3 dirr = ((forwardd.normalized * axisY) + (rightt.normalized * axisX)).normalized;
            Quaternion newRot = Quaternion.LookRotation(dirr);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * rotSpeed);
        }

        if (GetComponent<PlayerComboManager>().AnyComboBeingDone() || GetComponent<PlayerAreaAttack>().IsInAreaMode())
        {
            movement.x = 0.0f;
            movement.z = 0.0f;
            return;
        }

        /* stop fade
        if(axisX == 0.0f && axisY == 0.0f)
        {
            Vector3 movementStopped = new Vector3(0.0f, movement.y, 0.0f);
            movement = Vector3.Lerp(movement, movementStopped, Time.deltaTime * speedFadeSpeed);
            return;
        }*/

		float movementY = movement.y; //Lo reestablecemos al final para que no quede afectado por el movimiento en x, z;
		movement = Vector3.zero;
		
        bool walking = (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));
        Vector3 forward = Camera.main.transform.forward, right = Camera.main.transform.right; 
        forward.y = right.y = 0.0f;
        Vector3 dir = Core.PlaneVector(((forward.normalized * axisY) + (right.normalized * axisX))).normalized;
		movement += dir * speed * (walking ? 0.5f : 1.0f);

        if(dir.magnitude > 0.05)
        {
		    movement.y = 0;
            if (movement.magnitude > 0.05f)
            {
                Quaternion newRot = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * speed);
            }
            else transform.forward = dir;
        }
		
		movement.y = movementY; //Reestablecido
	}
	
	private void FollowSelected()
	{
		if(Core.selectedPlayer == null) return;
		
		float movementY = movement.y; //Lo reestablecemos al final para que no quede afectado por el movimiento en x, z;
		movement = Vector3.zero;
		
		Vector3 selectedPlayerPos = Core.selectedPlayer.gameObject.transform.position;
		float distanceToSelected = Vector3.Distance(transform.position, selectedPlayerPos);
		
		if(distanceToSelected > Core.playerToPlayerFollowDistance)
		{
			//Ve hacia al jugador
			Vector3 dir = (selectedPlayerPos - transform.position); 
			dir.y = 0; dir.Normalize();
			
			movement += dir * distanceToSelected;
			if(movement.magnitude > speed) movement = dir * speed;
			movement.y = 0;
			//
			
			//Separamos a los seguidores
			Vector3 otherFollowerPos = Core.GetOtherFollowerPlayer(GetComponent<Player>()).gameObject.transform.position; 
			float dist = Vector3.Distance(transform.position, otherFollowerPos); 
			
			//Repulsion entre los seguidores
			Vector3 repulsionDir = transform.position - otherFollowerPos;
			repulsionDir.y = 0; repulsionDir.Normalize();
			movement += repulsionDir.normalized * speed * 1.0f/dist;
			
			//Modificamos hacia donde miran
			if(movement == Vector3.zero) //Si estan quietos, que miren donde mira el pj seleccionado
			{
				Vector3 selectedForward = Core.selectedPlayer.gameObject.transform.forward;
				transform.forward = Vector3.Lerp(transform.forward, selectedForward, Time.deltaTime * rotSpeed);
			}
			else transform.forward = Vector3.Lerp(transform.forward, movement, Time.deltaTime * rotSpeed);
		}
		
		movement.y = movementY; //Reestablecido
	}
	
	private void Jump()
	{
        if (GetComponent<PlayerComboManager>().AnyComboBeingDone() || GetComponent<PlayerAreaAttack>().IsInAreaMode())
            return;
        
        ++jumpsDone;
        movement.y = jumpForce;
	}
	
	public void Boost(Vector3 dir, float multiplier = 1.0f)
	{
		if(boost.magnitude > 0.2f) return; //Aun no ha acabado el boost anterior
		boost = dir * boostForce * multiplier;
	}

    public void SetSuspendedInAir(bool suspended)
    {
        suspendedInAir = suspended;
        if(suspended)
        {
            jumpsDone = 3; // SIIII 3 !!!! Para que haga la caida en Fall, y no en la animacion del 2o salto!
            movement.y = 0.0f;
        }
    }

	public bool IsGrounded()
	{
        return controller.isGrounded;
	}

    public void LookToTarget()
    {
        transform.forward = Core.PlaneVector(player.GetTarget().transform.position - transform.position);
    }

    public bool IsJumping()
    {
        return jumpsDone > 0;
    }

    public bool IsFirstJump()
    {
        return jumpsDone == 1;
    }

    public bool IsSecondJump()
    {
        return jumpsDone == 2;
    }

    public bool ComesFromSuspendingInAir()
    {
        return jumpsDone == 3;
    }

    public void SetMovementLockedToTarget()
    {

    }

    internal void SetMovementLocked(bool p)
    {
        throw new System.NotImplementedException();
    }
}
