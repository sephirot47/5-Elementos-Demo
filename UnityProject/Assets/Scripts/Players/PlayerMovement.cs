using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	Player player;

	public float speed = 16.0f, boostFading = 0.95f,
				 rotSpeed = 5.0f,
				 jumpForce = 25.0f,
				 boostForce = 2.0f;
	
	private int jumpsDone = 0;

    public GameObject ground;

	public Vector3 boost = Vector3.zero;
	public Vector3 movement = Vector3.zero;

	private CharacterController controller;

	void Start ()
	{
		player = GetComponent<Player>();
		controller = GetComponent<CharacterController>();

        ground = Core.GetSubGameObject(this.gameObject, "Base Human");
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
		movement.y += Core.gravity * Time.fixedDeltaTime; //gravity
	}
	
	
	private void SelectedMove()
	{
        if (GetComponent<PlayerComboManager>().IsComboing())
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
		Vector3 dir = ((Camera.main.transform.forward * axisY) + (Camera.main.transform.right * axisX)).normalized;
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
	
	public bool IsJumping()
	{
		return jumpsDone > 0;
	}

    public bool IsSecondJumping()
    {
        return jumpsDone == 2;
    }

	public bool IsGrounded()
	{
		RaycastHit hit;
        if(ground != null)
        {
            Physics.CheckSphere(ground.transform.position, 0.1f, ~(1 << LayerMask.NameToLayer("Player")) );
        }
        return Physics.Raycast( controller.transform.position, Vector3.down, out hit, 0.1f, ~(1 << LayerMask.NameToLayer("Player")));
	}
}
