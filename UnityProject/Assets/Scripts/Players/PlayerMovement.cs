using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	Player player;

	public float speed = 16.0f, boostFading = 0.95f,
				 rotSpeed = 5.0f,
				 jumpForce = 25.0f,
				 boostForce = 100.0f;
	
	private int jumpsDone = 0;

	public Vector3 boost = Vector3.zero;
	public Vector3 movement = Vector3.zero;

	private CharacterController controller;

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
        if (GetComponent<PlayerComboManager>().AnyComboBeingDone())
        {
            movement.x = 0.0f;
            movement.z = 0.0f;
            return;
        }

		float axisX = Input.GetAxis ("Horizontal"), 
              axisY = Input.GetAxis ("Vertical");

		float movementY = movement.y; //Lo reestablecemos al final para que no quede afectado por el movimiento en x, z;
		movement = Vector3.zero;
		
        bool walking = (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));
        Vector3 forward = Camera.main.transform.forward, right = Camera.main.transform.right; 
        forward.y = right.y = 0.0f;
        Vector3 dir = ((forward.normalized * axisY) + (right.normalized * axisX)).normalized;
		movement += dir * speed * (walking ? 0.5f : 1.0f);

		movement.y = 0;
		if (movement.magnitude > 0.5f) 
		{
			Quaternion newRot = Quaternion.LookRotation(movement);
			transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * speed);
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
		++jumpsDone;
        movement.y = jumpForce;
	}
	
	public void Boost(Vector3 dir)
	{
		if(boost.magnitude > 0.2f) return; //Aun no ha acabado el boost anterior
		boost = dir * boostForce;
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
}
